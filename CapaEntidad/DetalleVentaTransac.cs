using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaEntidad
{
 
    public class DetalleVentaTransac
    {
		public string IdVentaTransac { get; set; }
		public string IdVenta { get; set; }
        public Producto oProducto { get; set; }
        public int Cantidad { get; set; }
        public decimal Total { get; set; }
        public string IdTransaccion { get; set; }

    }
}
