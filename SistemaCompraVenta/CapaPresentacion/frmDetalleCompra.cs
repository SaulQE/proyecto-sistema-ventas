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
    public partial class frmDetalleCompra : Form
    {
        public frmDetalleCompra()
        {
            InitializeComponent();
        }

        private void btnbuscar_Click(object sender, EventArgs e)
        {
            // Crea una nueva instancia de la clase CN_Compra y llama al método ObtenerCompra()
            // Obtiene una compra utilizando el número de compra proporcionado en el campo de texto txtbusqueda.Text.
            // El objeto devuelto por el método ObtenerCompra() se asigna a la variable oCompra
            Compra oCompra = new CN_Compra().ObtenerCompra(txtbusqueda.Text);


            if(oCompra.id_Compra != 0) // Verifica si se encontró una compra válida
            {
                // Asigna los valores obtenidos de la compra a los campos correspondientes del formulario
                txtnumerodocumento.Text = oCompra.Nro_Documento;
                txtfecha.Text = oCompra.F_Registro;
                txttipodocumento.Text = oCompra.Tipo_Documento;
                txtusuario.Text = oCompra.oUsuario.Nom_Completo;
                txtdocproveedor.Text = oCompra.oProveedor.Documento;
                txtnombreproveedor.Text = oCompra.oProveedor.Razon_Social;

                dgvdata.Rows.Clear(); // Limpia el contenido del DataGridView

                // Agrega cada detalle de compra al DataGridView
                foreach (Detalle_Compra dc in oCompra.oDetalle_Compra)
                {
                    dgvdata.Rows.Add(new object[] { dc.oProducto.Nombre, dc.Precio_Compra, dc.Cantidad, dc.Monto_Total });
                }
                // Muestra el monto total de la compra en el campo correspondiente
                txtmontototal.Text = oCompra.Monto_total.ToString("0.00");

            }
        }

        private void btnlimpiar_Click(object sender, EventArgs e)
        {
            // Limpia todos los campos del formulario y el contenido del DataGridView
            txtfecha.Text = "";
            txttipodocumento.Text = "";
            txtusuario.Text = "";
            txtdocproveedor.Text = "";
            txtnombreproveedor.Text = "";

            dgvdata.Rows.Clear();
            txtmontototal.Text = "0.00";
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
            string Texto_Html = Properties.Resources.PlantillaCompra.ToString();
            Negocio odatos = new CN_Negocio().ObtenerDatos();

            Texto_Html = Texto_Html.Replace("@nombrenegocio", odatos.Nombre.ToUpper());
            Texto_Html = Texto_Html.Replace("@docnegocio",odatos.RUC);
            Texto_Html = Texto_Html.Replace("@direcnegocio",odatos.Direccion);

            Texto_Html = Texto_Html.Replace("@tipodocumento", txttipodocumento.Text.ToUpper());
            Texto_Html = Texto_Html.Replace("@numerodocumento",txtnumerodocumento.Text);

            Texto_Html = Texto_Html.Replace("@docproveedor",txtdocproveedor.Text);
            Texto_Html = Texto_Html.Replace("@nombreproveedor",txtnombreproveedor.Text);
            Texto_Html = Texto_Html.Replace("@fecharegistro",txtfecha.Text);
            Texto_Html = Texto_Html.Replace("@usuarioregistro",txtusuario.Text);

            // Construye las filas de la tabla HTML con los detalles de compra obtenidos del DataGridView
            string filas = string.Empty;
            foreach (DataGridViewRow row in dgvdata.Rows) 
            {
                // Construye una nueva fila en formato HTML.
                // Agrega el valor de cada celda de la fila actual a la cadena HTML de la fila.
                filas += "<tr>";
                filas += "<td>" + row.Cells["Producto"].Value.ToString() + "</td>";
                filas += "<td>" + row.Cells["Precio_Compra"].Value.ToString() + "</td>";
                filas += "<td>" + row.Cells["Cantidad"].Value.ToString() + "</td>";
                filas += "<td>" + row.Cells["Monto_Total"].Value.ToString() + "</td>";
                // Cierra la etiqueta de la fila en formato HTML.
                filas += "</tr>";
            }

            // Reemplaza la etiqueta de filas en el contenido HTML con las filas construidas
            Texto_Html = Texto_Html.Replace("@filas", filas);
            Texto_Html = Texto_Html.Replace("@montototal", txtmontototal.Text);

            // Crea un cuadro de diálogo para guardar el archivo PDF
            SaveFileDialog saveFile = new SaveFileDialog();
            saveFile.FileName = string.Format("Compra_{0}.pdf", txtnumerodocumento.Text);
            saveFile.Filter = "Pdf Files | *.pdf"; // Solo visualizar archivos pdf en la ventana de dialogo

            if(saveFile.ShowDialog() == DialogResult.OK)
            {
                /* Crea un nuevo objeto FileStream utilizando el nombre de archivo proporcionado en saveFile.FileName
                 y FileMode.Create para crear un nuevo archivo o reemplazar un archivo existente. */
                using (FileStream stream = new FileStream(saveFile.FileName, FileMode.Create))
                {
                    // Crea un documento PDF y lo abre
                    Document pdfDoc = new Document(PageSize.A4,25,25,25,25);
                    // Obtiene una instancia de PdfWriter para escribir en el documento PDF utilizando el objeto pdfDoc y el flujo de datos stream.
                    PdfWriter writer = PdfWriter.GetInstance(pdfDoc, stream);
                    pdfDoc.Open();

                    // Obtiene el logo del negocio y lo agrega al documento PDF
                    bool obtenido = true;
                    byte[] byteImage = new CN_Negocio().ObtenerLogo(out obtenido);


                    if(obtenido)
                    {
                        // Se crea una instancia de la clase Image de iTextSharp utilizando el arreglo de bytes byteImage que contiene la imagen.
                        iTextSharp.text.Image img = iTextSharp.text.Image.GetInstance(byteImage);
                        img.ScaleToFit(60, 60);
                        img.Alignment = iTextSharp.text.Image.UNDERLYING; // Se establece la alineación de la imagen como UNDERLYING para que se muestre debajo del contenido del documento.
                        // pdfDoc.Left representa el margen izquierdo del documento y pdfDoc.GetTop(51) representa la posición vertical en la página.
                        img.SetAbsolutePosition(pdfDoc.Left,pdfDoc.GetTop(51));
                        pdfDoc.Add(img);
                    }


                    /* Se crea una instancia de la clase StringReader que permite leer el contenido de la cadena Texto_Html.
                     Esto es necesario para que el método ParseXHtml pueda analizar y convertir el HTML en contenido PDF. */
                    using (StringReader sr = new StringReader(Texto_Html) )
                    {
                        /* Se obtiene la instancia de XMLWorkerHelper y se llama al método ParseXHtml para convertir el HTML en contenido PDF.
                         Se pasan como parámetros el escritor (writer), el documento PDF (pdfDoc) y el lector de cadena (sr) que contiene el HTML. */
                        XMLWorkerHelper.GetInstance().ParseXHtml(writer, pdfDoc, sr);
                    }

                    // Cierra el documento y el stream, y muestra el mensaje de éxito
                    pdfDoc.Close();
                    stream.Close();
                    MessageBox.Show("Documento generado","Mensaje",MessageBoxButtons.OK, MessageBoxIcon.Information);

                }
            }
        }
    }
}
