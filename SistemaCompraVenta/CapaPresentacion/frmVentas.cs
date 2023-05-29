using CapaEntidad;
using CapaNegocio;
using CapaPresentacion.Modales;
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

namespace CapaPresentacion
{
    public partial class frmVentas : Form
    {
        private Usuario _Usuario;
        public frmVentas(Usuario oUsuario = null)
        {
            _Usuario = oUsuario;
            InitializeComponent();
        }

        private void frmVentas_Load(object sender, EventArgs e)
        {
            cbotipodocumento.Items.Add(new OpcionCombo() { Valor = "Boleta", Texto = "Boleta" }); // Esto es para mi cbotipodocumento
            cbotipodocumento.Items.Add(new OpcionCombo() { Valor = "Factura", Texto = "Factura" });
            cbotipodocumento.DisplayMember = "Texto"; //aca mostrara el dato que tiene de nombre Texto
            cbotipodocumento.ValueMember = "Valor"; //no mostraria y manejaria como valor interno, y sera aquel dato con nombre Valor
            cbotipodocumento.SelectedIndex = 0; //Para siempre selecionar el primero

            txtfecha.Text = DateTime.Now.ToString("dd/MM/yyyy"); // Para mostrar la fecha en el txtfecha
            txtidproducto.Text = "0";

            txtpagacon.Text = "";
            txtcambio.Text = "";
            txttotalpagar.Text = "0.00";
        }

        private void btnbuscarcliente_Click(object sender, EventArgs e)
        {
            using (var modal = new mdCliente())
            {
                var result = modal.ShowDialog(); // Hacer que se muestre el form y cualquier acción de resultado lo va obtener en la variable result

                if (result == DialogResult.OK) // Si cumple quiero que mis txt sean igual a mi form de Modal (mdProveedor) y accedera a sus atributos
                {
                    txtdocliente.Text = modal._Cliente.Documento;
                    txtnombrecliente.Text = modal._Cliente.Nom_Completo;
                    txtcodproducto.Select();
                }
                else
                {
                    txtdocliente.Select();
                }

            }
        }

        private void btnbuscarproducto_Click(object sender, EventArgs e)
        {
            using (var modal = new mdProducto())
            {
                var result = modal.ShowDialog(); // Hacer que se muestre el form y cualquier acción de resultado lo va obtener en la variable result

                if (result == DialogResult.OK) // Si cumple quiero que mis txt sean igual a mi form de Modal (mdProveedor) y accedera a sus atributos
                {
                    txtidproducto.Text = modal._Producto.id_Producto.ToString();
                    txtcodproducto.Text = modal._Producto.Codigo;
                    txtproducto.Text = modal._Producto.Nombre;
                    txtprecio.Text = modal._Producto.Precio_Venta.ToString("0.00");
                    txtstock.Text = modal._Producto.Stock.ToString();
                    txtcantidad.Select(); // Para hacer un focus (osea que se seleccione ahi)
                }
                else
                {
                    txtcodproducto.Select();
                }
            }
        }

        private void txtcodproducto_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyData == Keys.Enter) // Si la tecla del teclado es enter entonces
            {
                /* Se realiza una búsqueda en la lista de productos utilizando la clase CN_Producto para obtener un producto específico
                   El metodo listar devuelve una lista de productos y luego con el metodo where se hara el filtrado de productos segun las condiciones */

                Producto oProducto = new CN_Producto().Listar().Where(p => p.Codigo == txtcodproducto.Text && p.Estado == true).FirstOrDefault(); // FirstOrDefault para obtener el primer producto que cumpla las condiciones

                if (oProducto != null)
                {
                    txtcodproducto.BackColor = System.Drawing.Color.Honeydew; // Si encontro el producto se pintara de color verde
                    txtidproducto.Text = oProducto.id_Producto.ToString();
                    txtproducto.Text = oProducto.Nombre;
                    txtprecio.Text = oProducto.Precio_Venta.ToString("0.00");
                    txtstock.Text = oProducto.Stock.ToString();
                    txtcantidad.Select();

                }
                else
                {
                    txtcodproducto.BackColor = System.Drawing.Color.MistyRose; // Si no encontro el producto se pintara igualmente
                    txtidproducto.Text = "0";
                    txtproducto.Text = "";
                    txtprecio.Text = "";
                    txtstock.Text = "";
                    txtcantidad.Value = 1;

                }

            }
        }

        private void btnagregarproducto_Click(object sender, EventArgs e)
        {
            decimal precio = 0;
            bool producto_existe = false;

            if (int.Parse(txtidproducto.Text) == 0)
            {
                MessageBox.Show("Debe seleccionar un producto", "Mensaje", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return; // Salir del metodo del click para no ejecutar ninguna linea que esta por debajo
            }

            if (!decimal.TryParse(txtprecio.Text, out precio)) // TryParce para convertirlo en un decimal, y sera true por que estoy negando false con !
            {
                MessageBox.Show("Precio - Formato moneda incorrecto", "Mensaje", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                txtprecio.Select();
                return;
            }

            if (Convert.ToInt32(txtstock.Text) < Convert.ToInt32(txtcantidad.Value.ToString()) )
            {
                MessageBox.Show("La cantidad no puede ser mayor al stock", "Mensaje", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }

            foreach (DataGridViewRow fila in dgvdata.Rows) // Esta recorriendo cada Rows(Fila) del dgvdata y estoy almacenando en fila
            {
                if (fila.Cells["id"].Value.ToString() == txtidproducto.Text) // Si id_Producto contiene ese id_Producto que estoy agregando
                {
                    producto_existe = true;
                    break;
                }
            }

            if (!producto_existe) // con ! estoy negando el resultado si es true lo niega y es lo vuelve false y viceversa
            {
                bool respuesta = new CN_Venta().RestarStock( // estoy pasando los parametros del metodo RestaStock
                    Convert.ToInt32( txtidproducto.Text),
                    Convert.ToInt32( txtcantidad.Value.ToString())
                    );

                if (respuesta)
                {
                    dgvdata.Rows.Add(new object[]
                    {
                        txtidproducto.Text,  // Agregando
                        txtproducto.Text,
                        precio.ToString("0.00"), // Aqui menciono para que se pinten con decimales de 2
                        txtcantidad.Value.ToString(), // no es textBox, es un NumericUpDown por eso accedo por la propiedad Value 
                        (txtcantidad.Value * precio).ToString("0.00")
                    });
                    // Ya habiendo registrado el producto en el dgvdata, empieza a calcular el total
                    calcularTotal();
                    limpiarProducto();
                    txtcodproducto.Select();
                }

            }

        }

        private void calcularTotal()
        {
            decimal total = 0;
            if (dgvdata.Rows.Count > 0)
            { // Conteo de todas las filas de mi dgvdata
                foreach (DataGridViewRow row in dgvdata.Rows)
                    /* Recorriendo cada uno de ellos y en especial esta recorreiendo la columna subtotal
                       y con el += estoy aumentando el valor de total */
                    total += Convert.ToDecimal(row.Cells["SubTotal"].Value.ToString());
            }
            txttotalpagar.Text = total.ToString("0.00"); // Si no hay registros se pintara lo que tiene total, y viceversa
        }

        private void limpiarProducto()
        {
            txtidproducto.Text = "0";
            txtcodproducto.Text = "";
            txtproducto.Text = "";
            txtprecio.Text = "";
            txtstock.Text = "";
            txtcantidad.Value = 1;

        }

        private void dgvdata_CellPainting(object sender, DataGridViewCellPaintingEventArgs e)
        {
            // Verificar si el indice de la fila es menor a 0, si es así retornara
            if (e.RowIndex < 0)
                return;

            // Verificar si la columna actual es la columna número 5.
            if (e.ColumnIndex == 5)
            {
                // Realizar el pintado de todas las partes de la celda.
                e.Paint(e.CellBounds, DataGridViewPaintParts.All);

                // Obtener las dimensiones del ícono "eliminar20" definido en los recursos.
                var w = Properties.Resources.eliminar20.Width;
                var h = Properties.Resources.eliminar20.Height;

                // Calcular la posición, y del ícono dentro de la celda para centrarlo.
                var x = e.CellBounds.Left + (e.CellBounds.Width - w) / 2;
                var y = e.CellBounds.Top + (e.CellBounds.Height - h) / 2;

                // Dibujar el ícono "eliminar20" en la posición calculada.
                e.Graphics.DrawImage(Properties.Resources.eliminar20, new Rectangle(x, y, w, h));

                // Indicar que el evento ha sido manejado
                e.Handled = true;
            }
        }

        private void dgvdata_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (dgvdata.Columns[e.ColumnIndex].Name == "btneliminar")
            {
                int indice = e.RowIndex; // Aqui estoy diciendo que fila voy a eliminar por medio del indice

                if (indice >= 0)
                {
                    bool respuesta = new CN_Venta().SumarStock( // estoy pasando los parametros del metodo SumarStock
                        Convert.ToInt32( dgvdata.Rows[indice].Cells["id"].Value.ToString()),
                        Convert.ToInt32( dgvdata.Rows[indice].Cells["Cantidad"].Value.ToString()));

                    if (respuesta)
                    {
                        dgvdata.Rows.RemoveAt(indice); // Cuando elimine un item, tambien calcular el total
                        calcularTotal();
                    }

                }

            }
        }

        private void txtprecio_KeyPress(object sender, KeyPressEventArgs e)
        {
            // Verifica si el carácter presionado es un dígito
            if (Char.IsDigit(e.KeyChar))
            {
                e.Handled = false; // Permite el ingreso del carácter
            }
            else
            {
                // Verifica si el campo de texto está vacío y el carácter presionado es un punto decimal
                if (txtprecio.Text.Trim().Length == 0 && e.KeyChar.ToString() == ".")
                {
                    e.Handled = true; // Bloquea el ingreso del carácter
                }
                else
                {
                    // Verifica si el carácter presionado es un control o un punto decimal
                    if (Char.IsControl(e.KeyChar) || e.KeyChar.ToString() == ".")
                    {
                        e.Handled = false;
                    }
                    else
                    {
                        e.Handled = true;
                    }
                }
            }
        }

        private void txtpagacon_KeyPress(object sender, KeyPressEventArgs e)
        {
            // Verifica si el carácter presionado es un dígito
            if (Char.IsDigit(e.KeyChar))
            {
                e.Handled = false; // Permite el ingreso del carácter
            }
            else
            {
                // Verifica si el campo de texto está vacío y el carácter presionado es un punto decimal
                if (txtpagacon.Text.Trim().Length == 0 && e.KeyChar.ToString() == ".")
                {
                    e.Handled = true; // Bloquea el ingreso del carácter
                }
                else
                {
                    // Verifica si el carácter presionado es un control o un punto decimal
                    if (Char.IsControl(e.KeyChar) || e.KeyChar.ToString() == ".")
                    {
                        e.Handled = false;
                    }
                    else
                    {
                        e.Handled = true;
                    }
                }
            }
        }

        private void calcularCambio()
        {
            // Verificar si el campo de texto txttotalpagar está vacío.
            if (txttotalpagar.Text.Trim() == "")
            {
                MessageBox.Show("No existen productos en la venta", "Mensaje", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }

            decimal pagacon;
            decimal total = Convert.ToDecimal(txttotalpagar.Text);

            // Verificar si el campo de texto txtpagacon está vacío y establecer su valor a "0" si es así.
            if (txtpagacon.Text.Trim() == "")
            {
                txtpagacon.Text = "0";
            }

            // Intentar convertir el valor del campo de texto txtpagacon a decimal.
            if (decimal.TryParse(txtpagacon.Text.Trim(), out pagacon))
            {
                // Verificar si la cantidad pagada es menor al total.
                if (pagacon < total)
                {
                    txtcambio.Text = "0.00";
                }
                else
                {
                    decimal cambio = pagacon - total;
                    // Establecer el valor calculado en el campo de texto txtcambio formateado como "0.00".
                    txtcambio.Text = cambio.ToString("0.00");

                }
            }

        }

        private void txtpagacon_KeyDown(object sender, KeyEventArgs e)
        {
            // Verificar si se presionó la tecla Enter.
            if (e.KeyData == Keys.Enter)
            {
                // Llamar al método calcularCambio.
                calcularCambio();
            }
        }

        private void btncrearventa_Click(object sender, EventArgs e)
        {

            if (txtdocliente.Text == "")
            {
                MessageBox.Show("Debe ingresar documento del cliente","Mensaje",MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }

            if (txtnombrecliente.Text == "")
            {
                MessageBox.Show("Debe ingresar nombre del cliente", "Mensaje", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }

            if (dgvdata.Rows.Count < 1)
            {
                MessageBox.Show("Debe ingresar productos en la venta", "Mensaje", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }

            DataTable detalle_venta = new DataTable();

            detalle_venta.Columns.Add("id_Producto", typeof(int));
            detalle_venta.Columns.Add("Precio_Venta", typeof(decimal));
            detalle_venta.Columns.Add("Cantidad", typeof(int));
            detalle_venta.Columns.Add("SubTotal", typeof(decimal));

            foreach (DataGridViewRow row in dgvdata.Rows)
            {
                detalle_venta.Rows.Add(new object[]
                {
                    row.Cells["id"].Value.ToString(),
                    row.Cells["Precio_Venta"].Value.ToString(),
                    row.Cells["Cantidad"].Value.ToString(),
                    row.Cells["SubTotal"].Value.ToString(),
                });
            }

            int idcorrelativo = new CN_Venta().ObtenerCorrelativo();
            string numeroDocumento = string.Format("{0:00000}", idcorrelativo);
            calcularCambio();

            Venta oVenta = new Venta()
            {
                oUsuario = new Usuario() { id_Usuario = _Usuario.id_Usuario },
                Tipo_Documento = ((OpcionCombo)cbotipodocumento.SelectedItem).Texto,
                Nro_Documento = numeroDocumento,
                Documento_Cliente = txtdocliente.Text,
                Nom_Cliente = txtnombrecliente.Text,
                Monto_Pago = Convert.ToDecimal(txtpagacon.Text),
                Monto_Cambio = Convert.ToDecimal(txtcambio.Text),
                Monto_Total = Convert.ToDecimal(txttotalpagar.Text)
            };

            string mensaje = string.Empty;
            bool respuesta = new CN_Venta().Registrar(oVenta, detalle_venta, out mensaje);

            if (respuesta)
            {
                var result = MessageBox.Show("Número de venta generado:\n" + numeroDocumento + "\n\n¿Desea copiar al portapapeles?", "Mensaje",
                    MessageBoxButtons.YesNo, MessageBoxIcon.Information);

                if (result == DialogResult.Yes)
                    Clipboard.SetText(numeroDocumento);

                txtdocliente.Text = "";
                txtnombrecliente.Text = "";
                dgvdata.Rows.Clear();
                calcularTotal();
                txtpagacon.Text = "";
                txtcambio.Text = "";
            }
            else
                MessageBox.Show(mensaje, "Mensaje", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);

        }
    }
}
