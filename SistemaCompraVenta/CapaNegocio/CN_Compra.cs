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

        public Compra ObtenerCompra(string numero)
        {
            Compra oCompra = objcd_compra.ObtenerCompra(numero); // Llamada al método ObtenerCompra() de la instancia objcd_compra para obtener la información de la compra según el número de documento

            // Se verifica si se obtuvo una compra válida
            if (oCompra.id_Compra !=  0)
            {
                // Se llama al método ObtenerDetalleCompra() de la instancia objcd_compra para obtener los detalles de la compra
                List<Detalle_Compra> oDetalleCompra = objcd_compra.ObtenerDetalleCompra(oCompra.id_Compra);

                // Se asigna la lista de detalles de compra a la propiedad oDetalle_Compra del objeto Compra
                oCompra.oDetalle_Compra = oDetalleCompra;
            }
            return oCompra;
        }
    }
}
