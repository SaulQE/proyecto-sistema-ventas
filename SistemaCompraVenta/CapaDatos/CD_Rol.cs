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
        public List<Rol> Listar() // Se esta enlistando
        {
            List<Rol> Lista = new List<Rol>(); //aqui esta creando una lista de tipo list Rol

            using (SqlConnection oconexion = new SqlConnection(Conexion.cadena))
            {
                try
                {
                    StringBuilder query = new StringBuilder();
                    query.AppendLine("select id_Rol,Descripcion from ROL");


                    SqlCommand cmd = new SqlCommand(query.ToString(), oconexion); //Se ejecuta con SqlCommand, osea le estamos diciendo que acción va ejecutar
                    cmd.CommandType = CommandType.Text; //tipo texto

                    oconexion.Open();

                    using (SqlDataReader dr = cmd.ExecuteReader()) // Para dar lectura de la ejecución
                    {
                        while (dr.Read()) // Procede a leer toda la lista de la ejecución del select
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
