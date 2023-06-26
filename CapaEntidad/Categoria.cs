using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaEntidad
{
 
    public class Categoria
    {
        public string id_categoria { get; set; }
        public Catalogo oCatalogo { get; set; }
        public string nombre_categoria { get; set; }
        public string estado { get; set; }

    }
}
