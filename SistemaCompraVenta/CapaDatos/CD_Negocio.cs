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
    public class CD_Negocio
    {
        public Negocio ObtenerDatos(){

            Negocio obj = new Negocio();

            try {

                using (SqlConnection conexion = new SqlConnection(Conexion.cadena)) {
                    conexion.Open();

                    string query = "select id_Negocio,Nombre,RUC,Direccion from NEGOCIO where id_Negocio = 1";
                    SqlCommand cmd = new SqlCommand(query, conexion);
                    cmd.CommandType = CommandType.Text; // Comando de tipo texto

                    using (SqlDataReader dr = cmd.ExecuteReader()) { //dar lectura 
                        while (dr.Read()) {  // Mientras lee, se va a guardar mi obj
                            obj = new Negocio() 
                            { 
                                id_Negocio = int.Parse(dr["id_Negocio"].ToString()),
                                Nombre = dr["Nombre"].ToString(),
                                RUC = dr["RUC"].ToString(),
                                Direccion = dr["Direccion"].ToString()
                            };
                        }
                    }
                }

            }catch{ 
                obj = new Negocio();
            }

            return obj;
        }

        public bool GuardarDatos(Negocio objeto, out string mensaje) {

            mensaje = string.Empty;  //Vacio
            bool respuesta = true;   //Guardar respuesta y por defecto true

            try
            {

                using (SqlConnection conexion = new SqlConnection(Conexion.cadena))
                {
                    conexion.Open();

                    StringBuilder query = new StringBuilder(); //El StringBuilder nos permite usar saltos de linea
                    query.AppendLine("update NEGOCIO set Nombre = @nombre,");
                    query.AppendLine("RUC = @ruc,");
                    query.AppendLine("Direccion = @direccion");
                    query.AppendLine("where id_Negocio = 1;");

                    SqlCommand cmd = new SqlCommand(query.ToString(), conexion);

                    cmd.Parameters.AddWithValue("@nombre", objeto.Nombre);
                    cmd.Parameters.AddWithValue("@ruc", objeto.RUC);
                    cmd.Parameters.AddWithValue("@direccion", objeto.Direccion);
                    cmd.CommandType = CommandType.Text; // Comando de tipo texto

                    if(cmd.ExecuteNonQuery() < 1) { // Si el numero de filas afectadas es menor a 1 (es porque no actualizo)
                        mensaje = "No se pudo guardar los datos";
                        respuesta = false;
                    }

                }
            }catch (Exception ex)
            { 
                mensaje = ex.Message;
                respuesta = false;
            }

            return respuesta;
        }

        public byte[] ObtenerLogo(out bool obtenido) { 
            obtenido = true;
            byte[] LogoBytes = new byte[0];

            try
            {

                using (SqlConnection conexion = new SqlConnection(Conexion.cadena))
                {
                    conexion.Open();
                    string query = "select Logo from NEGOCIO where id_Negocio = 1";

                    SqlCommand cmd = new SqlCommand(query, conexion);
                    cmd.CommandType = CommandType.Text; // Comando de tipo texto

                    using (SqlDataReader dr = cmd.ExecuteReader()) //dar lectura 
                    { 
                        while (dr.Read())  // Mientras lee, se va a guardar mi obj
                        {
                            LogoBytes = (byte[])dr["Logo"]; // estoy casteando
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                obtenido = false;
                LogoBytes = new byte[0];
            }

            return LogoBytes;
        }

        public bool ActualizarLogo(byte[] image, out string mensaje) {
            mensaje = string.Empty;  //Vacio
            bool respuesta = true;   //Guardar respuesta y por defecto true

            try
            {

                using (SqlConnection conexion = new SqlConnection(Conexion.cadena))
                {
                    conexion.Open();

                    StringBuilder query = new StringBuilder(); //El StringBuilder nos permite usar saltos de linea
                    query.AppendLine("update NEGOCIO set Logo = @imagen");
                    query.AppendLine("where id_Negocio = 1;");

                    SqlCommand cmd = new SqlCommand(query.ToString(), conexion);
                    cmd.Parameters.AddWithValue("@imagen", image);
                    cmd.CommandType = CommandType.Text; // Comando de tipo texto

                    if (cmd.ExecuteNonQuery() < 1) // Si el numero de filas afectadas es menor a 1 (es porque no actualizo)
                    { 
                        mensaje = "No se pudo actualizar el logo";
                        respuesta = false;
                    }

                }
            }
            catch (Exception ex)
            {
                mensaje = ex.Message;
                respuesta = false;
            }

            return respuesta;

        }

    }
}
