using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using CapaDatos;
using CapaEntidad;

namespace CapaNegocio
{
    public class CN_Categoria
    {
        private CD_Categoria objCapaDato = new CD_Categoria();

        public List<Categoria> Listar()
        {
            return objCapaDato.Listar();
        }


        public string Registrar(Categoria obj, out string Mensaje)
        {

            Mensaje = string.Empty;

            if(string.IsNullOrEmpty(obj.nombre_categoria) || string.IsNullOrWhiteSpace(obj.nombre_categoria))
            {
                Mensaje = "No puede estar vacio el campo del nombre de la categoria";
            }
            if (string.IsNullOrEmpty(obj.estado) || string.IsNullOrWhiteSpace(obj.estado))
            {
                Mensaje = "Seleccione el estado";
            }
            if (string.IsNullOrEmpty(Mensaje))
            {
                return objCapaDato.Registrar(obj, out Mensaje);
            }
            else
            {
                return string.Empty;
            }


        }

        public bool Editar(Categoria obj, out string Mensaje)
        {
            Mensaje = string.Empty;

            if (string.IsNullOrEmpty(obj.nombre_categoria) || string.IsNullOrWhiteSpace(obj.nombre_categoria))
            {
                Mensaje = "No puede estar vacio el campo del nombre de la categoria";
            }

            if (string.IsNullOrEmpty(obj.estado) || string.IsNullOrWhiteSpace(obj.estado))
            {
                Mensaje = "Seleccione el estado";
            }

            if (string.IsNullOrEmpty(Mensaje))
            {
                return objCapaDato.Editar(obj, out Mensaje);
            }
            else
            {
                return false;
            }

        }

        public bool Eliminar(string id)
        {
            return objCapaDato.Eliminar(id);
        }

    }
}
