using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaEntidad
{

    public class VentaTransac
    {
		public string idVenta { get; set; }
        public string id_clienteU { get; set; }
        public decimal TotalProducto { get; set; }
        public decimal MontoTotal { get; set; }
        public string Contacto { get; set; }
        public string IdMunicipio { get; set; }
        public string Telefono { get; set; }
        public string Direccion { get; set; }
        public string IdTransaccion { get; set; }
        public string FechaVenta { get; set; }
        public List<DetalleVentaTransac> oDetalleVentaTransac { get; set; }

    }
}
