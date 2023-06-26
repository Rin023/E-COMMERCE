using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaEntidad
{
    public class Usuario_Admin
    {
        public string id_admin { get; set; }
        public Persona Opersona { get; set; }
        public Usuario Ousuario { get; set; }
        public Usuario_tienda OclienteUser { get; set; }
        public  Rol ORol{ get; set; }

    }
}
