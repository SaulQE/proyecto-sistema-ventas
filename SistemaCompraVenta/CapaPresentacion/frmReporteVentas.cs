using CapaEntidad;
using CapaNegocio;
using CapaPresentacion.Utilidades;
using ClosedXML.Excel;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CapaPresentacion
{
    public partial class frmReporteVentas : Form
    {
        public frmReporteVentas()
        {
            InitializeComponent();
        }

        private void frmReporteVentas_Load(object sender, EventArgs e)
        {
            // Agregar cada columna del DataGridView a la lista desplegable de búsqueda
            foreach (DataGridViewColumn columna in dgvdata.Columns)
            {
                cbobusqueda.Items.Add(new OpcionCombo() { Valor = columna.Name, Texto = columna.HeaderText });
            }
            // Establecer las propiedades de visualización y valor de la lista desplegable de búsqueda
            cbobusqueda.DisplayMember = "Texto";
            cbobusqueda.ValueMember = "Valor";
            cbobusqueda.SelectedIndex = 0;
        }

        private void btnbuscarreporte_Click(object sender, EventArgs e)
        {
            List<ReporteVenta> lista = new List<ReporteVenta>();

            lista = new CN_Reporte().Venta(txtfechainicio.Value.ToString(),txtfechafin.Value.ToString());

            dgvdata.Rows.Clear();

            foreach (ReporteVenta rv in lista)
            {
                // Agregar filas al dgvdata, para eso se utiliza new object para crear un nuevo arreglo de objetos que contenga los valores
                dgvdata.Rows.Add(new object[]
                {
                    rv.F_Registro,
                    rv.Tipo_Documento,
                    rv.Nro_Documento,
                    rv.Monto_Total,
                    rv.UsuarioRegistro,
                    rv.Documento_Cliente,
                    rv.Nom_Cliente,
                    rv.CodigoProducto,
                    rv.NombreProducto,
                    rv.Categoria,
                    rv.Precio_Venta,
                    rv.Cantidad,
                    rv.SubTotal
                });
            }
        }

        private void btnbuscar_Click(object sender, EventArgs e)
        {
            string columnaFiltro = ((OpcionCombo)cbobusqueda.SelectedItem).Valor.ToString();

            if (dgvdata.Rows.Count > 0)
            {
                foreach (DataGridViewRow row in dgvdata.Rows) //Recorre cada fila del DataGridView
                {
                    /* Selecciono la celda de la columna, obtengo el valor y lo convierto a texto, lugo elimino los espacios en blanco
                       y convierto todo el texto a mayus, ya teniendo esto voy a comparar el valor de la celda con el texto dado en busqueda. */
                    if (row.Cells[columnaFiltro].Value.ToString().Trim().ToUpper().Contains(txtbusqueda.Text.Trim().ToUpper()))
                        row.Visible = true;
                    else
                        row.Visible = false;
                }
            }
        }

        private void btnlimpiarbuscador_Click(object sender, EventArgs e)
        {
            txtbusqueda.Text = "";
            foreach (DataGridViewRow row in dgvdata.Rows)
            {
                row.Visible = true;
            }
        }

        private void btnexportar_Click(object sender, EventArgs e)
        {
            if (dgvdata.Rows.Count < 1)
            {
                MessageBox.Show("No hay registro para exportar", "Mensaje", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);

            }
            else
            {
                DataTable dt = new DataTable();

                // accedo a todas las columnas que tiene mi dgvdata
                foreach (DataGridViewColumn columna in dgvdata.Columns)
                {

                    // Agregar una nueva columna al DataTable con el encabezado de la columna actual del DataGridView
                    dt.Columns.Add(columna.HeaderText, typeof(string));
                }

                foreach (DataGridViewRow row in dgvdata.Rows) // accedo a todas las filas de mi datagridview
                {
                    if (row.Visible)
                        // Agregar los valores de cada celda de la fila actual al DataTable
                        dt.Rows.Add(new object[]{
                            row.Cells[0].Value.ToString(),
                            row.Cells[1].Value.ToString(),
                            row.Cells[2].Value.ToString(),
                            row.Cells[3].Value.ToString(),
                            row.Cells[4].Value.ToString(),
                            row.Cells[5].Value.ToString(),
                            row.Cells[6].Value.ToString(),
                            row.Cells[7].Value.ToString(),
                            row.Cells[8].Value.ToString(),
                            row.Cells[9].Value.ToString(),
                            row.Cells[10].Value.ToString(),
                            row.Cells[11].Value.ToString(),
                            row.Cells[12].Value.ToString()
                        });
                }
                SaveFileDialog saveFile = new SaveFileDialog();
                saveFile.FileName = string.Format("ReporteVentas_{0}.xlsx", DateTime.Now.ToString("ddMMyyyyHHmmss"));
                saveFile.Filter = "Excel File | *.xlxs"; // Solo visualizar archivos xlxs en la ventana de dialogo

                if (saveFile.ShowDialog() == DialogResult.OK)
                {
                    try
                    {
                        // Crear un nuevo documento de Excel y agregar los datos del DataTable en una hoja
                        XLWorkbook wb = new XLWorkbook();
                        var hoja = wb.Worksheets.Add(dt, "Informe");

                        // Ajustar el ancho de las columnas según el contenido
                        hoja.ColumnsUsed().AdjustToContents();

                        // Guardar el archivo en la ubicación especificada
                        wb.SaveAs(saveFile.FileName);
                        MessageBox.Show("Reporte Generado", "Mensaje", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    }
                    catch
                    {
                        MessageBox.Show("Error al generar reporte", "Mensaje", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    }
                }
            }
        }
    }
}
