﻿using CapaEntidad;
using CapaNegocio;
using CapaPresentacion.Modales;
using CapaPresentacion.Utilidades;
using DocumentFormat.OpenXml.Wordprocessing;
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
    public partial class frmCompras : Form
    {
        private Usuario _Usuario;

        public frmCompras(Usuario oUsuario = null)
        {
            _Usuario = oUsuario;
            InitializeComponent();
        }

        private void frmCompras_Load(object sender, EventArgs e)
        {
           
            cbotipodocumento.Items.Add(new OpcionCombo() { Valor = "Boleta", Texto = "Boleta" }); // Esto es para mi cbotipodocumento
            cbotipodocumento.Items.Add(new OpcionCombo() { Valor = "Factura", Texto = "Factura" });
            cbotipodocumento.DisplayMember = "Texto"; //aca mostrara el dato que tiene de nombre Texto
            cbotipodocumento.ValueMember = "Valor"; //no mostraria y manejaria como valor interno, y sera aquel dato con nombre Valor
            cbotipodocumento.SelectedIndex = 0; //Para siempre selecionar el primero

            txtfecha.Text = DateTime.Now.ToString("dd/MM/yyyy"); // Para mostrar la fecha en el txtfecha

            txtidproveedor.Text = "0";
            txtidproducto.Text = "0";
        }

        private void btnbuscarproveedor_Click(object sender, EventArgs e)
        {
            using (var modal = new mdProveedor())
            {
                var result = modal.ShowDialog(); // Hacer que se muestre el form y cualquier acción de resultado lo va obtener en la variable result

                if(result == DialogResult.OK) // Si cumple quiero que mis txt sean igual a mi form de Modal (mdProveedor) y accedera a sus atributos
                {
                    txtidproveedor.Text = modal._Proveedor.id_Proveedor.ToString();
                    txtdocproveedor.Text = modal._Proveedor.Documento;
                    txtnombreproveedor.Text = modal._Proveedor.Razon_Social;
                }
                else
                {
                    txtdocproveedor.Select();
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
                    txtpreciocompra.Select(); // Para hacer un focus (osea que se seleccione ahi)
                }
                else
                {
                    txtcodproducto.Select();
                }
            }
        }

        private void txtcodproducto_KeyDown(object sender, KeyEventArgs e)
        {
            if(e.KeyData == Keys.Enter) // Si la tecla del teclado es enter entonces
            {
                /* Se realiza una búsqueda en la lista de productos utilizando la clase CN_Producto para obtener un producto específico
                   El metodo listar devuelve una lista de productos y luego con el metodo where se hara el filtrado de productos segun las condiciones */

                Producto oProducto = new CN_Producto().Listar().Where(p => p.Codigo == txtcodproducto.Text && p.Estado == true).FirstOrDefault(); // FirstOrDefault para obtener el primer producto que cumpla las condiciones

                if (oProducto != null)
                {
                    txtcodproducto.BackColor = System.Drawing.Color.Honeydew; // Si encontro el producto se pintara de color verde
                    txtidproducto.Text = oProducto.id_Producto.ToString();
                    txtproducto.Text = oProducto.Nombre;
                    txtpreciocompra.Select();

                }
                else {
                    txtcodproducto.BackColor = System.Drawing.Color.MistyRose; // Si no encontro el producto se pintara igualmente
                    txtidproducto.Text = "0"; // El id Producto sera 0 porque no se ha encontrado
                    txtproducto.Text = ""; 

                }

            }


        }

        private void btnagregarproducto_Click(object sender, EventArgs e)
        {
            decimal preciocompra = 0;
            decimal precioventa = 0;
            bool producto_existe = false;

            if (int.Parse(txtidproducto.Text) == 0)
            {
                MessageBox.Show("Debe seleccionar un producto","Mensaje", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }
            if (!decimal.TryParse(txtpreciocompra.Text, out preciocompra)) // TryParce para convertirlo en un decimal, y sera true
            {
                MessageBox.Show("Precio Compra - Formato moneda incorrecto", "Mensaje", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                txtpreciocompra.Select();
                return;
            }
            if (!decimal.TryParse(txtprecioventa.Text, out precioventa))
            {
                MessageBox.Show("Precio Venta - Formato moneda incorrecto", "Mensaje", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                txtprecioventa.Select();
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
            if (!producto_existe) // con ! estoy negando el resultado si es falso lo niega y es true
            {
                dgvdata.Rows.Add(new object[]
                {
                    txtidproducto.Text,  // Agregando
                    txtproducto.Text,
                    preciocompra.ToString("0.00"), // Aqui menciono para que se pinten con decimales de 2
                    precioventa.ToString("0.00"),
                    txtcantidad.Value.ToString(), // no es textBox, es un NumericUpDown por eso accedo por la propiedad Value 
                    (txtcantidad.Value * preciocompra).ToString("0.00")

                });

                calcularTotal();
                limpiarProducto();
                txtcodproducto.Select();

            }



        }

        private void limpiarProducto()
        {
            txtidproducto.Text = "0";
            txtcodproducto.Text = "";
            txtcodproducto.BackColor = System.Drawing.Color.White;
            txtproducto.Text = "";
            txtpreciocompra.Text = "";
            txtprecioventa.Text = "";
            txtcantidad.Value = 1;

        }

        private void calcularTotal()
        {
            decimal total = 0;
            if(dgvdata.Rows.Count > 0){ // Conteo de todas las filas de mi dgvdata
                foreach (DataGridViewRow row in dgvdata.Rows)
                    /* Recorriendo cada uno de ellos y en especial esta recorreiendo la columna subtotal
                       y con el += estoy aumentando el valor de total */
                    total += Convert.ToDecimal(row.Cells["Monto_Total"].Value.ToString()); 
            }
            txttotalpagar.Text = total.ToString("0.00"); // Si no hay registros se pintara lo que tiene total
        }

        private void dgvdata_CellPainting(object sender, DataGridViewCellPaintingEventArgs e)
        {
            if (e.RowIndex < 0)
                return;
            if (e.ColumnIndex == 6)
            {
                e.Paint(e.CellBounds, DataGridViewPaintParts.All);

                var w = Properties.Resources.eliminar20.Width;
                var h = Properties.Resources.eliminar20.Height;
                var x = e.CellBounds.Left + (e.CellBounds.Width - w) / 2;
                var y = e.CellBounds.Top + (e.CellBounds.Height - h) / 2;

                e.Graphics.DrawImage(Properties.Resources.eliminar20, new Rectangle(x, y, w, h));
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
                    dgvdata.Rows.RemoveAt(indice); // Cuando elimine un item, tambien calcular el total
                    calcularTotal();

                }

            }
        }

        private void txtpreciocompra_KeyPress(object sender, KeyPressEventArgs e)
        {
            // Verifica si el carácter presionado es un dígito
            if (Char.IsDigit(e.KeyChar))
            {
                e.Handled = false; // Permite el ingreso del carácter
            }
            else
            {
                // Verifica si el campo de texto está vacío y el carácter presionado es un punto decimal
                if (txtpreciocompra.Text.Trim().Length == 0 && e.KeyChar.ToString() == ".")
                {
                    e.Handled = true; // Bloquea el ingreso del carácter
                }
                else {
                    // Verifica si el carácter presionado es un control o un punto decimal
                    if (Char.IsControl(e.KeyChar) || e.KeyChar.ToString() == ".")
                    {
                        e.Handled = false;
                    }
                    else {
                        e.Handled = true; 
                    }
                }
            }
        }

        private void txtprecioventa_KeyPress(object sender, KeyPressEventArgs e)
        {
            // Verifica si el carácter presionado es un dígito
            if (Char.IsDigit(e.KeyChar))
            {
                e.Handled = false; // Permite el ingreso del carácter
            }
            else
            {
                // Verifica si el campo de texto está vacío y el carácter presionado es un punto decimal
                if (txtprecioventa.Text.Trim().Length == 0 && e.KeyChar.ToString() == ".")
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

        private void btnregistrar_Click(object sender, EventArgs e)
        {
            // Verifica si no se ha seleccionado un proveedor
            if (Convert.ToInt32(txtidproveedor.Text) == 0)
            {
                MessageBox.Show("Debe selecionar un proveedor", "Mensaje", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return; // Sale del método
            }
            // Verifica si no se han ingresado productos en la compra
            if (dgvdata.Rows.Count < 1)
            {
                MessageBox.Show("Debe ingresar productos en la compra", "Mensaje", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }

            // Crea una DataTable para almacenar el detalle de la compra
            DataTable detalle_compra = new DataTable();

            // Agrega las columnas a la DataTable
            detalle_compra.Columns.Add("id_Producto", typeof(int));
            detalle_compra.Columns.Add("Precio_Compra", typeof(decimal));
            detalle_compra.Columns.Add("Precio_Venta", typeof(decimal));
            detalle_compra.Columns.Add("Cantidad", typeof(int));
            detalle_compra.Columns.Add("Monto_Total", typeof(decimal));

            // Recorre cada fila del DataGridView
            foreach (DataGridViewRow row in dgvdata.Rows)
            {
                // Agrega una nueva fila a la DataTable con los valores de la fila del DataGridView
                detalle_compra.Rows.Add(
                    new object[]{
                        Convert.ToInt32(row.Cells["id"].Value.ToString()),
                        row.Cells["Precio_Compra"].Value.ToString(),
                        row.Cells["Precio_Venta"].Value.ToString(),
                        row.Cells["Cantidad"].Value.ToString(),
                        row.Cells["Monto_Total"].Value.ToString(),
                    });
            }

            // Obtiene el correlativo de la compra
            int idcorrelativo = new CN_Compra().ObtenerCorrelativo();

            // Formatea el correlativo como un número de documento
            string numerodocumento = string.Format("{0:00000}", idcorrelativo);

            // Crea un objeto de tipo Compra con los datos ingresados
            Compra oCompra = new Compra()
            {
                oUsuario = new Usuario() { id_Usuario = _Usuario.id_Usuario },
                oProveedor = new Proveedor() { id_Proveedor = Convert.ToInt32(txtidproveedor.Text) },
                Tipo_Documento = ((OpcionCombo)cbotipodocumento.SelectedItem).Texto,
                Nro_Documento = numerodocumento,
                Monto_total = Convert.ToDecimal(txttotalpagar.Text),
            };

            string mensaje = string.Empty;

            // Llama al método Registrar() de la capa de negocio para registrar la compra
            bool respuesta = new CN_Compra().Registrar(oCompra, detalle_compra, out mensaje);

            if (respuesta)
            {
                // Muestra el número de compra generado y pregunta si desea copiarlo al portapapeles
                var result = MessageBox.Show("Número de compra generada:\n" + numerodocumento + "\n\n¿Desea copiar al portapapeles?"
                    ,"Mensaje",MessageBoxButtons.YesNo,MessageBoxIcon.Information);
                
                if (result == DialogResult.Yes)
                    Clipboard.SetText(numerodocumento);

                // Reinicia los campos y el DataGridView
                txtidproveedor.Text = "0";
                txtdocproveedor.Text = "";
                txtnombreproveedor.Text = "";
                dgvdata.Rows.Clear();
                calcularTotal();

            }
            else
            {
                MessageBox.Show(mensaje,"Mensaje",MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }

        }
    }
}