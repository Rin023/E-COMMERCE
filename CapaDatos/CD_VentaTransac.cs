using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Data.SqlClient;
using System.Data;
using CapaEntidad;
using System.Globalization;

namespace CapaDatos
{
    public class CD_VentaTransac
    {

        //registramos la venta_transaccion y su detalle de venta
        public bool Registrar(VentaTransac obj,DataTable DetalleVenta, out string Mensaje)
        {
            bool respuesta = false;
            Mensaje = string.Empty;

            try
            {
                using (SqlConnection oconexion = new SqlConnection(Conexion.cn))
                {
                    SqlCommand cmd = new SqlCommand("uSP_RegistrarVenta", oconexion);

                    cmd.Parameters.AddWithValue("IdCliente", obj.id_clienteU);
                    cmd.Parameters.AddWithValue("TotalProducto", obj.TotalProducto);
                    cmd.Parameters.AddWithValue("MontoTotal", obj.MontoTotal);
                    cmd.Parameters.AddWithValue("Contacto", obj.Contacto);
                    cmd.Parameters.AddWithValue("IdMunicipio", obj.IdMunicipio);
                    cmd.Parameters.AddWithValue("Telefono", obj.Telefono);
                    cmd.Parameters.AddWithValue("Direccion", obj.Direccion);
                    cmd.Parameters.AddWithValue("IdTransaccion", obj.IdTransaccion);
                    cmd.Parameters.AddWithValue("DetalleVenta", DetalleVenta);

                    cmd.Parameters.Add("Resultado", SqlDbType.Bit).Direction = ParameterDirection.Output;
                    cmd.Parameters.Add("Mensaje", SqlDbType.VarChar,500).Direction = ParameterDirection.Output;
                    cmd.CommandType = CommandType.StoredProcedure;

                    oconexion.Open();
                    cmd.ExecuteNonQuery();

                    respuesta = Convert.ToBoolean(cmd.Parameters["Resultado"].Value);
                    Mensaje = cmd.Parameters["Mensaje"].Value.ToString();
                }
            }
            catch (Exception ex)
            {
                respuesta = false;
                Mensaje = ex.Message;
            }
            return respuesta;
        }

        //METODO PARA LISTAR LAS COMPRAS DEL USUARIO
        public List<DetalleVentaTransac> ListarCompras(string idclienteU)
        {

            List<DetalleVentaTransac> lista = new List<DetalleVentaTransac>();

            try
            {
                using (SqlConnection oconexion = new SqlConnection(Conexion.cn))
                {

                    string query = "SELECT* FROM fn_ListarCompras(@id_clienteU)";
                    SqlCommand cmd = new SqlCommand(query, oconexion);
                    cmd.Parameters.AddWithValue("@id_clienteU", idclienteU);
                    cmd.CommandType = CommandType.Text;
                    oconexion.Open();

                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            lista.Add(new DetalleVentaTransac()
                            {

                                oProducto = new Producto()
                                {
                                    foto = dr["foto"].ToString(),
                                    rutafoto = dr["rutafoto"].ToString(),
                                    nombre = dr["Nombre"].ToString(),
                                    precio = Convert.ToDecimal(dr["precio"], new CultureInfo("es-NI"))

                                },
                                Cantidad = Convert.ToInt32(dr["cantidad"]),
                                Total = Convert.ToDecimal(dr["Total"], new CultureInfo("es-NI")),
                                IdTransaccion = dr["IDTransaccion"].ToString()
                            });
                        }
                    }
                }
            }
            catch (Exception e)
            {
                lista = new List<DetalleVentaTransac>();
            }

            return lista;
        }


    }
}
