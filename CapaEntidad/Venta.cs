using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaEntidad
{
    public class Venta
    {
        public string id_venta { get; set; }
        public Usuario_Admin Oadmin { get; set; }
        public Persona Opersona { get; set; }
        public string fecha_facturacion { get; set; }
    }
}
