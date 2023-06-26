using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using CapaDatos;
using CapaEntidad;

namespace CapaNegocio
{
    public class CN_Catalogo
    {

        private CD_Catalogo objCapaDato = new CD_Catalogo();

        public List<Catalogo> Listar()
        {
            return objCapaDato.Listar();
        }

    }
}
