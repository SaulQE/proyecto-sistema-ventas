using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaEntidad
{
    public class Permiso
    {
        public int id_Permiso { get; set; }
        public Rol oRol { get; set; }
        public string Nom_Menu { get; set; }
        public string F_Registro { get; set; }

    }
}
