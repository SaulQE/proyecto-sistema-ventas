using CapaEntidad;
using CapaNegocio;
using CapaPresentacion.Utilidades;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CapaPresentacion.Modales
{
    public partial class mdProducto : Form
    {
        public Producto _Producto { get; set; } // Creo una propiedad unica para mi clase producto

        public mdProducto()
        {
            InitializeComponent();
        }

        private void mdProducto_Load(object sender, EventArgs e)
        {

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
                item.id_Producto,
                item.Codigo,
                item.Nombre,
                item.oCategoria.Descripcion,
                item.Stock,
                item.Precio_Compra,
                item.Precio_Venta
                });
            }
        }

        private void dgvdata_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            int iRow = e.RowIndex;      // Obtengo el iRow el cual es el RowIndex que ha sido seleccionado
            int iColum = e.ColumnIndex; // Obtengo el iColum el cual es el ColumnIndex que ha sido seleccionado (indice de la columna seleccionada)
            if (iRow >= 0 && iColum > 0)
            {
                _Producto = new Producto() // le estoy diciendo que sea un nuevo producto
                {
                    id_Producto = Convert.ToInt32(dgvdata.Rows[iRow].Cells["id"].Value.ToString()), // Se acceden a las columnas del dgv y dependiendo se
                    Codigo = dgvdata.Rows[iRow].Cells["Codigo"].Value.ToString(),                   //  va convertir para guardarlo en el atributo correspondiente de la clase
                    Nombre = dgvdata.Rows[iRow].Cells["Nombre"].Value.ToString(),
                    Stock = Convert.ToInt32(dgvdata.Rows[iRow].Cells["Stock"].Value.ToString()),
                    Precio_Compra = Convert.ToDecimal(dgvdata.Rows[iRow].Cells["Precio_Compra"].Value.ToString()),
                    Precio_Venta = Convert.ToDecimal(dgvdata.Rows[iRow].Cells["Precio_Venta"].Value.ToString()),

                };
                this.DialogResult = DialogResult.OK;
                this.Close(); // Aqui se cierra el modal de donde nos encontramos
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
    }
}
