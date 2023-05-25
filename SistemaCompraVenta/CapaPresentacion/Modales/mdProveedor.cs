using CapaEntidad;
using CapaNegocio;
using CapaPresentacion.Utilidades;
using DocumentFormat.OpenXml.Drawing;
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
    public partial class mdProveedor : Form
    {
        public Proveedor _Proveedor{ get; set; } // Guion bajo quiere decir que es una propiedad de esta form

        public mdProveedor()
        {
            InitializeComponent();
        }

        private void mdProveedor_Load(object sender, EventArgs e)
        {

            foreach (DataGridViewColumn columna in dgvdata.Columns)
            {
                if (columna.Visible == true)
                {
                    cbobusqueda.Items.Add(new OpcionCombo() { Valor = columna.Name, Texto = columna.HeaderText });
                }
            }
            cbobusqueda.DisplayMember = "Texto";
            cbobusqueda.ValueMember = "Valor";
            cbobusqueda.SelectedIndex = 0;

            List<Proveedor> listaProveedor = new CN_Proveedor().Listar(); // Aqui obtengo la lista de todo los Proveedor

            foreach (Proveedor item in listaProveedor) // voy a recorrer listaProveedor, y el item esta comteniendo cada clase Proveedor que esta dentro de la listaProveedor
            {

                dgvdata.Rows.Add(new object[] {item.id_Proveedor,item.Documento,item.Razon_Social}); // Envio todos mi valores a mi dgvData, como el txtid, documento y razon social
            }

        }

        private void dgvdata_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            int iRow = e.RowIndex; // obtengo el iRow el cual es el RowIndex que ha sido seleccionado
            int iColum = e.ColumnIndex;  // Obtengo el iColum el cual es el ColumnIndex que ha sido seleccionado (indice de la columna seleccionada)

            if (iRow >= 0 && iColum > 0) {

                _Proveedor = new Proveedor()
                {
                    id_Proveedor = Convert.ToInt32(dgvdata.Rows[iRow].Cells["id"].Value.ToString()), // Caracteristicas seleccionadas
                    Documento = dgvdata.Rows[iRow].Cells["Documento"].Value.ToString(),
                    Razon_Social = dgvdata.Rows[iRow].Cells["Razon_Social"].Value.ToString()
                };

                this.DialogResult = DialogResult.OK;
                this.Close(); // Cerrar formulario
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
