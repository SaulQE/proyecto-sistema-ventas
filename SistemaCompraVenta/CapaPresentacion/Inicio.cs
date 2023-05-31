using CapaEntidad;
using CapaNegocio;
using CapaPresentacion.Modales;
using FontAwesome.Sharp;
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
    public partial class Inicio : Form
    {
        private static Usuario usuarioActual;
        private static Button MenuActivo = null;
        private static Form FormularioActivo = null;

        public Inicio(Usuario objusuario = null)
        {
            if (objusuario == null)
                usuarioActual = new Usuario() { Nom_Completo = "ADMIN PREDEFINIDO", id_Usuario = 1 };
            else
                usuarioActual = objusuario;

            InitializeComponent();
            personalizardiseño();
        }

        private void DiseñoFormulario_Load(object sender, EventArgs e)
        {
            List<Permiso> ListaPermisos = new CN_Permiso().Listar(usuarioActual.id_Usuario); //Aqui se comparte el id_Usuario para el metodo Listar y poder enlistar sus permisos

            foreach (Button button in menu.Controls.OfType<Button>()) //
            {
                bool encontrado = ListaPermisos.Any(m => m.Nom_Menu == button.Name); // m viene hacer cada elemento que tiene mi lista y esta validando haciendo un recorrido automatico

                if (encontrado == false)
                {
                    button.Visible = false;
                }
            }
            lblusuario.Text = usuarioActual.Nom_Completo;
        }


        private void AbrirFormulario(Button menu, Form formulario)
        {
            //if (MenuActivo != null)
          //  {
           //   MenuActivo.BackColor = Color.FromArgb(33, 38, 43); 
          //  }
            // menu.BackColor = Color.FromArgb(22, 25, 28);
           //  MenuActivo = menu;


            if (FormularioActivo != null)
                FormularioActivo.Close();

            FormularioActivo = formulario;
            formulario.TopLevel = false; //para que no sea superior
            formulario.FormBorderStyle = FormBorderStyle.None; //para que no tenga bordes
            formulario.Dock = DockStyle.Fill; //Para rellenar todo el espacio del contenedor

            contenedor.Controls.Add(formulario); //aca se agrega el formulario al contenedor
            formulario.BringToFront(); //para mandar al frente al form
            formulario.Show(); // para que se muestre el formulario


        }

        private void menuusuarios_Click(object sender, EventArgs e)
        {
            AbrirFormulario((Button)sender, new frmUsuarios());
        }

        private void menumantenimiento_Click(object sender, EventArgs e)
        {
            mostrarsubmenu(submenumantenimiento); //
        }

        private void submenucategoria_Click(object sender, EventArgs e)
        {
            AbrirFormulario((Button)sender, new frmCategoria());
        }

        private void submenuproductos_Click(object sender, EventArgs e)
        {
            AbrirFormulario((Button)sender, new frmProducto());
        }

        private void menuventas_Click(object sender, EventArgs e)
        {
            mostrarsubmenu(submenuventas); //
        }

        private void submenuregistrarventa_Click(object sender, EventArgs e)
        {
            AbrirFormulario((Button)sender, new frmVentas(usuarioActual));
        }

        private void submenuverdetalleventa_Click(object sender, EventArgs e)
        {
            AbrirFormulario((Button)sender, new frmDetalleVenta());
        }

        private void menucompras_Click(object sender, EventArgs e)
        {
            mostrarsubmenu(submenucompras); //
        }

        private void submenuregistrarcompra_Click(object sender, EventArgs e)
        {
            AbrirFormulario((Button)sender, new frmCompras(usuarioActual));
        }

        private void submenuverdetallecompra_Click(object sender, EventArgs e)
        {
            AbrirFormulario((Button)sender, new frmDetalleCompra());
        }

        private void menuclientes_Click(object sender, EventArgs e)
        {
            AbrirFormulario((Button)sender, new frmClientes());
        }

        private void menuproveedores_Click(object sender, EventArgs e)
        {
            AbrirFormulario((Button)sender, new frmProveedores());
        }



        private void personalizardiseño()
        {
            submenumantenimiento.Visible = false;
            submenuventas.Visible = false;
            submenucompras.Visible = false;
            submenureportes.Visible = false;
        }
        private void ocultarsubmenu()
        {
            if (submenumantenimiento.Visible == true)
                submenumantenimiento.Visible = false;
            if (submenuventas.Visible == true)
                submenuventas.Visible = false;
            if (submenucompras.Visible == true)
                submenucompras.Visible = false;
            if (submenureportes.Visible == true)
                submenureportes.Visible = false;
        }

        private void mostrarsubmenu(Panel submenu)
        {
            if (submenu.Visible == false)
            {
                ocultarsubmenu();
                submenu.Visible = true;
            }
            else
            {
                submenu.Visible = false;
            }
        }

        private void submenunegocio_Click(object sender, EventArgs e)
        {
            AbrirFormulario((Button)sender, new frmNegocio());
        }

        private void submenureportecompras_Click(object sender, EventArgs e)
        {
            AbrirFormulario(menureportes, new frmReporteCompras());
        }

        private void submenureporteventas_Click(object sender, EventArgs e)
        {
            AbrirFormulario(menureportes, new frmReporteVentas());
        }

        private void menureportes_Click(object sender, EventArgs e)
        {
            mostrarsubmenu(submenureportes); 
        }

        private void menuacercade_Click(object sender, EventArgs e)
        {
            mdAcercade md = new mdAcercade();
            md.ShowDialog();
        }

        private void btnsalir_Click(object sender, EventArgs e)
        {
            if(MessageBox.Show("¿Desea salir?","Mensaje",MessageBoxButtons.YesNo,MessageBoxIcon.Question) == DialogResult.Yes)
            {
                this.Close();
            }
        }
    }
}
