using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaEntidad
{
    public class Persona
    {

        public string id_persona { get; set; }
        
        public TipoPersona ObTipoPersona { get; set; }
        public string Nombre { get; set; }

        public string Apellido { get; set; }

        public string Direccion { get; set; }
        
        public string Telefono { get; set; }
        public string correo { get; set; }
        
    }
}
