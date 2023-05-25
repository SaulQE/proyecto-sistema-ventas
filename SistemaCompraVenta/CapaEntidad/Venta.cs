using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaEntidad
{
    public class Venta
    {
        public int id_Venta { get; set; }
        public Usuario oUsuario { get; set; }
        public string Tipo_Documento { get; set; }
        public string Nro_Documento { get; set; }
        public string Documento_Cliente { get; set; }
        public string Nom_Cliente { get; set; }
        public decimal Monto_Pago { get; set; }
        public decimal Monto_Cambio { get; set; }
        public decimal Monto_Total { get; set; }
        public List<Detalle_Venta> oDetalle_Venta { get; set; }
        public string F_Registro { get; set; }

    }
}
