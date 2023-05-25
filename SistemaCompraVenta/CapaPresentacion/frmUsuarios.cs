﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using CapaPresentacion.Utilidades;

using CapaEntidad;
using CapaNegocio;

namespace CapaPresentacion
{
    public partial class frmUsuarios : Form
    {
        public frmUsuarios()
        {
            InitializeComponent();
        }

        private void frmUsuarios_Load(object sender, EventArgs e)
        {
            cboestado.Items.Add(new OpcionCombo() { Valor = 1, Texto = "Activo" }); // Esto es para mi cbo Estado
            cboestado.Items.Add(new OpcionCombo() { Valor = 0, Texto = "No Activo" });
            cboestado.DisplayMember = "Texto"; //aca mostrara el dato que tiene de nombre Texto
            cboestado.ValueMember = "Valor"; //no mostraria y manejaria como valor interno, y sera aquel dato con nombre Valor
            cboestado.SelectedIndex = 0; //Para siempre selecionar el primero


            List<Rol> listaRol = new CN_Rol().Listar(); // Aqui obtengo la lista

            foreach (Rol item in listaRol) // voy a recorrer listaRol, y el item esta comteniendo cada clase rol que esta dentro de la listaRol
            {
                cborol.Items.Add(new OpcionCombo() { Valor = item.id_Rol, Texto = item.Descripcion });
            }
            cborol.DisplayMember = "Texto";
            cborol.ValueMember = "Valor"; 
            cborol.SelectedIndex = 0;


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



            List<Usuario> listaUsuario = new CN_Usuario().Listar(); // Aqui obtengo la lista de todo los usuarios

            foreach (Usuario item in listaUsuario) // voy a recorrer listaUsuario, y el item esta comteniendo cada clase usuario que esta dentro de la listaUsuario
            {

                dgvdata.Rows.Add(new object[] {"",item.id_Usuario,item.Documento,item.Nom_Completo,item.Correo,item.Clave, // Envio todos mi valores a mi dgvData, como el txtid, 
                item.oRol.id_Rol, 
                item.oRol.Descripcion,
                item.Estado == true ? 1 : 0, // Estamos diciendo que si es true me muestre 1, en lo contrario muestre 0
                item.Estado == true ? "Activo" : "No Activo" // 1era columna viene ser los valores y 2da el texto del estado
                });
            }
        }

        private void btnguardar_Click(object sender, EventArgs e)
        {
            string mensaje = string.Empty;

            Usuario objusuario = new Usuario() // Creo objeto de la clase usuario
            {
                id_Usuario = Convert.ToInt32(txtid.Text),  // Llenamos los atributos de la clase con los campos de texto
                Documento = txtdocumento.Text,
                Nom_Completo = txtnombrecompleto.Text,
                Correo = txtcorreo.Text,
                Clave = txtclave.Text,
                oRol = new Rol() { id_Rol = Convert.ToInt32(((OpcionCombo) cborol.SelectedItem).Valor)}, // ComboBox
                Estado = Convert.ToInt32(((OpcionCombo)cboestado.SelectedItem).Valor) == 1 ? true : false
            };


            if(objusuario.id_Usuario == 0)
            {
                int idusuariogenerado = new CN_Usuario().Registrar(objusuario, out mensaje); // Estamos pasando los parametros que requiere el metodo Registrar y como respuesta obtenemos el idusuariogenerado

                if (idusuariogenerado != 0)
                {
                    dgvdata.Rows.Add(new object[] {"",idusuariogenerado,txtdocumento.Text,txtnombrecompleto.Text,txtcorreo.Text,txtclave.Text,
                        ((OpcionCombo)cborol.SelectedItem).Valor.ToString(), //obtenemos el item selecionado, lo comvertimos en una opcioncombo y accedemos a su atributo texto, y se combierte en cadena texto
                        ((OpcionCombo)cborol.SelectedItem).Texto.ToString(),
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
                bool resultado = new CN_Usuario().Editar(objusuario, out mensaje);

                if (resultado)
                {
                    DataGridViewRow row = dgvdata.Rows[Convert.ToInt32(txtindice.Text)];
                    row.Cells["id"].Value = txtid.Text; //
                    row.Cells["Documento"].Value = txtdocumento.Text;
                    row.Cells["Nom_Completo"].Value = txtnombrecompleto.Text;
                    row.Cells["Correo"].Value = txtcorreo.Text;
                    row.Cells["Clave"].Value = txtclave.Text;
                    row.Cells["id_Rol"].Value = ((OpcionCombo)cborol.SelectedItem).Valor.ToString(); //Obtener el valor 
                    row.Cells["Rol"].Value = ((OpcionCombo)cborol.SelectedItem).Texto.ToString(); //Obtener el texto
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
            txtdocumento.Text = "";
            txtnombrecompleto.Text = "";
            txtcorreo.Text = "";
            txtclave.Text = "";
            txtconfirmarclave.Text = "";
            cborol.SelectedIndex = 0;
            cboestado.SelectedIndex = 0;

            txtdocumento.Select();
        }

        private void dgvdata_CellPainting(object sender, DataGridViewCellPaintingEventArgs e)
        {
            if (e.RowIndex < 0 )          
                return;
            if(e.ColumnIndex == 0)
            {
                e.Paint(e.CellBounds, DataGridViewPaintParts.All);

                var w = Properties.Resources.check20.Width;
                var h = Properties.Resources.check20.Height;
                var x = e.CellBounds.Left + (e.CellBounds.Width - w) / 2;
                var y = e.CellBounds.Top + (e.CellBounds.Height - h) / 2;

                e.Graphics.DrawImage(Properties.Resources.check20, new Rectangle(x,y,w,h));
                e.Handled = true;

            }

        }

        private void dgvdata_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (dgvdata.Columns[e.ColumnIndex].Name == "btnseleccionar")
            {
                int indice = e.RowIndex;

                if(indice >= 0)
                {

                    txtindice.Text = indice.ToString();
                    txtid.Text = dgvdata.Rows[indice].Cells["id"].Value.ToString(); //
                    txtdocumento.Text = dgvdata.Rows[indice].Cells["Documento"].Value.ToString();
                    txtnombrecompleto.Text = dgvdata.Rows[indice].Cells["Nom_Completo"].Value.ToString();
                    txtcorreo.Text = dgvdata.Rows[indice].Cells["Correo"].Value.ToString();
                    txtclave.Text = dgvdata.Rows[indice].Cells["Clave"].Value.ToString();
                    txtconfirmarclave.Text = dgvdata.Rows[indice].Cells["Clave"].Value.ToString();

                    foreach (OpcionCombo oc in cborol.Items) // lee todas las oc (las clases) que tengo dentro de cbroRol
                    {
                        if (Convert.ToInt32(oc.Valor) == Convert.ToInt32(dgvdata.Rows[indice].Cells["id_Rol"].Value))
                        {
                            int indice_combo = cborol.Items.IndexOf(oc); // Obtenemos el indice dentro del ComboBox
                            cborol.SelectedIndex = indice_combo; // Y ese combo mandamos a mostrar al cboRol
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
            if(Convert.ToInt32(txtid.Text) != 0)
            {
                if(MessageBox.Show("¿Desea eliminar el usuario?","Mensaje",MessageBoxButtons.YesNo,MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    string mensaje = string.Empty;

                    Usuario objusuario = new Usuario() 
                    {
                        id_Usuario = Convert.ToInt32(txtid.Text),  
                    };

                    bool respuesta = new CN_Usuario().Eliminar(objusuario, out mensaje); 

                    if(respuesta)
                    {
                        dgvdata.Rows.RemoveAt(Convert.ToInt32(txtindice.Text)); // Si la respeusta es true, se procede a eliminar la fila del DGV
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

            if(dgvdata.Rows.Count > 0)
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
            foreach(DataGridViewRow row in dgvdata.Rows)
            {
                row.Visible = true;
            }
        }

        private void btnlimpiar_Click(object sender, EventArgs e)
        {
            Limpiar();
        }
    }
}
