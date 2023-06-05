using CapaEntidad;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaDatos
{
    public class CD_Cliente
    {

        public List<Cliente> Listar()
        {
            List<Cliente> Lista = new List<Cliente>(); // Crear una nueva lista de tipo Cliente

            using (SqlConnection oconexion = new SqlConnection(Conexion.cadena))
            {
                try
                {
                    StringBuilder query = new StringBuilder(); // Crear una nueva instancia de StringBuilder para construir la consulta SQL
                    query.AppendLine("select id_Cliente,Documento,Nom_Completo,Correo,Telefono,Estado from CLIENTE");

                    SqlCommand cmd = new SqlCommand(query.ToString(), oconexion);
                    cmd.CommandType = CommandType.Text;

                    oconexion.Open();

                    // Ejecutar el comando y obtener un lector de datos
                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        while (dr.Read()) // Leer cada fila de datos
                        {
                            // Crear un nuevo objeto Cliente y agregarlo a la lista
                            Lista.Add(new Cliente()
                            {
                                // Asignar valores a las propiedades del objeto Cliente
                                id_Cliente = Convert.ToInt32(dr["id_Cliente"]),
                                Documento = dr["Documento"].ToString(),
                                Nom_Completo = dr["Nom_Completo"].ToString(),
                                Correo = dr["Correo"].ToString(),
                                Telefono = dr["Telefono"].ToString(),
                                Estado = Convert.ToBoolean(dr["Estado"])
                            });
                        }
                    }
                }
                catch (Exception ex)
                {
                    // En caso de error, asignar una lista vacía
                    Lista = new List<Cliente>();

                }
            }
            return Lista; // Devolver la lista de Clientes

        }


        // Metodo para registrar cliente
        public int Registrar(Cliente obj, out string Mensaje)
        {
            int idClientegenerado = 0;
            Mensaje = string.Empty; // El valor del mensaje esta vacio


            try
            {
                using (SqlConnection oconexion = new SqlConnection(Conexion.cadena))
                {
                    // Crear un nuevo comando SQL para invocar un procedimiento almacenado
                    SqlCommand cmd = new SqlCommand("SP_RegistrarCliente", oconexion);
                    cmd.Parameters.AddWithValue("Documento", obj.Documento); // Asignar valores a los parámetros del comando
                    cmd.Parameters.AddWithValue("Nom_Completo", obj.Nom_Completo);
                    cmd.Parameters.AddWithValue("Correo", obj.Correo);
                    cmd.Parameters.AddWithValue("Telefono", obj.Telefono);
                    cmd.Parameters.AddWithValue("Estado", obj.Estado);
                    // Parametros de salida
                    cmd.Parameters.Add("Resultado", SqlDbType.Int).Direction = ParameterDirection.Output;
                    cmd.Parameters.Add("Mensaje", SqlDbType.VarChar, 500).Direction = ParameterDirection.Output;

                    cmd.CommandType = CommandType.StoredProcedure;


                    oconexion.Open();
                    cmd.ExecuteNonQuery(); // Ejecutar el comando

                    //Obtengo los valores de los parametros de salida
                    idClientegenerado = Convert.ToInt32(cmd.Parameters["Resultado"].Value);
                    Mensaje = cmd.Parameters["Mensaje"].Value.ToString();

                }
            }
            catch (Exception ex)
            {
                // Capturar la excepción y obtener el mensaje de error
                idClientegenerado = 0;
                Mensaje = ex.Message;
            }

            return idClientegenerado; // Devolver el ID generado
        }



        // Metodo editar Cliente
        public bool Editar(Cliente obj, out string Mensaje)
        {
            bool respuesta = false;
            Mensaje = string.Empty; // El valor del mensaje esta vacio


            try
            {
                using (SqlConnection oconexion = new SqlConnection(Conexion.cadena))
                {
                    SqlCommand cmd = new SqlCommand("SP_EditarCliente", oconexion); // Crear un nuevo comando SQL para invocar un procedimiento almacenado
                    cmd.Parameters.AddWithValue("id_Cliente", obj.id_Cliente); // Asignar valores a los parámetros del comando
                    cmd.Parameters.AddWithValue("Documento", obj.Documento);
                    cmd.Parameters.AddWithValue("Nom_Completo", obj.Nom_Completo);
                    cmd.Parameters.AddWithValue("Correo", obj.Correo);
                    cmd.Parameters.AddWithValue("Telefono", obj.Telefono);
                    cmd.Parameters.AddWithValue("Estado", obj.Estado);
                    // Parametros de salida
                    cmd.Parameters.Add("Resultado", SqlDbType.Int).Direction = ParameterDirection.Output;
                    cmd.Parameters.Add("Mensaje", SqlDbType.VarChar, 500).Direction = ParameterDirection.Output;
                    cmd.CommandType = CommandType.StoredProcedure;

                    oconexion.Open();
                    cmd.ExecuteNonQuery(); // Ejecutar el comando

                    // Obtener el valor del parámetro de salida
                    respuesta = Convert.ToBoolean(cmd.Parameters["Resultado"].Value);
                    Mensaje = cmd.Parameters["Mensaje"].Value.ToString();

                }
            }
            catch (Exception ex)
            {
                respuesta = false;
                Mensaje = ex.Message;
            }

            return respuesta; // Devolver la respuesta (true o false)
        }


        // Metodo para eliminar clientes
        public bool Eliminar(Cliente obj, out string Mensaje)
        {
            bool respuesta = false;
            Mensaje = string.Empty; // El valor del mensaje esta vacio

            try
            {
                using (SqlConnection oconexion = new SqlConnection(Conexion.cadena))
                {
                    // Crear un nuevo comando SQL para eliminar un cliente por su ID
                    SqlCommand cmd = new SqlCommand("delete from cliente where id_Cliente = @id", oconexion);
                    cmd.Parameters.AddWithValue("@id", obj.id_Cliente); // Asignar valor al parámetro del comando
                    cmd.CommandType = CommandType.Text;  // Establecer el tipo de comando como un texto SQL


                    oconexion.Open();
                    respuesta = cmd.ExecuteNonQuery() > 0 ? true : false; // Ejecutar el comando y comprobar si se afectaron filas

                }
            }
            catch (Exception ex)
            {
                // Capturar la excepción y obtener el mensaje de error
                respuesta = false;
                Mensaje = ex.Message;
            }

            return respuesta; // Devolver la respuesta (true o false)
        }

    }
}
