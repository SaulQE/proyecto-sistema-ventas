using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using CapaDatos;
using CapaEntidad;

namespace CapaNegocio
{
    public class CN_Rol
    {
        private CD_Rol objcd_rol = new CD_Rol(); // Estamos creando un obj de la clase rol

        public List<Rol> Listar() // Devuelve una lista de la clase Rol
        {
            return objcd_rol.Listar(); // Se devuelve el metodo listar, este metodo listar hacer llamdado a listar de CD_Rol

        }

    }
}
