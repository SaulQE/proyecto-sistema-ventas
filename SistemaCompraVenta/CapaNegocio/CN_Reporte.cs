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
        private CD_Reporte objdc_reporte = new CD_Reporte();

        public List<ReporteCompra> Compra(string fechainicio, string fechafin, int idproveedor)
        {
            return objdc_reporte.Compra(fechainicio, fechafin, idproveedor);
        }

        public List<ReporteVenta> Venta(string fechainicio, string fechafin)
        {
            return objdc_reporte.Venta(fechainicio, fechafin);
        }

    }
}
