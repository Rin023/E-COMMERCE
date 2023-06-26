using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CapaDatos;
using CapaEntidad;
namespace CapaNegocio
{
    public class CN_Reporte
    {
        private CD_Reporte objCapaDato = new CD_Reporte();

        public List<Reporte> Ventas(string fechainicio, string fechafin, string idventa)
        {
            return objCapaDato.Ventas(fechainicio, fechafin, idventa);
        }


        public DashBoard verDashBoard()
        {
            return objCapaDato.verDashBoard();
        }
    }
}
