using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CapaEntidad;

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
                    StringBuilder query = new StringBuilder();
                    query.AppendLine("update PRODUCTO set Stock = Stock - @cantidad where id_Producto = @idproducto");

                    SqlCommand cmd = new SqlCommand(query.ToString(), oconoxion);
                    cmd.Parameters.AddWithValue("@cantidad", cantidad);
                    cmd.Parameters.AddWithValue("@idproducto", idproducto);
                    cmd.CommandType = CommandType.Text;

                    oconoxion.Open();

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
                    StringBuilder query = new StringBuilder();
                    query.AppendLine("update PRODUCTO set Stock = Stock + @cantidad where id_Producto = @idproducto");

                    SqlCommand cmd = new SqlCommand(query.ToString(), oconoxion);
                    cmd.Parameters.AddWithValue("@cantidad", cantidad);
                    cmd.Parameters.AddWithValue("@idproducto", idproducto);
                    cmd.CommandType = CommandType.Text;

                    oconoxion.Open();

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


    }
}
