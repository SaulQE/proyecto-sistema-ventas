using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CapaEntidad;
using System.Reflection;

namespace CapaDatos
{
    public class CD_Venta
    {
        public int ObtenerCorrelativo()
        {
            int idcorrelativo = 0; // Variable para almacenar el valor del próximo correlativo

            using (SqlConnection oconexion = new SqlConnection(Conexion.cadena))
            {
                try
                {
                    StringBuilder query = new StringBuilder();
                    query.AppendLine("select count(*) +1 from VENTA"); // Consulta para obtener el total de registros en la tabla VENTA y agregar 1 al resultado

                    SqlCommand cmd = new SqlCommand(query.ToString(), oconexion); // Creación de un comando SQL utilizando la consulta y la conexión
                    cmd.CommandType = CommandType.Text;                           // Especificación de que el tipo de comando es de texto

                    oconexion.Open();

                    idcorrelativo = Convert.ToInt32(cmd.ExecuteScalar()); // Ejecución de la consulta y asignación del resultado al valor de idcorrelativo

                }
                catch (Exception ex)
                {
                    idcorrelativo = 0; // En caso de que ocurra una excepción, se asigna 0 al valor de idcorrelativo
                }
            }
            return idcorrelativo; // Devuelve el valor del próximo correlativo
        }

        public bool RestarStock(int idproducto, int cantidad)
        {
            bool respuesta = true;

            using (SqlConnection oconoxion = new SqlConnection(Conexion.cadena))
            {
                try
                {
                    // Construir el comando SQL para actualizar el stock del producto restando la cantidad especificada.
                    StringBuilder query = new StringBuilder();
                    query.AppendLine("update PRODUCTO set Stock = Stock - @cantidad where id_Producto = @idproducto");

                    // Creación del objeto SqlCommand para ejecutar el comando SQL en la base de datos.
                    SqlCommand cmd = new SqlCommand(query.ToString(), oconoxion);

                    // Asignación de parámetros a la consulta SQL.
                    cmd.Parameters.AddWithValue("@cantidad", cantidad);
                    cmd.Parameters.AddWithValue("@idproducto", idproducto);
                    cmd.CommandType = CommandType.Text;

                    oconoxion.Open();

                    // Ejecutar el comando y verificar si se afectaron filas en la base de datos.
                    respuesta = cmd.ExecuteNonQuery() > 0 ? true : false;

                }catch (Exception ex)
                {
                    respuesta = false;
                }
            }
            return respuesta;
        }

        public bool SumarStock(int idproducto, int cantidad)
        {
            bool respuesta = true;

            using (SqlConnection oconoxion = new SqlConnection(Conexion.cadena))
            {
                try
                {
                    // Construir el comando SQL para actualizar el stock del producto sumando la cantidad especificada.
                    StringBuilder query = new StringBuilder();
                    query.AppendLine("update PRODUCTO set Stock = Stock + @cantidad where id_Producto = @idproducto");

                    // Creación del objeto SqlCommand para ejecutar el comando SQL en la base de datos.
                    SqlCommand cmd = new SqlCommand(query.ToString(), oconoxion);

                    // Asignación de parámetros a la consulta SQL.
                    cmd.Parameters.AddWithValue("@cantidad", cantidad);
                    cmd.Parameters.AddWithValue("@idproducto", idproducto);
                    cmd.CommandType = CommandType.Text;

                    oconoxion.Open();

                    // Ejecutar el comando y verificar si se afectaron filas en la base de datos.
                    respuesta = cmd.ExecuteNonQuery() > 0 ? true : false;

                }
                catch (Exception ex)
                {
                    respuesta = false;
                }
            }
            return respuesta;
        }

        public bool Registrar(Venta obj, DataTable DetalleVenta, out string Mensaje)
        {
            bool Respuesta = false; // Variable para almacenar la respuesta del registro
            Mensaje = string.Empty; // Variable para almacenar el mensaje de respuesta o cualquier mensaje de error

            try
            {
                using (SqlConnection oconexion = new SqlConnection(Conexion.cadena))
                {
                    SqlCommand cmd = new SqlCommand("SP_RegistrarVenta", oconexion);   // Creación de un nuevo comando SQL utilizando el procedimiento almacenado "SP_RegistrarVenta"
                    cmd.Parameters.AddWithValue("id_Usuario", obj.oUsuario.id_Usuario); // Asignación del valor al parámetro correspondiente
                    cmd.Parameters.AddWithValue("Tipo_Documento", obj.Tipo_Documento);
                    cmd.Parameters.AddWithValue("Nro_Documento", obj.Nro_Documento);
                    cmd.Parameters.AddWithValue("Documento_Cliente", obj.Documento_Cliente);
                    cmd.Parameters.AddWithValue("Nom_Cliente", obj.Nom_Cliente);
                    cmd.Parameters.AddWithValue("Monto_Pago", obj.Monto_Pago);
                    cmd.Parameters.AddWithValue("Monto_Cambio", obj.Monto_Cambio);
                    cmd.Parameters.AddWithValue("Monto_Total", obj.Monto_Total);
                    cmd.Parameters.AddWithValue("Detalle_Venta", DetalleVenta); // Asignación del valor de la tabla de detalle de venta al parámetro
                    cmd.Parameters.Add("Resultado", SqlDbType.Bit).Direction = ParameterDirection.Output;        // Definición del parámetro de salida para el resultado del registro
                    cmd.Parameters.Add("Mensaje", SqlDbType.VarChar, 500).Direction = ParameterDirection.Output; // Definición del parámetro de salida para el mensaje de respuesta

                    cmd.CommandType = CommandType.StoredProcedure; // Especificación de que el tipo de comando es un procedimiento almacenado

                    oconexion.Open();

                    cmd.ExecuteNonQuery();

                    Respuesta = Convert.ToBoolean(cmd.Parameters["Resultado"].Value); // Obtención del resultado y del mesaje a partir del parámetro de salida
                    Mensaje = cmd.Parameters["Mensaje"].Value.ToString();
                }

            } catch (Exception ex)
            {
                Respuesta = false;    // En caso de que ocurra una excepción, se asigna false a la respuesta del registro
                Mensaje = ex.Message; // Se asigna el mensaje de error a la variable de mensaje
            }
            

            return Respuesta; // Devuelve la respuesta del registro (true o false)
        }

        public Venta ObtenerVenta(string numero)
        {
            Venta obj = new Venta();

            using (SqlConnection conexion = new SqlConnection(Conexion.cadena))
            {
                try
                {
                    conexion.Open();
                    StringBuilder query = new StringBuilder();

                    query.AppendLine("select v.id_Venta,u.Nom_Completo,");
                    query.AppendLine("v.Documento_Cliente,v.Nom_Cliente,");
                    query.AppendLine("v.Tipo_Documento,v.Nro_Documento,");
                    query.AppendLine("v.Monto_Pago,v.Monto_Cambio,v.Monto_Total,");
                    query.AppendLine("convert(char(10), v.F_Registro, 103)[F_Registro]");
                    query.AppendLine("from VENTA v");
                    query.AppendLine("INNER JOIN USUARIO u on u.id_Usuario = v.id_Usuario");
                    query.AppendLine("where v.Nro_Documento = @numero");

                    // Se crea un comando SqlCommand con la consulta y la conexión.
                    SqlCommand cmd = new SqlCommand(query.ToString(), conexion);

                    // Se establece el parámetro @numero con el valor proporcionado en el metodo.
                    cmd.Parameters.AddWithValue("@numero", numero);

                    cmd.CommandType = CommandType.Text;

                    // Se ejecuta el comando y se obtiene un SqlDataReader para leer los resultados.
                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        // Se lee cada fila del resultado.
                        while (dr.Read())
                        {
                            // Se asignan los valores de las columnas del resultado a las propiedades del objeto Venta.
                            obj = new Venta()
                            {
                                id_Venta = int.Parse(dr["id_Venta"].ToString()),
                                oUsuario = new Usuario() { Nom_Completo = dr["Nom_Completo"].ToString() },
                                Documento_Cliente = dr["Documento_Cliente"].ToString(),
                                Nom_Cliente = dr["Nom_Cliente"].ToString(),
                                Tipo_Documento = dr["Tipo_Documento"].ToString(),
                                Nro_Documento = dr["Nro_Documento"].ToString(),
                                Monto_Pago = Convert.ToDecimal(dr["Monto_Pago"].ToString()),
                                Monto_Cambio = Convert.ToDecimal(dr["Monto_Cambio"].ToString()),
                                Monto_Total = Convert.ToDecimal(dr["Monto_Total"].ToString()),
                                F_Registro = dr["F_Registro"].ToString()
                            };
                        }
                    }
                }
                catch
                {
                    // Si ocurre una excepción, se crea una nueva instancia de Venta vacía.
                    obj = new Venta();
                }

            }
            // Se retorna el objeto venta obtenido.
            return obj;
        }

        public List<Detalle_Venta> ObtenerDetalleVenta(int idventa)
        {
            List<Detalle_Venta> oLista = new List<Detalle_Venta>();
            using (SqlConnection conexion = new SqlConnection(Conexion.cadena))
            {
                try
                {
                    conexion.Open();
                    // Se crea un objeto StringBuilder para construir la consulta SQL.
                    StringBuilder query = new StringBuilder();

                    // Se agrega la consulta SQL a la cadena de texto usando AppendLine para cada línea.
                    query.AppendLine("select p.Nombre,dv.Precio_Venta,dv.Cantidad,dv.SubTotal from DETALLE_VENTA dv");
                    query.AppendLine("INNER JOIN PRODUCTO p on p.id_Producto = dv.id_Producto");
                    query.AppendLine("where dv.id_Venta = @idventa");

                    // Se crea un comando SqlCommand con la consulta y la conexión.
                    SqlCommand cmd = new SqlCommand(query.ToString(), conexion);

                    // Se establece el parámetro @idventa con el valor proporcionado en el metodo.
                    cmd.Parameters.AddWithValue("@idventa", idventa);
                    cmd.CommandType = System.Data.CommandType.Text;

                    // Se ejecuta el comando y se obtiene un SqlDataReader para leer los resultados.
                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        // Se lee cada fila del resultado.
                        while (dr.Read())
                        {
                            // En el objeto creado de Detalle_Venta se asignan los valores de las columnas del resultado a sus propiedades.
                            oLista.Add(new Detalle_Venta()
                            {
                                oProducto = new Producto() { Nombre = dr["Nombre"].ToString() },
                                Precio_Venta = Convert.ToDecimal(dr["Precio_Venta"].ToString()),
                                Cantidad = Convert.ToInt32(dr["Cantidad"].ToString()),
                                SubTotal = Convert.ToDecimal(dr["SubTotal"].ToString())
                            });
                        }
                    }

                }
                catch
                {
                    // Si ocurre una excepción, se crea una nueva lista de Detalle_Venta vacía.
                    oLista = new List<Detalle_Venta>();
                }
            }
                return oLista;
        }


    }
}
