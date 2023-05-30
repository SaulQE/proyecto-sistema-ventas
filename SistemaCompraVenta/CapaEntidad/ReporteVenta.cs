using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaEntidad
{
    public class ReporteVenta
    {
        public string F_Registro { get; set; }
        public string Tipo_Documento { get; set; }
        public string Nro_Documento { get; set; }
        public string Monto_Total { get; set; }
        public string UsuarioRegistro { get; set; }
        public string Documento_Cliente { get; set; }
        public string Nom_Cliente { get; set; }
        public string CodigoProducto { get; set; }
        public string NombreProducto { get; set; }
        public string Categoria { get; set; }
        public string Precio_Venta { get; set; }
        public string Cantidad { get; set; }
        public string SubTotal { get; set; }
    }
}
