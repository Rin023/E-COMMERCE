using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;

namespace CapaDatos
{
    public  class Conexion
    {
        //guardamos la cadena de conexion para usarla en los metodos de todo el proyecto
        public static string cn = ConfigurationManager.ConnectionStrings["cadena"].ToString();

    }
}
