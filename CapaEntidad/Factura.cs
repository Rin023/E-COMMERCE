using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaEntidad
{
    public class Factura
    {
        public string VENTA { get; set; }
        public string PRODUCTO { get; set; }
        public float PRECIO { get; set; }
        public int UNIDADES { get; set; }
        public float TOTAL_UNIDADES { get; set; }
        public float SUB_TOTAL { get; set; }
        public float IMPUESTO { get; set; }
        public float TOTAL { get; set; }
        public string FECHA { get; set; }
    }
}
