using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using CapaEntidad;
using CapaDatos;

namespace CapaNegocio
{
    public class CN_TipoPersona
    {
        private CD_TipoPersona objTipoPersona = new CD_TipoPersona();

        public List<TipoPersona> Listar()
        {
            return objTipoPersona.Listar();
        }


    }
}
