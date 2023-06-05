using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Data;
using System.Data.SqlClient;
using CapaEntidad;
using System.Reflection;
using System.Collections;
using System.Security.Claims;
using System.Xml.Linq;

namespace CapaDatos
{
    public class CD_Usuario
    {

        // Método para listar usuarios
        public List<Usuario> Listar()
        {
            List<Usuario> Lista = new List<Usuario> ();

            using (SqlConnection oconexion = new SqlConnection(Conexion.cadena))
            {
                try
                {
                    // Crear consulta SQL para obtener los datos de los usuarios
                    StringBuilder query = new StringBuilder();
                    query.AppendLine("select u.id_Usuario,u.Documento,u.Nom_Completo,u.Correo,u.Clave,u.Estado,r.id_Rol,r.Descripcion from usuario u");
                    query.AppendLine("inner join rol r on r.id_Rol = u.id_Rol");

                    // Crear el comando SQL y establecer la conexión y la consulta
                    SqlCommand cmd = new SqlCommand(query.ToString(), oconexion);
                    cmd.CommandType = CommandType.Text;

                    oconexion.Open();

                    // Ejecutar el comando SQL y obtener los datos usando SqlDataReader
                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            // Crear un objeto Usuario y agregarlo a la lista
                            Lista.Add(new Usuario()
                            {
                                id_Usuario = Convert.ToInt32(dr["id_Usuario"]),
                                Documento = dr["Documento"].ToString(),
                                Nom_Completo = dr["Nom_Completo"].ToString(),
                                Correo = dr["Correo"].ToString(),
                                Clave = dr["Clave"].ToString(),
                                Estado = Convert.ToBoolean(dr["Estado"]),
                                oRol = new Rol() { id_Rol = Convert.ToInt32(dr["id_Rol"]),Descripcion = dr["Descripcion"].ToString()}
                            });
                        }

                    }

                }catch (Exception ex)
                {
                    // Si ocurre una excepción, se crea una nueva lista vacía
                    Lista = new List<Usuario> ();

                }
            }
            return Lista;

        }


        // Método para registrar un nuevo usuario
        public int Registrar(Usuario obj, out string Mensaje)
        {
            int idusuariogenerado = 0;
            Mensaje = string.Empty; // El valor del mensaje esta vacio


            try
            {
                using (SqlConnection oconexion = new SqlConnection(Conexion.cadena))
                {
                    // Crear el comando SQL para llamar al procedimiento almacenado 
                    SqlCommand cmd = new SqlCommand("SP_REGISTRARUSUARIO", oconexion);

                    // Establecer los parámetros del comando con los datos del usuario
                    cmd.Parameters.AddWithValue("Documento",obj.Documento);
                    cmd.Parameters.AddWithValue("Nom_Completo", obj.Nom_Completo);
                    cmd.Parameters.AddWithValue("Correo", obj.Correo);
                    cmd.Parameters.AddWithValue("Clave", obj.Clave);
                    cmd.Parameters.AddWithValue("id_Rol", obj.oRol.id_Rol);
                    cmd.Parameters.AddWithValue("Estado", obj.Estado);

                    // Establecer los parámetros de salida del comando
                    cmd.Parameters.Add("id_UsuarioResultado",SqlDbType.Int).Direction = ParameterDirection.Output;
                    cmd.Parameters.Add("Mensaje", SqlDbType.VarChar,500).Direction = ParameterDirection.Output;

                    cmd.CommandType = CommandType.StoredProcedure;

                    oconexion.Open();
                    // Ejecutar el comando SQL (procedimiento almacenado) para registrar el usuario
                    cmd.ExecuteNonQuery();

                    // Obtener el ID de usuario generado y el mensaje de respuesta
                    idusuariogenerado = Convert.ToInt32(cmd.Parameters["id_UsuarioResultado"].Value);
                    Mensaje = cmd.Parameters["Mensaje"].Value.ToString();
                    
                }

            }
            catch (Exception ex)
            {
                // Si ocurre una excepción, se establecen los valores por defecto
                idusuariogenerado = 0;
                Mensaje = ex.Message;
            }



            return idusuariogenerado;
        }



        // Método para editar un usuario existente
        public bool Editar(Usuario obj, out string Mensaje)
        {
            bool respuesta = false;
            Mensaje = string.Empty; // El valor del mensaje esta vacio


            try
            {
                using (SqlConnection oconexion = new SqlConnection(Conexion.cadena))
                {
                    // Crear el comando SQL para llamar al procedimiento almacenado "SP_EDITARUSUARIO"
                    SqlCommand cmd = new SqlCommand("SP_EDITARUSUARIO", oconexion);
                    cmd.Parameters.AddWithValue("id_Usuario", obj.id_Usuario);
                    cmd.Parameters.AddWithValue("Documento", obj.Documento);
                    cmd.Parameters.AddWithValue("Nom_Completo", obj.Nom_Completo);
                    cmd.Parameters.AddWithValue("Correo", obj.Correo);
                    cmd.Parameters.AddWithValue("Clave", obj.Clave);
                    cmd.Parameters.AddWithValue("id_Rol", obj.oRol.id_Rol);
                    cmd.Parameters.AddWithValue("Estado", obj.Estado);

                    // Establecer los parámetros de salida del comando
                    cmd.Parameters.Add("Respuesta", SqlDbType.Int).Direction = ParameterDirection.Output;
                    cmd.Parameters.Add("Mensaje", SqlDbType.VarChar,500).Direction = ParameterDirection.Output;

                    cmd.CommandType = CommandType.StoredProcedure;

                    oconexion.Open();

                    // Ejecutar el comando SQL (procedimiento almacenado) para editar el usuario
                    cmd.ExecuteNonQuery();

                    // Obtener la respuesta y el mensaje de respuesta
                    respuesta = Convert.ToBoolean(cmd.Parameters["Respuesta"].Value);
                    Mensaje = cmd.Parameters["Mensaje"].Value.ToString();

                }

            }
            catch (Exception ex)
            {
                // Si ocurre una excepción, se establecen los valores por defecto
                respuesta = false;
                Mensaje = ex.Message;
            }

            return respuesta;
        }


        // Método para eliminar un usuario
        public bool Eliminar(Usuario obj, out string Mensaje)
        {
            bool respuesta = false;
            Mensaje = string.Empty; // El valor del mensaje esta vacio


            try
            {
                using (SqlConnection oconexion = new SqlConnection(Conexion.cadena))
                {
                    // Crear el comando SQL para llamar al procedimiento almacenado "SP_ELIMINARUSUARIO"
                    SqlCommand cmd = new SqlCommand("SP_ELIMINARUSUARIO", oconexion);
                    cmd.Parameters.AddWithValue("id_Usuario", obj.id_Usuario);

                    // Establecer los parámetros de salida del comando
                    cmd.Parameters.Add("Respuesta", SqlDbType.Int).Direction = ParameterDirection.Output;
                    cmd.Parameters.Add("Mensaje", SqlDbType.VarChar,500).Direction = ParameterDirection.Output;
                    cmd.CommandType = CommandType.StoredProcedure;


                    oconexion.Open();
                    // Ejecutar el comando SQL (procedimiento almacenado) para eliminar el usuario
                    cmd.ExecuteNonQuery();

                    // Obtener la respuesta y el mensaje de respuesta
                    respuesta = Convert.ToBoolean(cmd.Parameters["Respuesta"].Value);
                    Mensaje = cmd.Parameters["Mensaje"].Value.ToString();

                }

            }
            catch (Exception ex)
            {
                // Si ocurre una excepción, se establecen los valores por defecto
                respuesta = false;
                Mensaje = ex.Message;
            }

            return respuesta;
        }


    }
}
