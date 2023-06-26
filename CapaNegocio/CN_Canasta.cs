using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using CapaEntidad;
using CapaDatos;

namespace CapaNegocio
{
    public class CN_Canasta
    {
        private CD_Canasta objCapaDato = new CD_Canasta();

        public List<Producto> Listar()
        {
            return objCapaDato.Listar();
        }

        public List<Factura> Factura(string id_venta)
        {
            return objCapaDato.Factura(id_venta);
        }

        public string CrearCanasta(string id_admin)
        {
            return objCapaDato.CrearCanasta(id_admin);
        }

        public bool ExisteCanasta(string id_venta, string id_producto)
        {
            return objCapaDato.ExisteCanasta(id_venta, id_producto);
        }

        public bool OperacionCanasta(string id_admin, string id_producto, string id_venta, bool sumar, out string Mensaje)
        {
            return objCapaDato.OperacionCanasta(id_admin, id_producto, id_venta, sumar, out Mensaje); 
        }

        public List<Canasta> ListarCanasta(string id_venta, string id_admin)
        {
            return objCapaDato.ListarCanasta(id_venta,id_admin);
        }

        public bool EliminarDeCanasta(string id_venta, string id_admin, string id_producto)
        {
            return objCapaDato.EliminarDeCanasta(id_venta,id_admin,id_producto);
        }

        public bool RealizarVenta(string id_venta, string id_admin, out string Mensaje)
        {
            return objCapaDato.RealizarVenta(id_venta, id_admin,out  Mensaje);
        }

        public bool CancelarVenta(string id_venta, string id_admin, out string Mensaje)
        {
            return objCapaDato.CancelarVenta(id_venta, id_admin, out Mensaje);
        }


    }
}
