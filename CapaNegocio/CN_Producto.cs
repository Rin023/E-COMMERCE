using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using CapaDatos;
using CapaEntidad;

namespace CapaNegocio
{
    public class CN_Producto
    {
        private CD_Producto objCapaDato = new CD_Producto();

        public List<Producto> Listar()
        {
            return objCapaDato.Listar();
        }

        public string Registrar(Producto obj, out string Mensaje)
        {
            Mensaje = string.Empty;

            if (string.IsNullOrEmpty(obj.oCategoria.id_categoria) || string.IsNullOrWhiteSpace(obj.oCategoria.id_categoria))
            {
                Mensaje = "Debe agregar un categoria";
            }
            else if (string.IsNullOrEmpty(obj.oCatalogo.id_catalogo) || string.IsNullOrWhiteSpace(obj.oCatalogo.id_catalogo))
            {
                Mensaje = "Debe asignar el catalogo";
            }
            else if (string.IsNullOrEmpty(obj.nombre) || string.IsNullOrWhiteSpace(obj.nombre))
            {
                Mensaje = "El nombre no pude estar vacio";
            }
            else if (string.IsNullOrEmpty(obj.descripcion) || string.IsNullOrWhiteSpace(obj.descripcion))
            {
                Mensaje = "agregue la descripcion";
            }
            else if (obj.stock == 0)
            {
                Mensaje = "El stock no puede estar vacio";
            }
            else if (obj.stock < 0)
            {
                Mensaje = "Valor de stock no valido";
            }
            else if (obj.precio == 0)
            {
                Mensaje = "El producto debe tener un precio";
            }
            else if (obj.precio < obj.costo || obj.precio == obj.costo)
            {
                Mensaje = "El precio debe exceceder el costo";
            }
            else if (obj.costo == 0)
            {
                Mensaje = "Defina el costo";
            }
            else if (string.IsNullOrEmpty(obj.estado) || string.IsNullOrWhiteSpace(obj.estado))
            {
                Mensaje = "Asigne un estado al producto";
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

      

        public bool Editar(Producto obj,out string Mensaje)
        {
            Mensaje = string.Empty;


            if (string.IsNullOrEmpty(obj.oCategoria.id_categoria) || string.IsNullOrWhiteSpace(obj.oCategoria.id_categoria))
            {
                Mensaje = "Debe agregar una categoria";
            }
            else if (string.IsNullOrEmpty(obj.oCatalogo.id_catalogo) || string.IsNullOrWhiteSpace(obj.oCatalogo.id_catalogo))
            {
                Mensaje = "Debe asignar el catalogo";
            }
            else if (string.IsNullOrEmpty(obj.nombre) || string.IsNullOrWhiteSpace(obj.nombre))
            {
                Mensaje = "El nombre no pude estar vacio";
            }
            else if (string.IsNullOrEmpty(obj.descripcion) || string.IsNullOrWhiteSpace(obj.descripcion))
            {
                Mensaje = "agregue la descripcion";
            }
            else if (obj.stock == 0)
            {
                Mensaje = "El stock no puede ser vacio";
            }
            else if (obj.stock < 0)
            {
                Mensaje = "Valor de stock no valido";
            }
            else if (obj.precio == 0)
            {
                Mensaje = "El producto debe tener un precio";
            }
            else if (obj.precio < obj.costo || obj.precio == obj.costo)
            {
                Mensaje = "El precio debe exceceder el costo";
            }
            else if (obj.costo == 0)
            {
                Mensaje = "Defina el costo";
            }
            else if (string.IsNullOrEmpty(obj.estado) || string.IsNullOrWhiteSpace(obj.estado))
            {
                Mensaje = "Asigne un estado al producto";
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

        public bool GuardarDatosImagen(Producto obj, out string Mensaje)
        {
            return objCapaDato.GuardarDatosImagen(obj, out Mensaje);
        }
        public bool Eliminar(string id)
        {
            return objCapaDato.Eliminar(id);
        }

    }
}
