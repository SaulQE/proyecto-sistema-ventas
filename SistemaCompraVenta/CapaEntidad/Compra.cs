using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaEntidad
{
    public class Compra
    {
        public int id_Compra { get; set; }
        public Usuario oUsuario { get; set; }
        public Proveedor oProveedor { get; set; }
        public string Tipo_Documento { get; set; }
        public string Nro_Documento { get; set; }
        public decimal Monto_total { get; set; }
        public List<Detalle_Compra> oDetalle_Compra { get; set; }
        public string F_Registro { get; set; }
    }
}
