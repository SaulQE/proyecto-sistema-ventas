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
        public List<Permiso> Listar(int id_Usuario) 
        {
            List<Permiso> Lista = new List<Permiso>(); 

            using (SqlConnection oconexion = new SqlConnection(Conexion.cadena))
            {
                try
                {
                    StringBuilder query = new StringBuilder(); 
                    query.AppendLine("select p.id_Rol,p.Nom_Menu from PERMISO p");
                    query.AppendLine("inner join ROL r on r.id_Rol = p.id_Rol");
                    query.AppendLine("inner join USUARIO u on u.id_Rol = r.id_Rol");
                    query.AppendLine("where u.id_Usuario = @id_Usuario"); 



                    SqlCommand cmd = new SqlCommand(query.ToString(), oconexion);
                    cmd.Parameters.AddWithValue("id_Usuario", id_Usuario); 
                    cmd.CommandType = CommandType.Text;

                    oconexion.Open();

                    using (SqlDataReader dr = cmd.ExecuteReader()) 
                    {
                        while (dr.Read())
                        {
                            Lista.Add(new Permiso()
                            {
                                oRol = new Rol() { id_Rol = Convert.ToInt32(dr["id_Rol"]) }, 
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
