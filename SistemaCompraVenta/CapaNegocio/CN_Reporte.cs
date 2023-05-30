using CapaDatos;
using CapaEntidad;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaNegocio
{
    public class CN_Reporte
    {
        // instancia de CD_Reporte
        private CD_Reporte objdc_reporte = new CD_Reporte();

        // creacion de metodo
        public List<ReporteCompra> Compra(string fechainicio, string fechafin, int idproveedor)
        {
            // Utilizando la instancia creada con su metodo Compra y pasando parametros
            return objdc_reporte.Compra(fechainicio, fechafin, idproveedor);
        }

        public List<ReporteVenta> Venta(string fechainicio, string fechafin)
        {
            // Utilizando la instancia creada con su metodo Venta y pasando parametros
            return objdc_reporte.Venta(fechainicio, fechafin);
        }

    }
}
