﻿using CapaEntidad;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaDatos
{
    public class CD_Reporte
    {
        public List<ReporteCompra> Compra(string fechainicio, string fechafin, int idproveedor)
        {
            List<ReporteCompra> Lista = new List<ReporteCompra>();

            using (SqlConnection oconexion = new SqlConnection(Conexion.cadena))
            {
                try
                {
                    StringBuilder query = new StringBuilder();

                    SqlCommand cmd = new SqlCommand("SP_ReporteCompras",oconexion);
                    cmd.Parameters.AddWithValue("fechaInicio",fechainicio);
                    cmd.Parameters.AddWithValue("fechaFin",fechafin);
                    cmd.Parameters.AddWithValue("id_Proveedor",idproveedor);
                    cmd.CommandType = CommandType.StoredProcedure;

                    oconexion.Open();

                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            Lista.Add(new ReporteCompra()
                            {
                                F_Registro = dr["F_Registro"].ToString(),
                                Tipo_Documento = dr["Tipo_Documento"].ToString(),
                                Nro_Documento = dr["Nro_Documento"].ToString(),
                                Monto_Total = dr["Monto_Total"].ToString(),
                                UsuarioRegistro = dr["UsuarioRegistro"].ToString(),
                                DocumentoProveedor = dr["DocumentoProveedor"].ToString(),
                                Razon_Social = dr["Razon_Social"].ToString(),
                                CodigoProducto = dr["CodigoProducto"].ToString(),
                                NombreProducto = dr["NombreProducto"].ToString(),
                                Categoria = dr["Categoria"].ToString(),
                                Precio_Compra = dr["Precio_Compra"].ToString(),
                                Precio_Venta = dr["Precio_Venta"].ToString(),
                                Cantidad = dr["Cantidad"].ToString(),
                                SubTotal = dr["SubTotal"].ToString(),

                            });
                        }
                    }
                }
                catch (Exception ex)
                {
                    Lista = new List<ReporteCompra>();

                }
            }
            return Lista;

        }


        public List<ReporteVenta> Venta(string fechainicio, string fechafin)
        {
            List<ReporteVenta> Lista = new List<ReporteVenta>();

            using (SqlConnection oconexion = new SqlConnection(Conexion.cadena))
            {
                try
                {
                    StringBuilder query = new StringBuilder();

                    SqlCommand cmd = new SqlCommand("SP_ReporteVentas", oconexion);
                    cmd.Parameters.AddWithValue("fechaInicio", fechainicio);
                    cmd.Parameters.AddWithValue("fechaFin", fechafin);
                    cmd.CommandType = CommandType.StoredProcedure;

                    oconexion.Open();

                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            Lista.Add(new ReporteVenta()
                            {
                                F_Registro = dr["F_Registro"].ToString(),
                                Tipo_Documento = dr["Tipo_Documento"].ToString(),
                                Nro_Documento = dr["Nro_Documento"].ToString(),
                                Monto_Total = dr["Monto_Total"].ToString(),
                                UsuarioRegistro = dr["UsuarioRegistro"].ToString(),
                                Documento_Cliente = dr["Documento_Cliente"].ToString(),
                                Nom_Cliente = dr["Nom_Cliente"].ToString(),
                                CodigoProducto = dr["CodigoProducto"].ToString(),
                                NombreProducto = dr["NombreProducto"].ToString(),
                                Categoria = dr["Categoria"].ToString(),
                                Precio_Venta = dr["Precio_Venta"].ToString(),
                                Cantidad = dr["Cantidad"].ToString(),
                                SubTotal = dr["SubTotal"].ToString(),
                            });
                        }
                    }
                }
                catch (Exception ex)
                {
                    Lista = new List<ReporteVenta>();

                }
            }
            return Lista;

        }

    }
}
