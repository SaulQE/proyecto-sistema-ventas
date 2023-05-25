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
    public partial class frmProducto : Form
    {
        public frmProducto()
        {
            InitializeComponent();
        }

        private void frmProducto_Load(object sender, EventArgs e)
        {

            cboestado.Items.Add(new OpcionCombo() { Valor = 1, Texto = "Activo" }); // Esto es para mi cbo Estado
            cboestado.Items.Add(new OpcionCombo() { Valor = 0, Texto = "No Activo" });
            cboestado.DisplayMember = "Texto"; //aca mostrara el dato que tiene de nombre Texto
            cboestado.ValueMember = "Valor"; //no mostraria y manejaria como valor interno, y sera aquel dato con nombre Valor
            cboestado.SelectedIndex = 0; //Para siempre selecionar el primero


            List<Categoria> listaCategoria = new CN_Categoria().Listar(); // Aqui obtengo la lista

            foreach (Categoria item in listaCategoria) // voy a recorrer listaCategoria, y el item esta comteniendo cada clase rol que esta dentro de la listaCategoria
            {
                cbocategoria.Items.Add(new OpcionCombo() { Valor = item.id_Categoria, Texto = item.Descripcion });
            }
            cbocategoria.DisplayMember = "Texto";
            cbocategoria.ValueMember = "Valor";
            cbocategoria.SelectedIndex = 0;


            foreach (DataGridViewColumn columna in dgvdata.Columns)
            {
                if (columna.Visible == true && columna.Name != "btnseleccionar")
                {
                    cbobusqueda.Items.Add(new OpcionCombo() { Valor = columna.Name, Texto = columna.HeaderText });
                }
            }
            cbobusqueda.DisplayMember = "Texto";
            cbobusqueda.ValueMember = "Valor";
            cbobusqueda.SelectedIndex = 0;



            List<Producto> listaProducto = new CN_Producto().Listar(); // Aqui obtengo la lista de todo los Productos

            foreach (Producto item in listaProducto) // voy a recorrer listaProducto, y el item esta comteniendo cada clase Producto que esta dentro de la listaProducto
            {

                dgvdata.Rows.Add(new object[] {
                "",
                item.id_Producto,
                item.Codigo,
                item.Nombre,
                item.Descripcion,
                item.oCategoria.id_Categoria,
                item.oCategoria.Descripcion,
                item.Stock,
                item.Precio_Compra,
                item.Precio_Venta,
                item.Estado == true ? 1 : 0, // Estamos diciendo que si es true me muestre 1, en lo contrario muestre 0
                item.Estado == true ? "Activo" : "No Activo" // 1era columna viene ser los valores y 2da el texto del estado
                });

            }
        }

        private void btnguardar_Click(object sender, EventArgs e)
        {

            string mensaje = string.Empty;

            Producto objproducto = new Producto() // Creo objeto de la clase Producto
            {
                id_Producto = Convert.ToInt32(txtid.Text),  // Llenamos los atributos de la clase con los campos de texto
                Codigo = txtcodigo.Text,
                Nombre = txtnombre.Text,
                Descripcion = txtdescripcion.Text,
                oCategoria = new Categoria() { id_Categoria = Convert.ToInt32(((OpcionCombo)cbocategoria.SelectedItem).Valor) }, // ComboBox
                Estado = Convert.ToInt32(((OpcionCombo)cboestado.SelectedItem).Valor) == 1 ? true : false
            };


            if (objproducto.id_Producto == 0)
            {
                int idgenerado = new CN_Producto().Registrar(objproducto, out mensaje); // Estamos pasando los parametros que requiere el metodo Registrar y como respuesta obtenemos el idusuariogenerado

                if (idgenerado != 0)
                {
                    dgvdata.Rows.Add(new object[] {"",
                    idgenerado,
                    txtcodigo.Text,
                    txtnombre.Text,
                    txtdescripcion.Text,
                    ((OpcionCombo)cbocategoria.SelectedItem).Valor.ToString(),
                    ((OpcionCombo)cbocategoria.SelectedItem).Texto.ToString(),
                    "0",
                    "0.00",
                    "0.00",
                    ((OpcionCombo)cboestado.SelectedItem).Valor.ToString(),
                    ((OpcionCombo)cboestado.SelectedItem).Texto.ToString()
                    });

                    Limpiar();
                }
                else
                {
                    MessageBox.Show(mensaje);

                }

            }
            else
            {
                bool resultado = new CN_Producto().Editar(objproducto, out mensaje);

                if (resultado)
                {
                    DataGridViewRow row = dgvdata.Rows[Convert.ToInt32(txtindice.Text)];
                    row.Cells["id"].Value = txtid.Text; //
                    row.Cells["Codigo"].Value = txtcodigo.Text;
                    row.Cells["Nombre"].Value = txtnombre.Text;
                    row.Cells["Descripcion"].Value = txtdescripcion.Text;
                    row.Cells["id_Categoria"].Value = ((OpcionCombo)cbocategoria.SelectedItem).Valor.ToString(); //Obtener el valor 
                    row.Cells["Categoria"].Value = ((OpcionCombo)cbocategoria.SelectedItem).Texto.ToString(); //Obtener el texto
                    row.Cells["EstadoValor"].Value = ((OpcionCombo)cboestado.SelectedItem).Valor.ToString();
                    row.Cells["Estado"].Value = ((OpcionCombo)cboestado.SelectedItem).Texto.ToString();

                    Limpiar();
                }
                else
                {
                    MessageBox.Show(mensaje);
                }
            }
        }

        private void Limpiar()
        {
            txtindice.Text = "-1";
            txtid.Text = "0";
            txtcodigo.Text = "";
            txtnombre.Text = "";
            txtdescripcion.Text = "";
            cbocategoria.SelectedIndex = 0;
            cboestado.SelectedIndex = 0;

            txtcodigo.Select();
        }

        private void dgvdata_CellPainting(object sender, DataGridViewCellPaintingEventArgs e)
        {
            if (e.RowIndex < 0)
                return;
            if (e.ColumnIndex == 0)
            {
                e.Paint(e.CellBounds, DataGridViewPaintParts.All);

                var w = Properties.Resources.check20.Width;
                var h = Properties.Resources.check20.Height;
                var x = e.CellBounds.Left + (e.CellBounds.Width - w) / 2;
                var y = e.CellBounds.Top + (e.CellBounds.Height - h) / 2;

                e.Graphics.DrawImage(Properties.Resources.check20, new Rectangle(x, y, w, h));
                e.Handled = true;

            }
        }

        private void dgvdata_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (dgvdata.Columns[e.ColumnIndex].Name == "btnseleccionar")
            {
                int indice = e.RowIndex;

                if (indice >= 0)
                {

                    txtindice.Text = indice.ToString();
                    txtid.Text = dgvdata.Rows[indice].Cells["id"].Value.ToString(); //
                    txtcodigo.Text = dgvdata.Rows[indice].Cells["Codigo"].Value.ToString();
                    txtnombre.Text = dgvdata.Rows[indice].Cells["Nombre"].Value.ToString();
                    txtdescripcion.Text = dgvdata.Rows[indice].Cells["Descripcion"].Value.ToString();

                    foreach (OpcionCombo oc in cbocategoria.Items) // lee todas las oc (las clases) que tengo dentro de cbocategoria
                    {
                        if (Convert.ToInt32(oc.Valor) == Convert.ToInt32(dgvdata.Rows[indice].Cells["id_Categoria"].Value))
                        {
                            int indice_combo = cbocategoria.Items.IndexOf(oc); // Obtenemos el indice dentro del ComboBox
                            cbocategoria.SelectedIndex = indice_combo; // Y ese combo mandamos a mostrar al cbocategoria
                            break;
                        }
                    }

                    foreach (OpcionCombo oc in cboestado.Items) // lee todas las oc (las clases) que tengo dentro de cbrestado
                    {
                        if (Convert.ToInt32(oc.Valor) == Convert.ToInt32(dgvdata.Rows[indice].Cells["EstadoValor"].Value))
                        {
                            int indice_combo = cboestado.Items.IndexOf(oc); // Obtenemos el indice dentro del ComboBox
                            cboestado.SelectedIndex = indice_combo; // Y ese combo mandamos a mostrar al cboestado
                            break;
                        }
                    }


                }

            }
        }

        private void btneliminar_Click(object sender, EventArgs e)
        {
            if (Convert.ToInt32(txtid.Text) != 0)
            {
                if (MessageBox.Show("¿Desea eliminar el producto?", "Mensaje", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    string mensaje = string.Empty;
                    Producto objproducto = new Producto()
                    {
                        id_Producto = Convert.ToInt32(txtid.Text),
                    };

                    bool Respuesta = new CN_Producto().Eliminar(objproducto, out mensaje);

                    if (Respuesta)
                    {
                        dgvdata.Rows.RemoveAt(Convert.ToInt32(txtindice.Text)); // Si la respeusta es true, se procede a eliminar la fila del DGV
                        Limpiar();
                    }
                    else
                    {
                        MessageBox.Show(mensaje, "Mensaje", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    }
                }
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

        private void btnlimpiar_Click(object sender, EventArgs e)
        {
            Limpiar();
        }

        private void btnexportar_Click(object sender, EventArgs e)
        {
            if (dgvdata.Rows.Count < 1) // cuenta las filas de mi dgvdata
            {
                MessageBox.Show("No hay datos para exportar", "Mensaje", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
            else
            {
                DataTable dt = new DataTable();

                foreach(DataGridViewColumn columna in dgvdata.Columns) // accedo a todas las columnas que tiene mi dgvdata
                {
                    if(columna.HeaderText != "" && columna.Visible)
                        dt.Columns.Add(columna.HeaderText, typeof(string));
                    
                }

                foreach (DataGridViewRow row in dgvdata.Rows) // accedo a todas las filas de mi datagridview
                {
                    if (row.Visible)
                        dt.Rows.Add(new object[]{
                            row.Cells[2].Value.ToString(),  // para empezar desde el indice 2
                            row.Cells[3].Value.ToString(),  
                            row.Cells[4].Value.ToString(),
                            row.Cells[6].Value.ToString(),  
                            row.Cells[7].Value.ToString(),  
                            row.Cells[8].Value.ToString(),  
                            row.Cells[9].Value.ToString(),  
                            row.Cells[11].Value.ToString(),  

                        });
                }
                SaveFileDialog saveFile = new SaveFileDialog();
                saveFile.FileName = string.Format("ReporteProducto_{0}.xlsx",DateTime.Now.ToString("ddMMyyyyHHmmss"));
                saveFile.Filter = "Excel File | *.xlxs"; // Solo visualizar archivos xlxs en la ventana de dialogo

                if (saveFile.ShowDialog() == DialogResult.OK) {
                    try {
                        XLWorkbook wb = new XLWorkbook();
                        var hoja = wb.Worksheets.Add(dt, "Informe");
                        hoja.ColumnsUsed().AdjustToContents(); // Se ajuste el ancho de las columnas segun el valor que tengan
                        wb.SaveAs(saveFile.FileName);
                        MessageBox.Show("Reporte Generado", "Mensaje", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    }catch {
                        MessageBox.Show("Error al generar reporte", "Mensaje", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    }
                }

            }
        }
    }
}