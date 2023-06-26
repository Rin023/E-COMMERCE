using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaEntidad
{
    public class Canasta
    {
        public Venta Oventa { get; set; }
        public Usuario_Admin Oadmin { get; set; }
        public Producto Oproducto { get; set; }
        public int unidades { get; set; }
        public float total_por_unidades { get; set; }
    }
    
}
