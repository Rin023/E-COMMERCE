using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaEntidad
{
    public class Usuario
    {

        public string id_usuario { get; set; }
        public string usuario { get; set; }
        public Persona objPersona { get; set; }
        public string contraseña { get; set; }
        public string ConfirmarContra { get; set; }
        public bool Reestrablecer { get; set; }

    }
}
