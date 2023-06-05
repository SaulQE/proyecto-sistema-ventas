﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using CapaDatos;
using CapaEntidad;

namespace CapaNegocio
{
    public class CN_Usuario
    {
        private CD_Usuario objcd_usuario = new CD_Usuario();

        // Método para listar los usuarios
        public List<Usuario> Listar()
        {
            return objcd_usuario.Listar();
        }

        // Método para registrar un nuevo usuario
        public int Registrar(Usuario obj, out string Mensaje)
        {
            Mensaje = string.Empty;

            // Verificar si faltan datos requeridos
            if (obj.Documento == "")
            {
                Mensaje += "Es necesario el documento del usuario\n";
            }

            if (obj.Nom_Completo == "")
            {
                Mensaje += "Es necesario el nombre completo del usuario\n";
            }
            
            if (obj.Clave == "")
            {
                Mensaje += "Es necesario la clave del usuario\n";
            }

            // Si hay mensajes de error, retornar 0
            if (Mensaje != string.Empty)
            {
                return 0;
            }
            else
            {
                // Llamar al método de la capa de datos para registrar el usuario
                return objcd_usuario.Registrar(obj, out Mensaje);
            }

        }

        // Método para editar un usuario existente
        public bool Editar(Usuario obj, out string Mensaje)
        {
            Mensaje = string.Empty;

            if (obj.Documento == "")
            {
                Mensaje += "Es necesario el documento del usuario\n";
            }

            if (obj.Nom_Completo == "")
            {
                Mensaje += "Es necesario el nombre completo del usuario\n";
            }

            if (obj.Clave == "")
            {
                Mensaje += "Es necesario la clave del usuario\n";
            }


            // Si hay mensajes de error, retornar false
            if (Mensaje != string.Empty)
            {
                return false;
            }

            else
            {
                // Llamar al método de la capa de datos para editar el usuario
                return objcd_usuario.Editar(obj, out Mensaje);
            }

        }

        // Método para eliminar un usuario
        public bool Eliminar(Usuario obj, out string Mensaje)
        {
            // Llamar al método de la capa de datos para eliminar el usuario
            return objcd_usuario.Eliminar(obj, out Mensaje);
        }

    }
}
