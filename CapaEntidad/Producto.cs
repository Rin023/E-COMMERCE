using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaEntidad
{

	public class Producto
    {
        public string id_prod { get; set; }
		public Categoria oCategoria { get; set; }
		public Catalogo oCatalogo { get; set; }
		public string nombre { get; set; }
		public string descripcion { get; set; }
		public int stock { get; set; }
		public decimal precio { get; set; }
		public string precioTexto { get; set; }
		public decimal costo { get; set; }
		public string costoTexto { get; set; }
		public string estado { get; set; }
		public string foto { get; set; }
		public string rutafoto { get; set; }
		public string Base64 { get; set; }
		public string extension { get; set; }
	}
}
