using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CapaEntidad;
using System.Reflection;

namespace CapaDatos
{
    public class CD_Venta
    {
        public int ObtenerCorrelativo()
        {
            int idcorrelativo = 0;

            using (SqlConnection oconexion = new SqlConnection(Conexion.cadena))
            {
                try
                {
                    StringBuilder query = new StringBuilder();
                    query.AppendLine("select count(*) +1 from VENTA"); 

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

        public bool RestarStock(int idproducto, int cantidad)
        {
            bool respuesta = true;

            using (SqlConnection oconoxion = new SqlConnection(Conexion.cadena))
            {
                try
                {
                    StringBuilder query = new StringBuilder();
                    query.AppendLine("update PRODUCTO set Stock = Stock - @cantidad where id_Producto = @idproducto");

                    SqlCommand cmd = new SqlCommand(query.ToString(), oconoxion);

                    cmd.Parameters.AddWithValue("@cantidad", cantidad);
                    cmd.Parameters.AddWithValue("@idproducto", idproducto);
                    cmd.CommandType = CommandType.Text;

                    oconoxion.Open();

                    respuesta = cmd.ExecuteNonQuery() > 0 ? true : false;

                }catch (Exception ex)
                {
                    respuesta = false;
                }
            }
            return respuesta;
        }

        public bool SumarStock(int idproducto, int cantidad)
        {
            bool respuesta = true;

            using (SqlConnection oconoxion = new SqlConnection(Conexion.cadena))
            {
                try
                {
                    StringBuilder query = new StringBuilder();
                    query.AppendLine("update PRODUCTO set Stock = Stock + @cantidad where id_Producto = @idproducto");

                    SqlCommand cmd = new SqlCommand(query.ToString(), oconoxion);

                    cmd.Parameters.AddWithValue("@cantidad", cantidad);
                    cmd.Parameters.AddWithValue("@idproducto", idproducto);
                    cmd.CommandType = CommandType.Text;

                    oconoxion.Open();

                    respuesta = cmd.ExecuteNonQuery() > 0 ? true : false;

                }
                catch (Exception ex)
                {
                    respuesta = false;
                }
            }
            return respuesta;
        }

        public bool Registrar(Venta obj, DataTable DetalleVenta, out string Mensaje)
        {
            bool Respuesta = false; 
            Mensaje = string.Empty; 

            try
            {
                using (SqlConnection oconexion = new SqlConnection(Conexion.cadena))
                {
                    SqlCommand cmd = new SqlCommand("SP_RegistrarVenta", oconexion);   
                    cmd.Parameters.AddWithValue("id_Usuario", obj.oUsuario.id_Usuario);
                    cmd.Parameters.AddWithValue("Tipo_Documento", obj.Tipo_Documento);
                    cmd.Parameters.AddWithValue("Nro_Documento", obj.Nro_Documento);
                    cmd.Parameters.AddWithValue("Documento_Cliente", obj.Documento_Cliente);
                    cmd.Parameters.AddWithValue("Nom_Cliente", obj.Nom_Cliente);
                    cmd.Parameters.AddWithValue("Monto_Pago", obj.Monto_Pago);
                    cmd.Parameters.AddWithValue("Monto_Cambio", obj.Monto_Cambio);
                    cmd.Parameters.AddWithValue("Monto_Total", obj.Monto_Total);
                    cmd.Parameters.AddWithValue("Detalle_Venta", DetalleVenta); 
                    cmd.Parameters.Add("Resultado", SqlDbType.Bit).Direction = ParameterDirection.Output;       
                    cmd.Parameters.Add("Mensaje", SqlDbType.VarChar, 500).Direction = ParameterDirection.Output;

                    cmd.CommandType = CommandType.StoredProcedure; 

                    oconexion.Open();

                    cmd.ExecuteNonQuery();

                    Respuesta = Convert.ToBoolean(cmd.Parameters["Resultado"].Value); 
                    Mensaje = cmd.Parameters["Mensaje"].Value.ToString();
                }

            } catch (Exception ex)
            {
                Respuesta = false;    
                Mensaje = ex.Message; 
            }
            

            return Respuesta; 
        }

        public Venta ObtenerVenta(string numero)
        {
            Venta obj = new Venta();

            using (SqlConnection conexion = new SqlConnection(Conexion.cadena))
            {
                try
                {
                    conexion.Open();
                    StringBuilder query = new StringBuilder();

                    query.AppendLine("select v.id_Venta,u.Nom_Completo,");
                    query.AppendLine("v.Documento_Cliente,v.Nom_Cliente,");
                    query.AppendLine("v.Tipo_Documento,v.Nro_Documento,");
                    query.AppendLine("v.Monto_Pago,v.Monto_Cambio,v.Monto_Total,");
                    query.AppendLine("convert(char(10), v.F_Registro, 103)[F_Registro]");
                    query.AppendLine("from VENTA v");
                    query.AppendLine("INNER JOIN USUARIO u on u.id_Usuario = v.id_Usuario");
                    query.AppendLine("where v.Nro_Documento = @numero");

                    SqlCommand cmd = new SqlCommand(query.ToString(), conexion);

                    cmd.Parameters.AddWithValue("@numero", numero);

                    cmd.CommandType = CommandType.Text;

                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            obj = new Venta()
                            {
                                id_Venta = int.Parse(dr["id_Venta"].ToString()),
                                oUsuario = new Usuario() { Nom_Completo = dr["Nom_Completo"].ToString() },
                                Documento_Cliente = dr["Documento_Cliente"].ToString(),
                                Nom_Cliente = dr["Nom_Cliente"].ToString(),
                                Tipo_Documento = dr["Tipo_Documento"].ToString(),
                                Nro_Documento = dr["Nro_Documento"].ToString(),
                                Monto_Pago = Convert.ToDecimal(dr["Monto_Pago"].ToString()),
                                Monto_Cambio = Convert.ToDecimal(dr["Monto_Cambio"].ToString()),
                                Monto_Total = Convert.ToDecimal(dr["Monto_Total"].ToString()),
                                F_Registro = dr["F_Registro"].ToString()
                            };
                        }
                    }
                }
                catch
                {
                    obj = new Venta();
                }

            }
            return obj;
        }

        public List<Detalle_Venta> ObtenerDetalleVenta(int idventa)
        {
            List<Detalle_Venta> oLista = new List<Detalle_Venta>();
            using (SqlConnection conexion = new SqlConnection(Conexion.cadena))
            {
                try
                {
                    conexion.Open();
                    StringBuilder query = new StringBuilder();

                    query.AppendLine("select p.Nombre,dv.Precio_Venta,dv.Cantidad,dv.SubTotal from DETALLE_VENTA dv");
                    query.AppendLine("INNER JOIN PRODUCTO p on p.id_Producto = dv.id_Producto");
                    query.AppendLine("where dv.id_Venta = @idventa");

                    SqlCommand cmd = new SqlCommand(query.ToString(), conexion);

                    cmd.Parameters.AddWithValue("@idventa", idventa);
                    cmd.CommandType = System.Data.CommandType.Text;

                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            oLista.Add(new Detalle_Venta()
                            {
                                oProducto = new Producto() { Nombre = dr["Nombre"].ToString() },
                                Precio_Venta = Convert.ToDecimal(dr["Precio_Venta"].ToString()),
                                Cantidad = Convert.ToInt32(dr["Cantidad"].ToString()),
                                SubTotal = Convert.ToDecimal(dr["SubTotal"].ToString())
                            });
                        }
                    }

                }
                catch
                {
                    oLista = new List<Detalle_Venta>();
                }
            }
                return oLista;
        }


    }
}
