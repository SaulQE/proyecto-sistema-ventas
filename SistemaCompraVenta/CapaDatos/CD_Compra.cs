using CapaEntidad;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
using System.Xml.Linq;

namespace CapaDatos
{
    public class CD_Compra
    {
        public int ObtenerCorrelativo()
        {
            int idcorrelativo = 0;

            using (SqlConnection oconexion = new SqlConnection(Conexion.cadena))
            {
                try
                {
                    StringBuilder query = new StringBuilder();
                    query.AppendLine("select count(*) +1 from COMPRA");

                    SqlCommand cmd = new SqlCommand(query.ToString(), oconexion); 
                    cmd.CommandType = CommandType.Text;                           

                    oconexion.Open(); 

                    idcorrelativo = Convert.ToInt32(cmd.ExecuteScalar()); 

                }
                catch (Exception ex)
                {
                    idcorrelativo = 0; 
                }
            }
            return idcorrelativo;
        }

        public bool Registrar(Compra obj, DataTable DetalleCompra, out string Mensaje)
        {
            bool Respuesta = false; 
            Mensaje = string.Empty; 

            using (SqlConnection oconexion = new SqlConnection(Conexion.cadena))
            {
                try
                {
                    SqlCommand cmd = new SqlCommand("SP_RegistrarCompra", oconexion);   
                    cmd.Parameters.AddWithValue("id_Usuario", obj.oUsuario.id_Usuario); 
                    cmd.Parameters.AddWithValue("id_Proveedor", obj.oProveedor.id_Proveedor); 
                    cmd.Parameters.AddWithValue("Tipo_Documento", obj.Tipo_Documento); 
                    cmd.Parameters.AddWithValue("Nro_Documento", obj.Nro_Documento); 
                    cmd.Parameters.AddWithValue("Monto_Total", obj.Monto_total); 
                    cmd.Parameters.AddWithValue("Detalle_Compra", DetalleCompra);
                    cmd.Parameters.Add("Resultado", SqlDbType.Bit).Direction = ParameterDirection.Output;        
                    cmd.Parameters.Add("Mensaje", SqlDbType.VarChar, 500).Direction = ParameterDirection.Output; 

                    cmd.CommandType = CommandType.StoredProcedure;

                    oconexion.Open();

                    cmd.ExecuteNonQuery(); 

                    Respuesta = Convert.ToBoolean(cmd.Parameters["Resultado"].Value); 
                    Mensaje = cmd.Parameters["Mensaje"].Value.ToString();
                }
                catch (Exception ex)
                {
                    Respuesta = false;    
                    Mensaje = ex.Message; 
                }
            }

            return Respuesta; 
        }

        public Compra ObtenerCompra(string numero)
        {
            // Se crea una instancia de la clase Compra
            Compra obj = new Compra();
            using (SqlConnection oconexion = new SqlConnection(Conexion.cadena))
            {
                try
                {
                    StringBuilder query = new StringBuilder();

                    // Se agrega la consulta SQL a la cadena de texto usando AppendLine para cada línea.
                    query.AppendLine("select c.id_Compra,");
                    query.AppendLine("u.Nom_Completo,");
                    query.AppendLine("pr.Documento,pr.Razon_Social,");
                    query.AppendLine("c.Tipo_Documento, c.Nro_Documento, c.Monto_Total, convert(char(10),c.F_Registro,103)[F_Registro]");
                    query.AppendLine("from COMPRA c");
                    query.AppendLine("INNER JOIN USUARIO u on u.id_Usuario = c.id_Usuario");
                    query.AppendLine("INNER JOIN PROVEEDOR pr on pr.id_Proveedor = c.id_Proveedor");
                    query.AppendLine("where c.Nro_Documento = @numero");

                    SqlCommand cmd = new SqlCommand(query.ToString(), oconexion);
                    cmd.Parameters.AddWithValue("@numero", numero); 
                    cmd.CommandType = CommandType.Text;

                    oconexion.Open();

                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            obj = new Compra()
                            {
                                id_Compra = Convert.ToInt32(dr["id_Compra"]),
                                oUsuario = new Usuario() { Nom_Completo = dr["Nom_Completo"].ToString() },
                                oProveedor = new Proveedor() { Documento = dr["Documento"].ToString(), Razon_Social = dr["Razon_Social"].ToString() },
                                Tipo_Documento = dr["Tipo_Documento"].ToString(),
                                Nro_Documento = dr["Nro_Documento"].ToString(),
                                Monto_total = Convert.ToDecimal(dr["Monto_Total"].ToString()),
                                F_Registro = dr["F_Registro"].ToString()
                            };
                        }
                    }
                }
                catch (Exception ex)
                {
                    obj = new Compra();
                }
            }
            return obj;
        }

        public List<Detalle_Compra> ObtenerDetalleCompra (int idcompra)
        {
            List<Detalle_Compra> oLista = new List<Detalle_Compra>();
            try
            {
                using (SqlConnection conexion = new SqlConnection(Conexion.cadena))
                {
                    conexion.Open();
                    StringBuilder query = new StringBuilder();

                    query.AppendLine("select p.Nombre,dc.Precio_Compra,dc.Cantidad,dc.Monto_Total from DETALLE_COMPRA dc");
                    query.AppendLine("INNER JOIN PRODUCTO p on p.id_Producto = dc.id_Producto");
                    query.AppendLine("where dc.id_Compra = @idcompra");

                    SqlCommand cmd = new SqlCommand(query.ToString(), conexion);

                    cmd.Parameters.AddWithValue("@idcompra", idcompra);
                    cmd.CommandType = System.Data.CommandType.Text;

                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            oLista.Add(new Detalle_Compra()
                            {
                                oProducto = new Producto() { Nombre = dr["Nombre"].ToString() },
                                Precio_Compra = Convert.ToDecimal(dr["Precio_Compra"].ToString()),
                                Cantidad = Convert.ToInt32(dr["Cantidad"].ToString()),
                                Monto_Total = Convert.ToDecimal(dr["Monto_Total"].ToString())
                            });
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                oLista = new List<Detalle_Compra>();
            }
            return oLista;
        }

    }
}
