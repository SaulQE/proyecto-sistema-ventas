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
    public partial class frmDetalleCompra : Form
    {
        public frmDetalleCompra()
        {
            InitializeComponent();
        }

        private void btnbuscar_Click(object sender, EventArgs e)
        {
            Compra oCompra = new CN_Compra().ObtenerCompra(txtbusqueda.Text);

            if(oCompra.id_Compra != 0)
            {
                txtnumerodocumento.Text = oCompra.Nro_Documento;

                txtfecha.Text = oCompra.F_Registro;
                txttipodocumento.Text = oCompra.Tipo_Documento;
                txtusuario.Text = oCompra.oUsuario.Nom_Completo;
                txtdocproveedor.Text = oCompra.oProveedor.Documento;
                txtnombreproveedor.Text = oCompra.oProveedor.Razon_Social;
            }
            
        }
    }
}
