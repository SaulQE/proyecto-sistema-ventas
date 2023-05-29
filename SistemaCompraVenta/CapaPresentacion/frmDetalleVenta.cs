using CapaEntidad;
using CapaNegocio;
using iTextSharp.text;
using iTextSharp.text.pdf;
using iTextSharp.tool.xml;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CapaPresentacion
{
    public partial class frmDetalleVenta : Form
    {
        public frmDetalleVenta()
        {
            InitializeComponent();
        }

        private void frmDetalleVenta_Load(object sender, EventArgs e)
        {
            txtbusqueda.Select();
        }

        private void btnbuscar_Click(object sender, EventArgs e)
        {
            Venta oVenta = new CN_Venta().ObtenerVenta(txtbusqueda.Text);

            if (oVenta.id_Venta != 0)
            {
                txtnumerodocumento.Text = oVenta.Nro_Documento;

                txtfecha.Text = oVenta.F_Registro;
                txttipodocumento.Text = oVenta.Tipo_Documento;
                txtusuario.Text = oVenta.oUsuario.Nom_Completo;
                txtdoccliente.Text = oVenta.Documento_Cliente;
                txtnombrecliente.Text = oVenta.Nom_Cliente;
                dgvdata.Rows.Clear();

                foreach(Detalle_Venta dv in oVenta.oDetalle_Venta)
                {
                    dgvdata.Rows.Add(new object[] { dv.oProducto.Nombre, dv.Precio_Venta, dv.Cantidad, dv.SubTotal });
                }

                txtmontototal.Text = oVenta.Monto_Total.ToString("0.00");
                txtmontopago.Text = oVenta.Monto_Pago.ToString("0.00");
                txtmontocambio.Text = oVenta.Monto_Cambio.ToString("0.00");

            }

        }

        private void btnlimpiar_Click(object sender, EventArgs e)
        {
            txtfecha.Text = "";
            txttipodocumento.Text = "";
            txtusuario.Text = "";
            txtdoccliente.Text = "";
            txtnombrecliente.Text = "";

            dgvdata .Rows.Clear();
            txtmontototal.Text = "0.00";
            txtmontopago.Text = "0.00";
            txtmontocambio.Text = "0.00";

        }

        private void btnexportar_Click(object sender, EventArgs e)
        {
            // Verifica si se ha ingresado un tipo de documento
            if (txttipodocumento.Text == "")
            {
                MessageBox.Show("No se encontraron resultados", "Mensaje", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }

            // Obtiene el contenido del archivo de plantilla HTML y reemplaza las etiquetas con los valores correspondientes
            string Texto_Html = Properties.Resources.PlantillaVenta.ToString();
            Negocio odatos = new CN_Negocio().ObtenerDatos();

            Texto_Html = Texto_Html.Replace("@nombrenegocio", odatos.Nombre.ToUpper());
            Texto_Html = Texto_Html.Replace("@docnegocio", odatos.RUC);
            Texto_Html = Texto_Html.Replace("@direcnegocio", odatos.Direccion);

            Texto_Html = Texto_Html.Replace("@tipodocumento", txttipodocumento.Text.ToUpper());
            Texto_Html = Texto_Html.Replace("@numerodocumento", txtnumerodocumento.Text);

            Texto_Html = Texto_Html.Replace("@doccliente", txtdoccliente.Text);
            Texto_Html = Texto_Html.Replace("@nombrecliente", txtnombrecliente.Text);
            Texto_Html = Texto_Html.Replace("@fecharegistro", txtfecha.Text);
            Texto_Html = Texto_Html.Replace("@usuarioregistro", txtusuario.Text);

            // Construye las filas de la tabla HTML con los detalles de compra obtenidos del DataGridView
            string filas = string.Empty;
            foreach (DataGridViewRow row in dgvdata.Rows)
            {
                // Construye una nueva fila en formato HTML.
                // Agrega el valor de cada celda de la fila actual a la cadena HTML de la fila.
                filas += "<tr>";
                filas += "<td>" + row.Cells["Producto"].Value.ToString() + "</td>";
                filas += "<td>" + row.Cells["Precio_Venta"].Value.ToString() + "</td>";
                filas += "<td>" + row.Cells["Cantidad"].Value.ToString() + "</td>";
                filas += "<td>" + row.Cells["Monto_Total"].Value.ToString() + "</td>";
                // Cierra la etiqueta de la fila en formato HTML.
                filas += "</tr>";
            }

            // Reemplaza la etiqueta de filas en el contenido HTML con las filas construidas
            Texto_Html = Texto_Html.Replace("@filas", filas);
            Texto_Html = Texto_Html.Replace("@montototal", txtmontototal.Text);
            Texto_Html = Texto_Html.Replace("@pagocon", txtmontopago.Text);
            Texto_Html = Texto_Html.Replace("@cambio", txtmontocambio.Text);

            // Crea un cuadro de diálogo para guardar el archivo PDF
            SaveFileDialog saveFile = new SaveFileDialog();
            saveFile.FileName = string.Format("Venta_{0}.pdf", txtnumerodocumento.Text);
            saveFile.Filter = "Pdf Files | *.pdf"; // Solo visualizar archivos pdf en la ventana de dialogo

            if (saveFile.ShowDialog() == DialogResult.OK)
            {
                /* Crea un nuevo objeto FileStream utilizando el nombre de archivo proporcionado en saveFile.FileName
                 y FileMode.Create para crear un nuevo archivo o reemplazar un archivo existente. */
                using (FileStream stream = new FileStream(saveFile.FileName, FileMode.Create))
                {
                    // Crea un documento PDF y lo abre
                    Document pdfDoc = new Document(PageSize.A4, 25, 25, 25, 25);

                    // Obtiene una instancia de PdfWriter para escribir en el documento PDF utilizando el objeto pdfDoc y el flujo de datos stream.
                    PdfWriter writer = PdfWriter.GetInstance(pdfDoc, stream);
                    pdfDoc.Open();

                    // Obtiene el logo del negocio y lo agrega al documento PDF
                    bool obtenido = true;
                    byte[] byteImage = new CN_Negocio().ObtenerLogo(out obtenido);


                    if (obtenido)
                    {
                        // Se crea una instancia de la clase Image de iTextSharp utilizando el arreglo de bytes byteImage que contiene la imagen.
                        iTextSharp.text.Image img = iTextSharp.text.Image.GetInstance(byteImage);
                        img.ScaleToFit(60, 60);
                        img.Alignment = iTextSharp.text.Image.UNDERLYING; // Se establece la alineación de la imagen como UNDERLYING para que se muestre debajo del contenido del documento.
                        // pdfDoc.Left representa el margen izquierdo del documento y pdfDoc.GetTop(51) representa la posición vertical en la página.
                        img.SetAbsolutePosition(pdfDoc.Left, pdfDoc.GetTop(51));
                        pdfDoc.Add(img);
                    }


                    /* Se crea una instancia de la clase StringReader que permite leer el contenido de la cadena Texto_Html.
                     Esto es necesario para que el método ParseXHtml pueda analizar y convertir el HTML en contenido PDF. */
                    using (StringReader sr = new StringReader(Texto_Html))
                    {
                        /* Se obtiene la instancia de XMLWorkerHelper y se llama al método ParseXHtml para convertir el HTML en contenido PDF.
                         Se pasan como parámetros el escritor (writer), el documento PDF (pdfDoc) y el lector de cadena (sr) que contiene el HTML. */
                        XMLWorkerHelper.GetInstance().ParseXHtml(writer, pdfDoc, sr);
                    }

                    // Cierra el documento y el stream, y muestra el mensaje de éxito
                    pdfDoc.Close();
                    stream.Close();
                    MessageBox.Show("Documento generado", "Mensaje", MessageBoxButtons.OK, MessageBoxIcon.Information);

                }
            }
        }
    }
}
