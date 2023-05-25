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
    public class CN_Compra
    {
        private CD_Compra objcd_compra = new CD_Compra(); // Creación de una instancia de la clase CD_Compra en la capa de datos

        public int ObtenerCorrelativo()
        {
            return objcd_compra.ObtenerCorrelativo(); // Llamada al método ObtenerCorrelativo() de la instancia objcd_compra para obtener el correlativo de compra
        }

        public bool Registrar(Compra obj,DataTable DetalleCompra, out string Mensaje)
        {
            
            return objcd_compra.Registrar(obj,DetalleCompra,out Mensaje); // Llamada al método Registrar() de la instancia objcd_compra para registrar una compra y obtener el resultado y mensaje correspondiente
        }
    }
}
