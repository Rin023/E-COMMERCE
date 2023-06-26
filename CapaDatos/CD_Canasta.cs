using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using CapaEntidad;

namespace CapaDatos
{
    public class CD_Canasta
    {
        //PEDIR FACTURA
        public List<Factura> Factura(string id_venta) 
        {
            List<Factura> data = new List<Factura>();
            try
            {
                using (SqlConnection oconexion = new SqlConnection(Conexion.cn))
                {
                    SqlCommand cmd = new SqlCommand("SP_Factura", oconexion);
                    cmd.Parameters.AddWithValue("id_venta", id_venta);
                    cmd.CommandType = CommandType.StoredProcedure;
                    oconexion.Open();

                    //cmd.ExecuteNonQuery();
                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            data.Add(new Factura()
                            {
                                VENTA = dr["VENTA"].ToString(),
                                PRODUCTO = dr["PRODUCTO"].ToString(),
                                PRECIO = (float)Convert.ToDecimal(dr["PRECIO"], new CultureInfo("es-NI")),
                                UNIDADES = Convert.ToInt32(dr["UNIDADES"]),
                                TOTAL_UNIDADES = (float)Convert.ToDecimal(dr["TOTAL_UNIDADES"], new CultureInfo("es-NI")),
                                SUB_TOTAL = (float)Convert.ToDecimal(dr["SUB_TOTAL"], new CultureInfo("es-NI")),
                                IMPUESTO = (float)Convert.ToDecimal(dr["IMPUESTO"], new CultureInfo("es-NI")),
                                TOTAL = (float)Convert.ToDecimal(dr["TOTAL"], new CultureInfo("es-NI")),
                                FECHA = dr["FECHA"].ToString()
                        });

                        }
                    }
                }
            }
            catch (Exception ex)
            {
                
            }

            return data;
        }
        //LISTA DE PRODUCTOS PARA LAS VENTAS DE LA TIENDA FISICA
        public List<Producto> Listar()
        {

            List<Producto> lista = new List<Producto>();

            try
            {
                using (SqlConnection oconexion = new SqlConnection(Conexion.cn))
                {
                    oconexion.Open();
                    string query = "SELECT *FROM STOCK WHERE STOCK.ESTADO != 'INHABILITADO'";
                    SqlCommand cmd = new SqlCommand(query, oconexion);
                    cmd.CommandType = CommandType.Text;


                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            lista.Add(new Producto()
                            {
                                id_prod = dr["ID"].ToString(),
                                nombre = dr["NOMBRE"].ToString(),
                                descripcion = dr["DESCRIPCION"].ToString(),
                                precio = Convert.ToDecimal(dr["PRECIO"], new CultureInfo("es-NI")),
                                costo = Convert.ToDecimal(dr["COSTO"], new CultureInfo("es-NI")),
                                stock = Convert.ToInt32(dr["STOCK"]),
                                oCategoria = new Categoria() { id_categoria = dr["IDCATE"].ToString(), nombre_categoria = dr["CATEGORIA"].ToString() },
                                oCatalogo = new Catalogo() { id_catalogo = dr["IDCATA"].ToString(), Nombre = dr["CATALOGO"].ToString() },
                                estado = dr["ESTADO"].ToString(),
                                foto = dr["IMAGEN"].ToString(),
                                rutafoto = dr["RUTA"].ToString()

                            });
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                lista = new List<Producto>();
            }

            return lista;
        }


        //creamos la canasta para el admin que esta en sesion y retornamos el id para ejecutar la venta
        public string CrearCanasta(string id_admin)
        {
            string salida = string.Empty;

            try
            {
                using (SqlConnection oconexion = new SqlConnection(Conexion.cn))
                {
                    SqlCommand cmd = new SqlCommand("SP_CrearVenta", oconexion);
                    cmd.Parameters.AddWithValue("ADMIN", id_admin);
                    cmd.Parameters.Add("SALIDA", SqlDbType.VarChar, 5).Direction = ParameterDirection.Output;
                    cmd.CommandType = CommandType.StoredProcedure;
                    oconexion.Open();
                    cmd.ExecuteNonQuery();
                    salida = Convert.ToString(cmd.Parameters["SALIDA"].Value);
                }

            }
            catch (Exception ex)
            {
                salida = string.Empty;
            }
            return salida;
        }

        //revisamos si el producto ya se encuentra agregado a la canasta
        public bool ExisteCanasta(string id_venta,string id_producto)
        {
            bool resultado = true;

            try
            {
                using (SqlConnection oconexion = new SqlConnection(Conexion.cn))
                {
                    SqlCommand cmd = new SqlCommand("SP_ExisteProductoCanasta", oconexion);
                    cmd.Parameters.AddWithValue("IDVENTA", id_venta);
                    cmd.Parameters.AddWithValue("IDPROD", id_producto);
                    cmd.Parameters.Add("RESULTADO", SqlDbType.Bit).Direction = ParameterDirection.Output;
                    cmd.CommandType = CommandType.StoredProcedure;

                    oconexion.Open();
                    cmd.ExecuteNonQuery();

                    resultado = Convert.ToBoolean(cmd.Parameters["RESULTADO"].Value);
                  
                }
            }
            catch (Exception ex)
            {
                resultado = false;
            }
            return resultado;
        }

        

        //realizamos las operaciones de agregacion,suma y resta de productos
        public bool OperacionCanasta(string id_admin, string id_producto,string id_venta , bool sumar,out string Mensaje)
        {
            bool resultado = true;
            Mensaje = string.Empty;
            try
            {
                using (SqlConnection oconexion = new SqlConnection(Conexion.cn))
                {
                    SqlCommand cmd = new SqlCommand("SP_OperacionCanasta", oconexion);
                    cmd.Parameters.AddWithValue("IDADMIN", id_admin);
                    cmd.Parameters.AddWithValue("IDVENTA", id_venta);
                    cmd.Parameters.AddWithValue("IDPROD", id_producto);
                    cmd.Parameters.AddWithValue("SUMA", sumar);
                    cmd.Parameters.Add("RESULTADO", SqlDbType.Bit).Direction = ParameterDirection.Output;
                    cmd.Parameters.Add("MENSAJE", SqlDbType.VarChar, 500).Direction = ParameterDirection.Output;
                    cmd.CommandType = CommandType.StoredProcedure;

                    oconexion.Open();
                    cmd.ExecuteNonQuery();

                    resultado = Convert.ToBoolean(cmd.Parameters["RESULTADO"].Value);
                    Mensaje = Convert.ToString(cmd.Parameters["MENSAJE"].Value);
                }
            }
            catch (Exception ex)
            {
                resultado = false;
                Mensaje = ex.Message;
            }
            return resultado;
        }


        //listamos los datos de los productos en la canasta para la venta
        public List<Canasta> ListarCanasta(string id_venta,string id_admin)
        {

            List<Canasta> lista = new List<Canasta>();

            try
            {
                using (SqlConnection oconexion = new SqlConnection(Conexion.cn))
                {
                   
                    string query = "SELECT* FROM fn_VerCanasta(@id_venta,@id_admin)";
                    SqlCommand cmd = new SqlCommand(query, oconexion);
                    cmd.Parameters.AddWithValue("@id_venta", id_venta);
                    cmd.Parameters.AddWithValue("@id_admin", id_admin);
                    cmd.CommandType = CommandType.Text;
                    oconexion.Open();

                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            lista.Add(new Canasta()
                            {
       
                                Oproducto = new Producto()
                                {
                                    id_prod = dr["id"].ToString(),
                                    foto = dr["foto"].ToString(),
                                    rutafoto = dr["ruta"].ToString(),
                                    nombre = dr["nombre"].ToString(),
                                    descripcion = dr["descripcion"].ToString(),
                                    precio = Convert.ToDecimal(dr["precio"], new CultureInfo("es-NI"))

                                },
                                unidades = Convert.ToInt32(dr["unidades"]),
                            });
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                lista = new List<Canasta>();
            }

            return lista;
        }

        //eliminamos directamente un producto de la canasta
        public bool EliminarDeCanasta(string id_venta,string id_admin, string id_producto)
        {
            bool resultado = true;

            try
            {
                using (SqlConnection oconexion = new SqlConnection(Conexion.cn))
                {
                    SqlCommand cmd = new SqlCommand("SP_EliminarDeCanasta", oconexion);
                    cmd.Parameters.AddWithValue("IDVENTA", id_venta);
                    cmd.Parameters.AddWithValue("IDADMIN", id_admin);
                    cmd.Parameters.AddWithValue("IDPROD", id_producto);
                    cmd.Parameters.Add("RESULTADO", SqlDbType.Bit).Direction = ParameterDirection.Output;
                    cmd.CommandType = CommandType.StoredProcedure;

                    oconexion.Open();
                    cmd.ExecuteNonQuery();

                    resultado = Convert.ToBoolean(cmd.Parameters["RESULTADO"].Value);

                }
            }
            catch (Exception ex)
            {
                resultado = false;
            }
            return resultado;
        }

        //realizamos el registro de la venta en la tabla de factura_venta
        public bool RealizarVenta(string id_venta, string id_admin , out string Mensaje)
        {
            bool resultado = true;
            Mensaje = string.Empty;
            try
            {
                using (SqlConnection oconexion = new SqlConnection(Conexion.cn))
                {
                    SqlCommand cmd = new SqlCommand("SP_RegistrarVenta", oconexion);
                    cmd.Parameters.AddWithValue("IDVENTA", id_venta);
                    cmd.Parameters.AddWithValue("IDADMIN", id_admin);
                    cmd.Parameters.Add("RESULTADO", SqlDbType.Bit).Direction = ParameterDirection.Output;
                    cmd.Parameters.Add("MENSAJE", SqlDbType.VarChar, 500).Direction = ParameterDirection.Output;
                    cmd.CommandType = CommandType.StoredProcedure;

                    oconexion.Open();
                    cmd.ExecuteNonQuery();

                    Mensaje = Convert.ToString(cmd.Parameters["MENSAJE"].Value);
                    resultado = Convert.ToBoolean(cmd.Parameters["RESULTADO"].Value);

                }
            }
            catch (Exception ex)
            {
                resultado = false;
                Mensaje = ex.Message;
            }
            return resultado;
        }

        //eliminamos todos los registros de productos del detalle venta (vaciamos la canasta)
        public bool CancelarVenta(string id_venta, string id_admin, out string Mensaje)
        {
            bool resultado = true;
            Mensaje = string.Empty;
            try
            {
                using (SqlConnection oconexion = new SqlConnection(Conexion.cn))
                {
                    SqlCommand cmd = new SqlCommand("SP_CancelarVenta", oconexion);
                    cmd.Parameters.AddWithValue("IDVENTA", id_venta);
                    cmd.Parameters.AddWithValue("IDADMIN", id_admin);
                    cmd.Parameters.Add("RESULTADO", SqlDbType.Bit).Direction = ParameterDirection.Output;
                    cmd.Parameters.Add("MENSAJE", SqlDbType.VarChar, 500).Direction = ParameterDirection.Output;
                    cmd.CommandType = CommandType.StoredProcedure;

                    oconexion.Open();
                    cmd.ExecuteNonQuery();

                    Mensaje = Convert.ToString(cmd.Parameters["MENSAJE"].Value);
                    resultado = Convert.ToBoolean(cmd.Parameters["RESULTADO"].Value);

                }
            }
            catch (Exception ex)
            {
                resultado = false;
                Mensaje = ex.Message;
            }
            return resultado;
        }



    }
}
