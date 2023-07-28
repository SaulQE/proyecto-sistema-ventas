using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Data.SqlClient;
using System.Data;
using CapaEntidad;

namespace CapaDatos
{
    public class CD_Rol
    {
        public List<Rol> Listar() 
        {
            List<Rol> Lista = new List<Rol>(); 

            using (SqlConnection oconexion = new SqlConnection(Conexion.cadena))
            {
                try
                {
                    StringBuilder query = new StringBuilder();
                    query.AppendLine("select id_Rol,Descripcion from ROL");


                    SqlCommand cmd = new SqlCommand(query.ToString(), oconexion);
                    cmd.CommandType = CommandType.Text;

                    oconexion.Open();

                    using (SqlDataReader dr = cmd.ExecuteReader()) 
                    {
                        while (dr.Read()) 
                        {
                            Lista.Add(new Rol()
                            {
                                id_Rol = Convert.ToInt32(dr["id_Rol"]),
                                Descripcion = dr["Descripcion"].ToString(),
                            });
                        }

                    }

                }
                catch (Exception ex)
                {
                    Lista = new List<Rol>();

                }
            }
            return Lista;

        }

    }
}
