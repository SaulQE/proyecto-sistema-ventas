using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using CapaDatos;
using CapaEntidad;


namespace CapaNegocio
{
    public class CN_Permiso
    {
        private CD_Permiso objcd_permiso = new CD_Permiso(); // Estamos creando un obj de la clse permiso

        public List<Permiso> Listar(int id_Usuario) // Devuelve una lista de la clase permiso y su evento recibe el id_Usuario
        {
            return objcd_permiso.Listar(id_Usuario); // Aqui pasamos el parametro id_Usuario

        }

    }
}
