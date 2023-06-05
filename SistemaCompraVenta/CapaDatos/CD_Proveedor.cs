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
    public class CD_Proveedor
    {
        public List<Proveedor> Listar()
        {
            List<Proveedor> Lista = new List<Proveedor>();

            using (SqlConnection oconexion = new SqlConnection(Conexion.cadena))
            {
                try
                {
                    // Crear consulta SQL para obtener los datos de Proveedor
                    StringBuilder query = new StringBuilder();
                    query.AppendLine("select id_Proveedor,Documento,Razon_Social,Correo,Telefono,Estado from PROVEEDOR");

                    // Crear el comando SQL y establecer la conexión y la consulta
                    SqlCommand cmd = new SqlCommand(query.ToString(), oconexion);
                    cmd.CommandType = CommandType.Text;

                    oconexion.Open();

                    // Ejecutar el comando SQL y obtener los datos usando SqlDataReader
                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            // Crear un objeto Proveedor y agregarlo a la lista
                            Lista.Add(new Proveedor()
                            {
                                id_Proveedor = Convert.ToInt32(dr["id_Proveedor"]),
                                Documento = dr["Documento"].ToString(),
                                Razon_Social = dr["Razon_Social"].ToString(),
                                Correo = dr["Correo"].ToString(),
                                Telefono = dr["Telefono"].ToString(),
                                Estado = Convert.ToBoolean(dr["Estado"])
                            });
                        }

                    }

                }
                catch (Exception ex)
                {
                    // Si ocurre una excepción, se crea una nueva lista vacía
                    Lista = new List<Proveedor>();

                }
            }
            return Lista;

        }


        //Metodo para registrar proveedor
        public int Registrar(Proveedor obj, out string Mensaje)
        {
            int idproveedorgenerado = 0;
            Mensaje = string.Empty; // El valor del mensaje esta vacio


            try
            {
                using (SqlConnection oconexion = new SqlConnection(Conexion.cadena))
                {
                    // Crear el comando SQL para llamar al procedimiento almacenado "SP_RegistrarProveedor"
                    SqlCommand cmd = new SqlCommand("SP_RegistrarProveedor", oconexion);// Establecer los parámetros del comando con los datos del proveedor
                    cmd.Parameters.AddWithValue("Documento", obj.Documento);
                    cmd.Parameters.AddWithValue("Razon_Social", obj.Razon_Social);
                    cmd.Parameters.AddWithValue("Correo", obj.Correo);
                    cmd.Parameters.AddWithValue("Telefono", obj.Telefono);
                    cmd.Parameters.AddWithValue("Estado", obj.Estado);

                    // Establecer los parámetros de salida del comando
                    cmd.Parameters.Add("Resultado", SqlDbType.Int).Direction = ParameterDirection.Output;
                    cmd.Parameters.Add("Mensaje", SqlDbType.VarChar, 500).Direction = ParameterDirection.Output;

                    cmd.CommandType = CommandType.StoredProcedure;


                    oconexion.Open();
                    // Ejecutar el comando SQL (procedimiento almacenado) para registrar el proveedor
                    cmd.ExecuteNonQuery();

                    // Obtener el ID de usuario generado y el mensaje
                    idproveedorgenerado = Convert.ToInt32(cmd.Parameters["Resultado"].Value);
                    Mensaje = cmd.Parameters["Mensaje"].Value.ToString();

                }

            }
            catch (Exception ex)
            {
                idproveedorgenerado = 0;
                Mensaje = ex.Message;
            }



            return idproveedorgenerado;
        }


        //Metodo para editar proveedor
        public bool Editar(Proveedor obj, out string Mensaje)
        {
            bool respuesta = false;
            Mensaje = string.Empty; // El valor del mensaje esta vacio


            try
            {
                using (SqlConnection oconexion = new SqlConnection(Conexion.cadena))
                {
                    SqlCommand cmd = new SqlCommand("SP_EditarProveedor", oconexion);
                    cmd.Parameters.AddWithValue("id_Proveedor", obj.id_Proveedor);
                    cmd.Parameters.AddWithValue("Documento", obj.Documento);
                    cmd.Parameters.AddWithValue("Razon_Social", obj.Razon_Social);
                    cmd.Parameters.AddWithValue("Correo", obj.Correo);
                    cmd.Parameters.AddWithValue("Telefono", obj.Telefono);
                    cmd.Parameters.AddWithValue("Estado", obj.Estado);
                    cmd.Parameters.Add("Resultado", SqlDbType.Int).Direction = ParameterDirection.Output;
                    cmd.Parameters.Add("Mensaje", SqlDbType.VarChar, 500).Direction = ParameterDirection.Output;
                    cmd.CommandType = CommandType.StoredProcedure;


                    oconexion.Open();
                    cmd.ExecuteNonQuery();

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

        // Metodo para eliminar proveedor
        public bool Eliminar(Proveedor obj, out string Mensaje)
        {
            bool respuesta = false;
            Mensaje = string.Empty; // El valor del mensaje esta vacio


            try
            {
                using (SqlConnection oconexion = new SqlConnection(Conexion.cadena))
                {
                    SqlCommand cmd = new SqlCommand("SP_EliminarProveedore", oconexion);
                    cmd.Parameters.AddWithValue("id_Proveedor", obj.id_Proveedor);
                    cmd.Parameters.Add("Resultado", SqlDbType.Int).Direction = ParameterDirection.Output;
                    cmd.Parameters.Add("Mensaje", SqlDbType.VarChar, 500).Direction = ParameterDirection.Output;
                    cmd.CommandType = CommandType.StoredProcedure;


                    oconexion.Open();
                    cmd.ExecuteNonQuery();

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

    }
}
