using CapaEntidad;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaDatos
{
    public class CD_Categoria
    {

        // Método para listar todas las categorías
        public List<Categoria> Listar()
        {
            List<Categoria> Lista = new List<Categoria>();

            using (SqlConnection oconexion = new SqlConnection(Conexion.cadena))
            {
                try
                {
                    // Construir la consulta SQL
                    StringBuilder query = new StringBuilder();
                    query.AppendLine("select id_Categoria,Descripcion,Estado from CATEGORIA");

                    // Crear el comando SQL
                    SqlCommand cmd = new SqlCommand(query.ToString(), oconexion);
                    cmd.CommandType = CommandType.Text;

                    oconexion.Open();

                    // Ejecutar la consulta y leer los resultados
                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            // Crear un objeto Categoria y agregarlo a la lista
                            Lista.Add(new Categoria()
                            {
                                id_Categoria = Convert.ToInt32(dr["id_Categoria"]),
                                Descripcion = dr["Descripcion"].ToString(),
                                Estado = Convert.ToBoolean(dr["Estado"])
                            });
                        }
                    }
                }
                catch (Exception ex)
                {
                    // En caso de error, se crea una nueva lista vacía
                    Lista = new List<Categoria>();

                }
            }
            return Lista;
        }


        // Metodo para registrar categoria
        public int Registrar(Categoria obj, out string Mensaje)
        {
            int idCategoriagenerado = 0;
            Mensaje = string.Empty; // El valor del mensaje esta vacio


            try
            {
                using (SqlConnection oconexion = new SqlConnection(Conexion.cadena))
                {
                    // Crear un nuevo comando SQL para invocar un procedimiento almacenado
                    SqlCommand cmd = new SqlCommand("SP_RegistrarCategoria", oconexion); // Asignar valores a los parámetros del comando
                    cmd.Parameters.AddWithValue("Descripcion", obj.Descripcion);
                    cmd.Parameters.AddWithValue("Estado", obj.Estado);

                    // Parametros de salida
                    cmd.Parameters.Add("Resultado", SqlDbType.Int).Direction = ParameterDirection.Output;
                    cmd.Parameters.Add("Mensaje", SqlDbType.VarChar, 500).Direction = ParameterDirection.Output;

                    cmd.CommandType = CommandType.StoredProcedure;


                    oconexion.Open();
                    cmd.ExecuteNonQuery();

                    //Obtengo los valores de los parametros de salida
                    idCategoriagenerado = Convert.ToInt32(cmd.Parameters["Resultado"].Value);
                    Mensaje = cmd.Parameters["Mensaje"].Value.ToString();

                }

            }
            catch (Exception ex)
            {
                // Capturar la excepción y obtener el mensaje de error
                idCategoriagenerado = 0;
                Mensaje = ex.Message;
            }
            return idCategoriagenerado;
        }



        public bool Editar(Categoria obj, out string Mensaje)
        {
            bool respuesta = false;
            Mensaje = string.Empty; // El valor del mensaje esta vacio


            try
            {
                using (SqlConnection oconexion = new SqlConnection(Conexion.cadena))
                {
                    // Crear un nuevo comando SQL para invocar un procedimiento almacenado
                    SqlCommand cmd = new SqlCommand("SP_EditarCategoria", oconexion);
                    cmd.Parameters.AddWithValue("id_Categoria", obj.id_Categoria);// Asignar valores a los parámetros del comando
                    cmd.Parameters.AddWithValue("Descripcion", obj.Descripcion);
                    cmd.Parameters.AddWithValue("Estado", obj.Estado);
                    // Parametros de salida
                    cmd.Parameters.Add("Resultado", SqlDbType.Int).Direction = ParameterDirection.Output;
                    cmd.Parameters.Add("Mensaje", SqlDbType.VarChar, 500).Direction = ParameterDirection.Output;
                    cmd.CommandType = CommandType.StoredProcedure;

                    oconexion.Open();

                    cmd.ExecuteNonQuery();

                    //Obtengo los valores de los parametros de salida
                    respuesta = Convert.ToBoolean(cmd.Parameters["Resultado"].Value);
                    Mensaje = cmd.Parameters["Mensaje"].Value.ToString();

                }

            }
            catch (Exception ex)
            {
                respuesta = false;
                Mensaje = ex.Message;
            }

            return respuesta;
        }


        public bool Eliminar(Categoria obj, out string Mensaje)
        {
            bool Resultado = false;
            Mensaje = string.Empty; // El valor del mensaje esta vacio


            try
            {
                using (SqlConnection oconexion = new SqlConnection(Conexion.cadena))
                {
                    // Crear un nuevo comando SQL para invocar un procedimiento almacenado
                    SqlCommand cmd = new SqlCommand("SP_EliminarCategoria", oconexion);
                    cmd.Parameters.AddWithValue("id_Categoria", obj.id_Categoria);// Asignar valores a los parámetros del comando
                    // Parametros de salida
                    cmd.Parameters.Add("Resultado", SqlDbType.Int).Direction = ParameterDirection.Output;
                    cmd.Parameters.Add("Mensaje", SqlDbType.VarChar,500).Direction = ParameterDirection.Output;
                    cmd.CommandType = CommandType.StoredProcedure;

                    oconexion.Open();
                    cmd.ExecuteNonQuery();

                    //Obtengo los valores de los parametros de salida
                    Resultado = Convert.ToBoolean(cmd.Parameters["Resultado"].Value);
                    Mensaje = cmd.Parameters["Mensaje"].Value.ToString();

                }

            }
            catch (Exception ex)
            {
                Resultado = false;
                Mensaje = ex.Message;
            }

            return Resultado;
        }
    }
}
