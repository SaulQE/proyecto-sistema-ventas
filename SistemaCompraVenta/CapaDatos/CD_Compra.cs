using CapaEntidad;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
                    cmd.Parameters.Add("Resultado", SqlDbType.Int).Direction = ParameterDirection.Output;        // Definición del parámetro de salida para el resultado del registro
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

    }
}
