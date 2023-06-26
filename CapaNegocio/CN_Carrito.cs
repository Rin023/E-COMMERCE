using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using CapaDatos;
using CapaEntidad;

namespace CapaNegocio
{
    public class CN_Carrito
    {
        private CD_Carrito objcarrito = new CD_Carrito();

        public bool ExisteCarrito(string idclienteU, string idproducto)
        {
            return objcarrito.ExisteCarrito(idclienteU, idproducto);
        }

        public bool operacionCarrito(string idclienteU, string idproducto, bool suma, out string Mensaje)
        {
            return objcarrito.operacionCarrito(idclienteU, idproducto, suma, out Mensaje);
        }

        public int CantidadEnCarrito(string idclienteU)
        {
            return objcarrito.CantidadEnCarrito(idclienteU);
        }

        public List<Carrito> ListarProducto(string idclienteU)
        {
            return objcarrito.ListarProducto(idclienteU);
        }

        public bool EliminarCarrito(string idclienteU, string idproducto)
        {
            return objcarrito.EliminarCarrito( idclienteU, idproducto);
        }


    }
}
