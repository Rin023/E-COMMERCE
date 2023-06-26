using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaEntidad
{

    public class Carrito
    {
        public string IdCarrito { get; set; }
        public Producto oProducto { get; set; }
        public Usuario_tienda oClienteU { get; set; }
        public int Cantidad { get; set; }
    }
}
