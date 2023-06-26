using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using CapaEntidad;
using System.Data.SqlClient;
using System.Data;
using System.Globalization;

namespace CapaDatos
{
    public class CD_Reporte
    {
        //Generamos un reporte general tanto de ventas y transacciones
        public List<Reporte> Ventas(string fechainicio,string fechafin,string idventa)
        {

            List<Reporte> lista = new List<Reporte>();

            try
            {
                using (SqlConnection oconexion = new SqlConnection(Conexion.cn))
                {                  
                    SqlCommand cmd = new SqlCommand("SP_ReporteVentas&Transacciones", oconexion);
                    cmd.Parameters.AddWithValue("FECHAINI", fechainicio);
                    cmd.Parameters.AddWithValue("FECHAFIN", fechafin);
                    cmd.Parameters.AddWithValue("ID", idventa);
                    cmd.CommandType = CommandType.StoredProcedure;
                    oconexion.Open();

                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            lista.Add(new Reporte()
                            {
                                FechaCompra = dr["Fecha Compra"].ToString(),
                                Cliente = dr["Cliente"].ToString(),
                                Producto = dr["Producto"].ToString(),
                                Precio = (float)Convert.ToDecimal(dr["Precio"], new CultureInfo("es-NI")),
                                Cantidad = Convert.ToInt32(dr["Cantidad"]),
                                TotalxUnidades = (float)Convert.ToDecimal(dr["Total x Unidades"], new CultureInfo("es-NI")),
                                Total = (float)Convert.ToDecimal(dr["Total"], new CultureInfo("es-NI")),
                                ID = dr["ID"].ToString(),
                            });
                        }
                    }
                }
            }
            catch
            {
                lista = new List<Reporte>();
            }

            return lista;
        }

        //hacemos conteo de usuarios,transacciones,ventas y productos para el dashboard
        public DashBoard verDashBoard()
        {

            DashBoard objeto = new DashBoard();

            try
            {
                using (SqlConnection oconexion = new SqlConnection(Conexion.cn))
                {
                   
                    SqlCommand cmd = new SqlCommand("SP_ReporteDashboard", oconexion);
                    cmd.CommandType = CommandType.StoredProcedure;
                    oconexion.Open();

                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            objeto = new DashBoard()
                            {
                                TotalUsuarios = Convert.ToInt32(dr["TotalUsuarios"]),
                                TotalTransacciones = Convert.ToInt32(dr["TotalTransacciones"]),
                                TotalProductos = Convert.ToInt32(dr["TotalProductos"]),
                                TotalVentas = Convert.ToInt32(dr["TotalVentas"]),
                                
                            };
                        }
                    }
                }
            }
            catch
            {
                objeto = new DashBoard();
            }

            return objeto;
        }

    }
}
