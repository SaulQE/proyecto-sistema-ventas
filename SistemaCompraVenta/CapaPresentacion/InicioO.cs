using CapaEntidad;
using CapaNegocio;
using CapaPresentacion.Modales;
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
        private List<string> permisosUsuario;

        public InicioO(Usuario objusuario = null)
        {
            if (objusuario != null)
                usuarioActual = objusuario;
              //usuarioActual = new Usuario() { Nom_Completo = "ADMIN PREDEFINIDO", id_Usuario = 1 };
            //else


            InitializeComponent();
        }

        private void InicioO_Load(object sender, EventArgs e)
        {
            List<Permiso> ListaPermisos = new CN_Permiso().Listar(usuarioActual.id_Usuario);

            foreach (Control panel in sidebar.Controls) // Recorre los paneles dentro del panel sidebar
            {
                if (panel is Panel submenu)
                {
                    foreach (Button button in submenu.Controls.OfType<Button>()) // Recorre los botones dentro del panel secundario
                    {
                        bool encontrado = ListaPermisos.Any(m => m.Nom_Menu == button.Name);

                        if (!encontrado)
                        {
                            button.Visible = false;
                        }
                    }
                }
            }

            lblusuario.Text = usuarioActual.Nom_Completo;
        }

        private void AbrirFormulario(Button sidebar, Form formulario)
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

        private void menumantenimiento_Click(object sender, EventArgs e)
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

        private void menusuarios_Click(object sender, EventArgs e)
        {
            AbrirFormulario(menusuarios, new frmUsuarios());
        }

        private void submenucategoria_Click(object sender, EventArgs e)
        {
            AbrirFormulario(submenucategoria, new frmCategoria());
        }

        private void submenuproducto_Click(object sender, EventArgs e)
        {
            AbrirFormulario(submenuproducto, new frmProducto());
        }

        private void submenunegocio_Click(object sender, EventArgs e)
        {
            AbrirFormulario(submenunegocio, new frmNegocio());
        }

        private void submenuregistrarventa_Click(object sender, EventArgs e)
        {
            AbrirFormulario(submenuregistrarventa, new frmVentas(usuarioActual));
        }

        private void submenuverdetalleventa_Click(object sender, EventArgs e)
        {
            AbrirFormulario(submenuverdetalleventa, new frmDetalleVenta());
        }

        private void submenuregistrarcompra_Click(object sender, EventArgs e)
        {
            AbrirFormulario(submenuregistrarcompra, new frmCompras(usuarioActual));
        }

        private void submenuverdetallecompra_Click(object sender, EventArgs e)
        {
            AbrirFormulario(submenuverdetallecompra, new frmDetalleCompra());
        }

        private void btnClientes_Click(object sender, EventArgs e)
        {
            AbrirFormulario(menuclientes, new frmClientes());
        }

        private void btnProveedores_Click(object sender, EventArgs e)
        {
            AbrirFormulario(menuproveedores, new frmProveedores());
        }

        private void submenureportecompra_Click(object sender, EventArgs e)
        {
            AbrirFormulario(submenureportecompras, new frmReporteCompras());
        }

        private void submenureporteventa_Click(object sender, EventArgs e)
        {
            AbrirFormulario(submenureporteventas, new frmReporteVentas());
        }

        private void menuacercade_Click(object sender, EventArgs e)
        {
            mdAcercade md = new mdAcercade();
            md.ShowDialog();
        }

        private void btnSalir_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("¿Desea salir?", "Mensaje", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                this.Close();
            }
        }
    }
}
