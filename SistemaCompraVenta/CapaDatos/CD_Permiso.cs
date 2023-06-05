using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Data.SqlClient;
using System.Data;
using CapaEntidad;
using System.Reflection;

namespace CapaDatos
{
    public class CD_Permiso
    {
        public List<Permiso> Listar(int id_Usuario) //listar, recibe el id de tipo entero y me retornar los permisos de un respectivo usuario
        {
            List<Permiso> Lista = new List<Permiso>(); //aqui esta creando una lista de tipo list permiso

            using (SqlConnection oconexion = new SqlConnection(Conexion.cadena))
            {
                try
                {
                    StringBuilder query = new StringBuilder(); // me ayuda hacer saltos de linea
                    query.AppendLine("select p.id_Rol,p.Nom_Menu from PERMISO p");
                    query.AppendLine("inner join ROL r on r.id_Rol = p.id_Rol");
                    query.AppendLine("inner join USUARIO u on u.id_Rol = r.id_Rol");
                    query.AppendLine("where u.id_Usuario = @id_Usuario"); // Aqui cambia de valor por el id_Usuario que se recibe



                    SqlCommand cmd = new SqlCommand(query.ToString(), oconexion);
                    cmd.Parameters.AddWithValue("id_Usuario", id_Usuario); // Aqui se remplaza el @id_usuario por el valor del id_Usuario
                    cmd.CommandType = CommandType.Text;

                    oconexion.Open();

                    using (SqlDataReader dr = cmd.ExecuteReader()) // Para dar lectura 
                    {
                        while (dr.Read())
                        {
                            Lista.Add(new Permiso()
                            {
                                oRol = new Rol() { id_Rol = Convert.ToInt32(dr["id_Rol"]) }, //Le estamos pasando el mismo tipo de la clase rol y su propiedad le estamos dando el valor que obtenemos
                                Nom_Menu = dr["Nom_Menu"].ToString(),
                            });
                        }

                    }

                }
                catch (Exception ex)
                {
                    Lista = new List<Permiso>();

                }
            }
            return Lista;

        }
    }
}
