using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using CapaEntidad;
using CapaNegocio;
using FontAwesome.Sharp;

namespace CapaPresentacion
{
    public partial class InicioFalso : Form
    {

        private static Usuario usuarioActual;
        private static IconMenuItem MenuActivo = null;
        private static Form FormularioActivo = null;

        public InicioFalso(Usuario objusuario = null)
        {
            if (objusuario == null) 
                usuarioActual = new Usuario() { Nom_Completo = "ADMIN PREDEFINIDO", id_Usuario = 1 };
            else
                usuarioActual = objusuario;

            InitializeComponent();
        }

        private void Inicio_Load(object sender, EventArgs e)
        {
            List<Permiso> ListaPermisos = new CN_Permiso().Listar(usuarioActual.id_Usuario); //Aqui se comparte el id_Usuario para el metodo Listar y poder enlistar sus permisos


            foreach (IconMenuItem iconMenu in menu.Items) //Todo tipo iconMenuItem dentro de menu.Items se va almacenar a iconMenu
            {
                bool encontrado = ListaPermisos.Any(m => m.Nom_Menu == iconMenu.Name); // m viene hacer cada elemento que tiene mi lista y esta validando haciendo un recorrido automatico

                if (encontrado == false)
                {
                    iconMenu.Visible = false;
                }

            }


            lblusuario.Text = usuarioActual.Nom_Completo;

        }


        private void AbrirFormulario(IconMenuItem menu, Form formulario)
        {
            if(MenuActivo != null)
            {
                MenuActivo.BackColor = Color.White;
            }
            menu.BackColor = Color.Silver;
            MenuActivo = menu;


            if (FormularioActivo != null)
            {
                FormularioActivo.Close();
            }

            FormularioActivo = formulario;
            formulario.TopLevel = false; //para que no sea superior
            formulario.FormBorderStyle = FormBorderStyle.None; //para que no tenga bordes
            formulario.Dock = DockStyle.Fill; //Para rellenar todo el espacio del contenedor
            formulario.BackColor = Color.SteelBlue; //color del sistema

            contenedor.Controls.Add(formulario); //aca se agrega el formulario al contenedor
            formulario.Show(); // para que se muestre el formulario

        }

        private void menuusuarios_Click(object sender, EventArgs e)
        {
            AbrirFormulario((IconMenuItem)sender, new frmUsuarios());
        }

        private void submenucategoria_Click(object sender, EventArgs e)
        {
            AbrirFormulario(menumantenimiento, new frmCategoria());
        }

        private void submenuproducto_Click(object sender, EventArgs e)
        {
            AbrirFormulario(menumantenimiento, new frmProducto());
        }

        private void submenuregistrarventa_Click(object sender, EventArgs e)
        {
            AbrirFormulario(menuventas, new frmVentas());
        }

        private void submenuverdetalleventa_Click(object sender, EventArgs e)
        {
            AbrirFormulario(menuventas, new frmDetalleVenta());
        }

        private void submenuregistrarcompra_Click(object sender, EventArgs e)
        {
            AbrirFormulario(menucompras, new frmCompras());
        }

        private void submenuverdetallecompra_Click(object sender, EventArgs e)
        {
            AbrirFormulario(menucompras, new frmDetalleCompra());
        }

        private void menuclientes_Click(object sender, EventArgs e)
        {
            AbrirFormulario((IconMenuItem)sender, new frmClientes());
        }

        private void menuproveedores_Click(object sender, EventArgs e)
        {
            AbrirFormulario((IconMenuItem)sender, new frmProveedores());
        }

        private void menureportes_Click(object sender, EventArgs e)
        {
            AbrirFormulario((IconMenuItem)sender, new frmReportes());
        }

    }
}
