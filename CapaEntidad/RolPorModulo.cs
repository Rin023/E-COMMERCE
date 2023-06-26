using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaEntidad
{
    public class RolPorModulo
    {
        public string IdRol { get; set; }
        public int IdModulo { get; set; }
        public int Estado { get; set; }
        public string AdminRolActual { get; set; }

    }
}
