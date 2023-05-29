using CapaDatos;
using CapaEntidad;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaNegocio
{
    public class CN_Venta
    {
        private CD_Venta objcd_venta = new CD_Venta(); // Creación de una instancia de la clase CD_Venta en la capa de datos

        public bool RestarStock(int idproducto, int cantidad)
        {
            return objcd_venta.RestarStock(idproducto, cantidad);
        }

        public bool SumarStock(int idproducto, int cantidad)
        {
            return objcd_venta.SumarStock(idproducto, cantidad);
        }

        public int ObtenerCorrelativo()
        {
            return objcd_venta.ObtenerCorrelativo(); // Llamada al método ObtenerCorrelativo() de la instancia objcd_compra para obtener el correlativo de compra
        }

        public bool Registrar(Venta obj, DataTable DetalleVenta, out string Mensaje)
        {

            return objcd_venta.Registrar(obj, DetalleVenta, out Mensaje); // Llamada al método Registrar() de la instancia objcd_compra para registrar una compra y obtener el resultado y mensaje correspondiente
        }

    }
}
