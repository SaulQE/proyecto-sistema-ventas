using CapaEntidad;
using CapaNegocio;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CapaPresentacion
{
    public partial class frmNegocio : Form
    {
        public frmNegocio()
        {
            InitializeComponent();
        }

        public Image ByteToImage(byte[] imageBytes) { 
            MemoryStream ms = new MemoryStream(); // Nos permite guardar imagenes en memoria
            ms.Write(imageBytes, 0, imageBytes.Length); // Es para el ancho del array
            Image image = new Bitmap(ms); // Bitmap ayuda a hacer la conversión directamente

            return image;
        }

        private void frmNegocio_Load(object sender, EventArgs e)
        {
            bool obtenido = true;
            byte[] byteimage = new CN_Negocio().ObtenerLogo(out obtenido); // Si pudo leer el logo desde la base de datos y lo esta almacenando en el arrays de bytes

            if (obtenido)
                piclogo.Image = ByteToImage(byteimage); // De esta forma ya estaria pintando el logo

            Negocio datos = new CN_Negocio().ObtenerDatos(); // ObtenerDatos esta trayendo toda la información de la bd y lo esta almacenando en la varible datos que es de tipo negocio

            txtnombre.Text = datos.Nombre;
            txtruc.Text = datos.RUC;
            txtdireccion.Text = datos.Direccion;

        }

        private void btnsubir_Click(object sender, EventArgs e)
        {
            string mensaje = string.Empty;

            OpenFileDialog ofd = new OpenFileDialog();
            ofd.FileName = "Files|*.jpg;*.jpeg;*.png";

            if(ofd.ShowDialog() == DialogResult.OK){
                byte[] byteimage = File.ReadAllBytes(ofd.FileName); // Convierto mi imagen en un array de bytes y va leer el archivo seleccionado del opf
                bool respuesta = new CN_Negocio().ActualizarLogo(byteimage, out mensaje);

                if (respuesta) // Si la respuesta es true va tener que ser igual a la imagen que deseamos
                    piclogo.Image = ByteToImage(byteimage); // Llamo a mi metodo que convierte un array a una imagen
                else
                    MessageBox.Show(mensaje, "Mensaje", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);

            }

        }

        private void btnguardar_Click(object sender, EventArgs e)
        {
            string mensaje = string.Empty;

            Negocio obj = new Negocio()
            {
                Nombre = txtnombre.Text,
                RUC = txtruc.Text,
                Direccion = txtdireccion.Text
            };

            bool respuesta = new CN_Negocio().GuardarDatos(obj, out mensaje); // Con esto ya estaria guardando los datos

            if(respuesta)
                MessageBox.Show("Los cambios fueron guardados", "Mensaje", MessageBoxButtons.OK, MessageBoxIcon.Information);
            else
                MessageBox.Show("No se pudo guardar los cambios", "Mensaje", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
        }
    }
}
