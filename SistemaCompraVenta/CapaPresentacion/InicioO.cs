using CapaEntidad;
using CapaNegocio;
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
    public partial class InicioO : Form
    {
        bool sidebarExpand = true;
        bool mantenimientoCollapsed = true;
        bool ventasCollapsed = true;
        bool comprasCollapsed = true;
        bool reportesCollapsed = true;

        private static Usuario usuarioActual;
        private static Form FormularioActivo = null;
        private Button botonActivo;

        public InicioO(Usuario objusuario = null)
        {
            if (objusuario == null)
                usuarioActual = new Usuario() { Nom_Completo = "ADMIN PREDEFINIDO", id_Usuario = 1 };
            else
                usuarioActual = objusuario;

            InitializeComponent();
        }

        private void InicioO_Load(object sender, EventArgs e)
        {
            List<Permiso> ListaPermisos = new CN_Permiso().Listar(usuarioActual.id_Usuario); //Aqui se comparte el id_Usuario para el metodo Listar y poder enlistar sus permisos

            foreach (Button button in sidebar.Controls.OfType<Button>()) //
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

            if (FormularioActivo != null)
                FormularioActivo.Close();

            FormularioActivo = formulario;
            formulario.TopLevel = false; //para que no sea superior
            formulario.FormBorderStyle = FormBorderStyle.None; //para que no tenga bordes
            formulario.Dock = DockStyle.Fill; //Para rellenar todo el espacio del contenedor

            Contenedor.Controls.Add(formulario); //aca se agrega el formulario al contenedor
            formulario.BringToFront(); //para mandar al frente al form
            formulario.Show(); // para que se muestre el formulario
        }

        private void sidebarTimer_Tick(object sender, EventArgs e)
        {


            if (sidebarExpand)
            {
                sidebar.Width -= 10;
                if (sidebar.Width == sidebar.MinimumSize.Width)
                {
                    sidebarExpand = false;
                    sidebarTimer.Stop();
                }
            }
            else
            {
                sidebar.Width += 10;
                if(sidebar.Width == sidebar.MaximumSize.Width)
                {
                    sidebarExpand= true;
                    sidebarTimer.Stop();
                }
            }
        }

        private void btnMenu_Click(object sender, EventArgs e)
        {
            sidebarTimer.Start();
        }

        private void MantenimientoTimer_Tick(object sender, EventArgs e)
        {
            if (mantenimientoCollapsed)
            {
                MantenimientoContainer.Height += 10;
                if (MantenimientoContainer.Height == MantenimientoContainer.MaximumSize.Height)
                {
                    mantenimientoCollapsed = false;
                    MantenimientoTimer.Stop();
                }
            }
            else
            {
                MantenimientoContainer.Height -= 10;
                if(MantenimientoContainer.Height == MantenimientoContainer.MinimumSize.Height)
                {
                    mantenimientoCollapsed = true;
                    MantenimientoTimer.Stop();
                }
            }
        }

        private void btnMantenimiento_Click(object sender, EventArgs e)
        {
            MantenimientoTimer.Start();
        }

        private void VentasTimer_Tick(object sender, EventArgs e)
        {
            if (ventasCollapsed)
            {
                VentasContainer.Height += 10;
                if (VentasContainer.Height == VentasContainer.MaximumSize.Height)
                {
                    ventasCollapsed = false;
                    VentasTimer.Stop();
                }
            }
            else
            {
                VentasContainer.Height -= 10;
                if (VentasContainer.Height == VentasContainer.MinimumSize.Height)
                {
                    ventasCollapsed = true;
                    VentasTimer.Stop();
                }
            }
        }

        private void btnVentas_Click(object sender, EventArgs e)
        {
            VentasTimer.Start();
        }

        private void ComprasTimer_Tick(object sender, EventArgs e)
        {
            if (comprasCollapsed)
            {
                ComprasContainer.Height += 10;
                if (ComprasContainer.Height == ComprasContainer.MaximumSize.Height)
                {
                    comprasCollapsed = false;
                    ComprasTimer.Stop();
                }
            }
            else
            {
                ComprasContainer.Height -= 10;
                if (ComprasContainer.Height == ComprasContainer.MinimumSize.Height)
                {
                    comprasCollapsed = true;
                    ComprasTimer.Stop();
                }
            }
        }

        private void btnCompras_Click(object sender, EventArgs e)
        {
            ComprasTimer.Start();
        }

        private void ReportesTimer_Tick(object sender, EventArgs e)
        {
            if (reportesCollapsed)
            {
                ReportesContainer.Height += 10;
                if (ReportesContainer.Height == ReportesContainer.MaximumSize.Height)
                {
                    reportesCollapsed = false;
                    ReportesTimer.Stop();
                }
            }
            else
            {
                ReportesContainer.Height -= 10;
                if (ReportesContainer.Height == ReportesContainer.MinimumSize.Height)
                {
                    reportesCollapsed = true;
                    ReportesTimer.Stop();
                }
            }
        }

        private void btnReportes_Click(object sender, EventArgs e)
        {
            ReportesTimer.Start();
        }

        private void btnUsuarios_Click(object sender, EventArgs e)
        {
            AbrirFormulario(btnUsuarios, new frmUsuarios());
        }
    }
}
