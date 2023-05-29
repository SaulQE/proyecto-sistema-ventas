using CapaEntidad;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
using System.Xml.Linq;

namespace CapaDatos
{
    public class CD_Compra
    {
        public int ObtenerCorrelativo()
        {
            int idcorrelativo = 0; // Variable para almacenar el valor del próximo correlativo

            using (SqlConnection oconexion = new SqlConnection(Conexion.cadena))
            {
                try
                {
                    StringBuilder query = new StringBuilder();
                    query.AppendLine("select count(*) +1 from COMPRA"); // Consulta para obtener el total de registros en la tabla COMPRA y agregar 1 al resultado

                    SqlCommand cmd = new SqlCommand(query.ToString(), oconexion); // Creación de un comando SQL utilizando la consulta y la conexión
                    cmd.CommandType = CommandType.Text;                           // Especificación de que el tipo de comando es de texto

                    oconexion.Open(); // Apertura de la conexión a la base de datos

                    idcorrelativo = Convert.ToInt32(cmd.ExecuteScalar()); // Ejecución de la consulta y asignación del resultado al valor de idcorrelativo

                }
                catch (Exception ex)
                {
                    idcorrelativo = 0; // En caso de que ocurra una excepción, se asigna 0 al valor de idcorrelativo
                }
            }
            return idcorrelativo; // Devuelve el valor del próximo correlativo
        }

        public bool Registrar(Compra obj, DataTable DetalleCompra, out string Mensaje)
        {
            bool Respuesta = false; // Variable para almacenar la respuesta del registro
            Mensaje = string.Empty; // Variable para almacenar el mensaje de respuesta o cualquier mensaje de error

            using (SqlConnection oconexion = new SqlConnection(Conexion.cadena))
            {
                try
                {
                    SqlCommand cmd = new SqlCommand("SP_RegistrarCompra", oconexion);   // Creación de un nuevo comando SQL utilizando el procedimiento almacenado "SP_RegistrarCompra"
                    cmd.Parameters.AddWithValue("id_Usuario", obj.oUsuario.id_Usuario); // Asignación del valor al parámetro correspondiente
                    cmd.Parameters.AddWithValue("id_Proveedor", obj.oProveedor.id_Proveedor); 
                    cmd.Parameters.AddWithValue("Tipo_Documento", obj.Tipo_Documento); 
                    cmd.Parameters.AddWithValue("Nro_Documento", obj.Nro_Documento); 
                    cmd.Parameters.AddWithValue("Monto_Total", obj.Monto_total); 
                    cmd.Parameters.AddWithValue("Detalle_Compra", DetalleCompra); // Asignación del valor de la tabla de detalle de compra al parámetro
                    cmd.Parameters.Add("Resultado", SqlDbType.Bit).Direction = ParameterDirection.Output;        // Definición del parámetro de salida para el resultado del registro
                    cmd.Parameters.Add("Mensaje", SqlDbType.VarChar, 500).Direction = ParameterDirection.Output; // Definición del parámetro de salida para el mensaje de respuesta

                    cmd.CommandType = CommandType.StoredProcedure; // Especificación de que el tipo de comando es un procedimiento almacenado

                    oconexion.Open();

                    cmd.ExecuteNonQuery(); 

                    Respuesta = Convert.ToBoolean(cmd.Parameters["Resultado"].Value); // Obtención del resultado y del mesaje a partir del parámetro de salida
                    Mensaje = cmd.Parameters["Mensaje"].Value.ToString();
                }
                catch (Exception ex)
                {
                    Respuesta = false;    // En caso de que ocurra una excepción, se asigna false a la respuesta del registro
                    Mensaje = ex.Message; // Se asigna el mensaje de error a la variable de mensaje
                }
            }

            return Respuesta; // Devuelve la respuesta del registro (true o false)
        }

        public Compra ObtenerCompra(string numero)
        {
            // Se crea una instancia de la clase Compra
            Compra obj = new Compra();
            // Se establece una conexión a la base de datos usando SqlConnection
            using (SqlConnection oconexion = new SqlConnection(Conexion.cadena))
            {
                try
                {
                    // Se crea un objeto StringBuilder para construir la consulta SQL.
                    StringBuilder query = new StringBuilder();

                    // Se agrega la consulta SQL a la cadena de texto usando AppendLine para cada línea.
                    query.AppendLine("select c.id_Compra,");
                    query.AppendLine("u.Nom_Completo,");
                    query.AppendLine("pr.Documento,pr.Razon_Social,");
                    query.AppendLine("c.Tipo_Documento, c.Nro_Documento, c.Monto_Total, convert(char(10),c.F_Registro,103)[F_Registro]");
                    query.AppendLine("from COMPRA c");
                    query.AppendLine("INNER JOIN USUARIO u on u.id_Usuario = c.id_Usuario");
                    query.AppendLine("INNER JOIN PROVEEDOR pr on pr.id_Proveedor = c.id_Proveedor");
                    query.AppendLine("where c.Nro_Documento = @numero");

                    // Se crea un comando SqlCommand con la consulta y la conexión.
                    SqlCommand cmd = new SqlCommand(query.ToString(), oconexion);
                    // Se establece el parámetro @numero con el valor proporcionado en el argumento.
                    cmd.Parameters.AddWithValue("@numero", numero); 
                    cmd.CommandType = CommandType.Text;

                    oconexion.Open();

                    // Se ejecuta el comando y se obtiene un SqlDataReader para leer los resultados.
                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        // Se lee cada fila del resultado.
                        while (dr.Read())
                        {
                            // Se asignan los valores de las columnas del resultado a las propiedades del objeto Compra.
                            obj = new Compra()
                            {
                                id_Compra = Convert.ToInt32(dr["id_Compra"]),
                                oUsuario = new Usuario() { Nom_Completo = dr["Nom_Completo"].ToString() },
                                oProveedor = new Proveedor() { Documento = dr["Documento"].ToString(), Razon_Social = dr["Razon_Social"].ToString() },
                                Tipo_Documento = dr["Tipo_Documento"].ToString(),
                                Nro_Documento = dr["Nro_Documento"].ToString(),
                                Monto_total = Convert.ToDecimal(dr["Monto_Total"].ToString()),
                                F_Registro = dr["F_Registro"].ToString()
                            };
                        }
                    }
                }
                catch (Exception ex)
                {
                    // Si ocurre una excepción, se crea una nueva instancia de Compra vacía.
                    obj = new Compra();
                }
            }
            // Se retorna el objeto Compra obtenido.
            return obj;
        }

        public List<Detalle_Compra> ObtenerDetalleCompra (int idcompra)
        {
            // Se crea una lista de Detalle_Compra para almacenar los detalles de la compra.
            List<Detalle_Compra> oLista = new List<Detalle_Compra>();
            try
            {
                // Se establece una conexión a la base de datos usando SqlConnection.
                using (SqlConnection conexion = new SqlConnection(Conexion.cadena))
                {
                    conexion.Open();
                    // Se crea un objeto StringBuilder para construir la consulta SQL.
                    StringBuilder query = new StringBuilder();

                    // Se agrega la consulta SQL a la cadena de texto usando AppendLine para cada línea.
                    query.AppendLine("select p.Nombre,dc.Precio_Compra,dc.Cantidad,dc.Monto_Total from DETALLE_COMPRA dc");
                    query.AppendLine("INNER JOIN PRODUCTO p on p.id_Producto = dc.id_Producto");
                    query.AppendLine("where dc.id_Compra = @idcompra");

                    // Se crea un comando SqlCommand con la consulta y la conexión.
                    SqlCommand cmd = new SqlCommand(query.ToString(), conexion);

                    // Se establece el parámetro @idcompra con el valor proporcionado en el metodo.
                    cmd.Parameters.AddWithValue("@idcompra", idcompra);
                    cmd.CommandType = System.Data.CommandType.Text;

                    // Se ejecuta el comando y se obtiene un SqlDataReader para leer los resultados.
                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        // Se lee cada fila del resultado.
                        while (dr.Read())
                        {
                            // En el objeto creado de Detalle_Compra se asignan los valores de las columnas del resultado a sus propiedades.
                            oLista.Add(new Detalle_Compra()
                            {
                                oProducto = new Producto() { Nombre = dr["Nombre"].ToString() },
                                Precio_Compra = Convert.ToDecimal(dr["Precio_Compra"].ToString()),
                                Cantidad = Convert.ToInt32(dr["Cantidad"].ToString()),
                                Monto_Total = Convert.ToDecimal(dr["Monto_Total"].ToString())
                            });
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                // Si ocurre una excepción, se crea una nueva lista de Detalle_Compra vacía.
                oLista = new List<Detalle_Compra>();
            }
            return oLista;
        }

    }
}
