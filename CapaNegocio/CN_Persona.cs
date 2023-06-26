using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using CapaDatos;
using CapaEntidad;

namespace CapaNegocio
{
    public class CN_Persona
    {
        private CD_Persona objPersona = new CD_Persona();
        public List<Persona> Listar()
        {
            return objPersona.Listar();
        }

    }
}
