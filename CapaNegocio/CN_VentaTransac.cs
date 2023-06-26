using CapaDatos;
using CapaEntidad;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaNegocio
{
    public class CN_VentaTransac
    {
        private CD_VentaTransac objCapaDato = new CD_VentaTransac();

        public bool Registrar(VentaTransac obj, DataTable DetalleVenta, out string Mensaje)
        {
            return objCapaDato.Registrar(obj, DetalleVenta, out Mensaje);
        }

        public List<DetalleVentaTransac> ListarCompras(string idclienteU)
        {
            return objCapaDato.ListarCompras(idclienteU);
        }
    }
}
