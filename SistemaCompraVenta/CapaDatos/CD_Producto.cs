using CapaEntidad;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections;
using System.Reflection;

namespace CapaDatos
{
    public class CD_Producto
    {

        public List<Producto> Listar()
        {
            List<Producto> Lista = new List<Producto>(); // Crear una lista de objetos Producto

            using (SqlConnection oconexion = new SqlConnection(Conexion.cadena))
            {
                try
                {
                    // Construir la consulta SQL para obtener los datos de los productos y sus categorías
                    StringBuilder query = new StringBuilder();
                    query.AppendLine("select id_Producto, Codigo, Nombre, p.Descripcion,c.id_Categoria,c.Descripcion[DescripcionCategoria],Stock,Precio_Compra,Precio_Venta,p.Estado from PRODUCTO p");
                    query.AppendLine("inner join CATEGORIA c on c.id_Categoria = p.id_Categoria");
                    SqlCommand cmd = new SqlCommand(query.ToString(), oconexion); // Crear un nuevo comando SQL
                    cmd.CommandType = CommandType.Text;

                    oconexion.Open();

                    using (SqlDataReader dr = cmd.ExecuteReader()) // Ejecutar el comando y obtener un lector de datos
                    {
                        // Iterar sobre los registros del lector de datos
                        while (dr.Read())
                        {
                            // Crear un nuevo objeto Producto y asignar los valores de las columnas del registro actual
                            Lista.Add(new Producto()
                            {
                                id_Producto = Convert.ToInt32(dr["id_Producto"]),
                                Codigo = dr["Codigo"].ToString(),
                                Nombre = dr["Nombre"].ToString(),
                                Descripcion = dr["Descripcion"].ToString(),
                                oCategoria = new Categoria() { id_Categoria = Convert.ToInt32(dr["id_Categoria"]),Descripcion = dr["DescripcionCategoria"].ToString() },
                                Stock = Convert.ToInt32( dr["Stock"].ToString()),
                                Precio_Compra = Convert.ToDecimal(dr["Precio_Compra"].ToString()),
                                Precio_Venta = Convert.ToDecimal(dr["Precio_Venta"].ToString()),
                                Estado = Convert.ToBoolean(dr["Estado"])
                            });
                        }
                    }
                }
                catch (Exception ex)
                {
                    Lista = new List<Producto>();

                }
            }
            return Lista;

        }


        // Metodo registrar producto
        public int Registrar(Producto obj, out string Mensaje)
        {
            int idProductogenerado = 0;
            Mensaje = string.Empty; // El valor del mensaje esta vacio


            try
            {
                using (SqlConnection oconexion = new SqlConnection(Conexion.cadena))
                {
                    SqlCommand cmd = new SqlCommand("SP_RegistrarProducto", oconexion); // Crear un nuevo comando SQL para invocar un procedimiento almacenado
                    cmd.Parameters.AddWithValue("Codigo", obj.Codigo); // Asignar valores a los parámetros del comando
                    cmd.Parameters.AddWithValue("Nombre", obj.Nombre);
                    cmd.Parameters.AddWithValue("Descripcion", obj.Descripcion);
                    cmd.Parameters.AddWithValue("@id_Categoria", obj.oCategoria.id_Categoria);
                    cmd.Parameters.AddWithValue("Estado", obj.Estado);

                    // Parametros de salida
                    cmd.Parameters.Add("Resultado", SqlDbType.Int).Direction = ParameterDirection.Output;
                    cmd.Parameters.Add("Mensaje", SqlDbType.VarChar, 500).Direction = ParameterDirection.Output;

                    cmd.CommandType = CommandType.StoredProcedure;

                    oconexion.Open(); 
                    cmd.ExecuteNonQuery(); // Ejecutar el comando

                    idProductogenerado = Convert.ToInt32(cmd.Parameters["Resultado"].Value); // Obtener el valor del parámetro de salida
                    Mensaje = cmd.Parameters["Mensaje"].Value.ToString();

                }

            }
            catch (Exception ex)
            {
                idProductogenerado = 0;
                Mensaje = ex.Message;
            }

            return idProductogenerado;
        }


        // Metodo editar producto
        public bool Editar(Producto obj, out string Mensaje)
        {
            bool respuesta = false;
            Mensaje = string.Empty; // El valor del mensaje esta vacio


            try
            {
                using (SqlConnection oconexion = new SqlConnection(Conexion.cadena))
                {
                    SqlCommand cmd = new SqlCommand("SP_EditarProducto", oconexion); // Crear un nuevo comando SQL para invocar un procedimiento almacenado
                    cmd.Parameters.AddWithValue("id_Producto", obj.id_Producto); // Asignar valores a los parámetros del comando
                    cmd.Parameters.AddWithValue("Codigo", obj.Codigo);
                    cmd.Parameters.AddWithValue("Nombre", obj.Nombre);
                    cmd.Parameters.AddWithValue("Descripcion", obj.Descripcion);
                    cmd.Parameters.AddWithValue("id_Categoria", obj.oCategoria.id_Categoria);
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

            return respuesta; // Devolver la respuesta (true  o false)
        }


        // Metodo eliminar producto
        public bool Eliminar(Producto obj, out string Mensaje)
        {
            bool Respuesta = false;
            Mensaje = string.Empty; // El valor del mensaje esta vacio


            try
            {
                using (SqlConnection oconexion = new SqlConnection(Conexion.cadena))
                {
                    SqlCommand cmd = new SqlCommand("SP_EliminarProducto", oconexion); // Crear un nuevo comando SQL para invocar un procedimiento almacenado
                    cmd.Parameters.AddWithValue("id_Producto", obj.id_Producto); // Asignar valores a los parámetros del comando

                    //Parametros de salida
                    cmd.Parameters.Add("Respuesta", SqlDbType.Int).Direction = ParameterDirection.Output;
                    cmd.Parameters.Add("Mensaje", SqlDbType.VarChar, 500).Direction = ParameterDirection.Output;
                    cmd.CommandType = CommandType.StoredProcedure;


                    oconexion.Open();
                    cmd.ExecuteNonQuery();

                    // Obtener el valor del parámetro de salida
                    Respuesta = Convert.ToBoolean(cmd.Parameters["Respuesta"].Value);
                    Mensaje = cmd.Parameters["Mensaje"].Value.ToString();

                }

            }
            catch (Exception ex)
            {
                Respuesta = false;
                Mensaje = ex.Message;
            }

            return Respuesta; // Devolver la respuesta (true  o false)
        }

    }
}
