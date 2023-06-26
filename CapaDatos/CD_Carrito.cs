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
    public class CD_Carrito
    {
        //chequiamos si el producto ya esta en el carrito
        public bool ExisteCarrito(string idclienteU, string idproducto)
        {

            bool resultado = true;
            try
            {
                using (SqlConnection oconexion = new SqlConnection(Conexion.cn))
                {
                    SqlCommand cmd = new SqlCommand("SP_ExisteCarrito", oconexion);
                    cmd.Parameters.AddWithValue("IDUSER", idclienteU);
                    cmd.Parameters.AddWithValue("IDPROD", idproducto);

                    cmd.Parameters.Add("RESULTADO", SqlDbType.Bit).Direction = ParameterDirection.Output;
                    cmd.CommandType = CommandType.StoredProcedure;

                    oconexion.Open();
                    cmd.ExecuteNonQuery();

                    resultado = Convert.ToBoolean(cmd.Parameters["RESULTADO"].Value);
                }
            }
            catch(Exception e)
            {
                resultado = false;
            }
            return resultado;
        }


        //realizamos las operaciones de agregar,sumar y restar productos en el carrito
        public bool operacionCarrito(string idclienteU, string idproducto, bool suma, out string Mensaje)
        {
            bool resultado = true;
            Mensaje = string.Empty;

            try
            {
                using (SqlConnection oconexion = new SqlConnection(Conexion.cn))
                {
                    SqlCommand cmd = new SqlCommand("SP_OperacionCarrito", oconexion);
                    cmd.Parameters.AddWithValue("IDUSER", idclienteU);
                    cmd.Parameters.AddWithValue("IDPROD", idproducto);
                    cmd.Parameters.AddWithValue("SUMAR", suma);
                    cmd.Parameters.Add("MENSAJE", SqlDbType.VarChar, 50).Direction = ParameterDirection.Output;
                    cmd.Parameters.Add("RESULTADO", SqlDbType.Bit).Direction = ParameterDirection.Output;
                    cmd.CommandType = CommandType.StoredProcedure;

                    oconexion.Open();
                    cmd.ExecuteNonQuery();

                    resultado = Convert.ToBoolean(cmd.Parameters["RESULTADO"].Value);
                    Mensaje = cmd.Parameters["MENSAJE"].Value.ToString();
                }
            }
            catch (Exception ex)
            {
                resultado = false;
                Mensaje = ex.Message;
            }
            return resultado;
        }

        //contamos la cantidad de productos que hay en carrito
        public int CantidadEnCarrito(string idclienteU)
        {
            int resultado = 0;
            try
            {
                using (SqlConnection oconexion = new SqlConnection(Conexion.cn))
                {
                    SqlCommand cmd = new SqlCommand("select count(*) from CARRITO where id_clienteU = @idclienteU", oconexion);
                    cmd.Parameters.AddWithValue("@idclienteU", idclienteU);
                    cmd.CommandType = CommandType.Text;
                    oconexion.Open();
                    resultado = Convert.ToInt32(cmd.ExecuteScalar());

                }

            }
            catch(Exception e)
            {
                resultado = 0;
            }
            return resultado;
        }

        //listamos los productos que se encuentran en el carrito
        public List<Carrito> ListarProducto(string idclienteU)
        {

            List<Carrito> lista = new List<Carrito>();

            try
            {
                using (SqlConnection oconexion = new SqlConnection(Conexion.cn))
                {

                    string query = "SELECT* FROM fn_optenerCarritoCliente(@id_clienteU) ";
                    SqlCommand cmd = new SqlCommand(query, oconexion);
                    cmd.Parameters.AddWithValue("@id_clienteU", idclienteU);
                    cmd.CommandType = CommandType.Text;
                    oconexion.Open();

                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            lista.Add(new Carrito()
                            {

                                oProducto = new Producto()
                                {
                                    id_prod = dr["id"].ToString(),
                                    foto = dr["foto"].ToString(),
                                    rutafoto = dr["ruta"].ToString(),
                                    nombre = dr["nombre"].ToString(),
                                    descripcion = dr["descripcion"].ToString(),
                                    precio = Convert.ToDecimal(dr["precio"], new CultureInfo("es-NI"))

                                },
                                Cantidad = Convert.ToInt32(dr["cantidad"])
                            });
                        }
                    }
                }
            }
            catch(Exception e)
            {
                lista = new List<Carrito>();
            }

            return lista;
        }

        //eliminamos un producto espécifico del carrito
        public bool EliminarCarrito(string idclienteU, string idproducto)
        {
            bool resultado = true;

            try
            {
                using (SqlConnection oconexion = new SqlConnection(Conexion.cn))
                {
                    SqlCommand cmd = new SqlCommand("SP_EliminarCarrito", oconexion);
                    cmd.Parameters.AddWithValue("IDUSER", idclienteU);
                    cmd.Parameters.AddWithValue("IDPROD", idproducto);
                    cmd.Parameters.Add("RESULT", SqlDbType.Bit).Direction = ParameterDirection.Output;
                    cmd.CommandType = CommandType.StoredProcedure;

                    oconexion.Open();
                    cmd.ExecuteNonQuery();

                    resultado = Convert.ToBoolean(cmd.Parameters["RESULT"].Value);

                }
            }
            catch(Exception e)
            {
                resultado = false;
            }
            return resultado;
        }

     
    }
}
