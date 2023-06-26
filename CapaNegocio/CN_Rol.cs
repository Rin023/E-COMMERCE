using CapaDatos;
using CapaEntidad;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaNegocio
{
    public class CN_Rol
    {
        private CD_Rol objCapaDato = new CD_Rol();
        public List<Rol> Listar()
        {
            return objCapaDato.Listar();
        }

        public List<RolPorModulo> ListarRolPorModulo()
        {
            return objCapaDato.ListarRolPorModulo();
        }

        public bool RegistrarRol(string nombreRol, int accesoModuloUsuarios, int accesoModuloVentas, int accesoModuloCategorias, int accesoModuloProductos, out string Mensaje)
        {

            Mensaje = string.Empty;

            if (string.IsNullOrEmpty(nombreRol))
            {
                Mensaje = "No puede estar vacio el campo de texto";
            }
            if (accesoModuloUsuarios != 1 && accesoModuloUsuarios != 0)
            {
                Mensaje = "Opss ocurrio un error al elegir el modulo usuario";
            }
            if (accesoModuloVentas != 1 && accesoModuloVentas != 0)
            {
                Mensaje = "Opss ocurrio un error al elegir el modulo venta";
            }

            if (accesoModuloCategorias != 1 && accesoModuloCategorias != 0)
            {
                Mensaje = "Opss ocurrio un error al elegir el modulo categoria";
            }

            if (accesoModuloProductos != 1 && accesoModuloProductos != 0)
            {
                Mensaje = "Opss ocurrio un error al elegir el modulo producto";
            }

            if (accesoModuloProductos == 0 && accesoModuloUsuarios == 0 && accesoModuloVentas == 0 && accesoModuloCategorias == 0)
            {
                Mensaje = "Por favor seleccione uno o mas modulos";
            }

            if (string.IsNullOrEmpty(Mensaje))
            {
                return objCapaDato.RegistrarRol(nombreRol, accesoModuloUsuarios, accesoModuloVentas, accesoModuloCategorias, accesoModuloProductos, out Mensaje);
            }
            else
            {
                return false;
            }
        }

        public bool eliminarRol(string idRol, out string mensaje)
        {
            return objCapaDato.eliminarRol(idRol, out mensaje);
        }
    }
}
