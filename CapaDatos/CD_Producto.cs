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
    public class CD_Producto
    {

        /*
         un listar producto tanto para la tienda como para la administracion
        con el fin de mostrar todos los datos de los productos existentes y ofrecidos al publico
         */
        public List<Producto> Listar()
        {

            List<Producto> lista = new List<Producto>();

            try
            {
                using (SqlConnection oconexion = new SqlConnection(Conexion.cn))
                {
                    oconexion.Open();
                    string query = "SELECT *FROM STOCK";
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
                                precio = Convert.ToDecimal (dr["PRECIO"],new CultureInfo("es-NI")),
                                costo = Convert.ToDecimal(dr["COSTO"], new CultureInfo("es-NI")),
                                stock = Convert.ToInt32 (dr["STOCK"]),
                                oCategoria  = new Categoria() { id_categoria = dr["IDCATE"].ToString() , nombre_categoria = dr["CATEGORIA"].ToString() },
                                oCatalogo = new Catalogo() { id_catalogo = dr["IDCATA"].ToString(), Nombre =  dr["CATALOGO"].ToString() },
                                estado = dr["ESTADO"].ToString(),
                                foto = dr["IMAGEN"].ToString(),
                                rutafoto = dr["RUTA"].ToString()

                            });
                        }
                    }
                }
            }
            catch
            {
                lista = new List<Producto>();
            }

            return lista;
        }

        
         //registramos un nuevo producto
         
        public string Registrar(Producto obj, out string Mensaje)
        {
            string id = string.Empty;
            Mensaje = string.Empty;

            try
            {
                using (SqlConnection oconexion = new SqlConnection(Conexion.cn))
                {
                    SqlCommand cmd = new SqlCommand("SP_RegistrarProducto", oconexion);
                    cmd.Parameters.AddWithValue("IDCATE", obj.oCategoria.id_categoria);
                    cmd.Parameters.AddWithValue("IDCATA", obj.oCatalogo.id_catalogo);
                    cmd.Parameters.AddWithValue("NOMBRE", obj.nombre);
                    cmd.Parameters.AddWithValue("DESCRIPCION", obj.descripcion);
                    cmd.Parameters.AddWithValue("STOCK", obj.stock);
                    cmd.Parameters.AddWithValue("PRECIO", obj.precio);
                    cmd.Parameters.AddWithValue("COSTO", obj.costo);
                    cmd.Parameters.AddWithValue("ESTADO", obj.estado);
                    cmd.Parameters.Add("SALIDA", SqlDbType.VarChar, 5).Direction = ParameterDirection.Output;
                    cmd.Parameters.Add("MENSAJE", SqlDbType.VarChar, 50).Direction = ParameterDirection.Output;
                    cmd.CommandType = CommandType.StoredProcedure;

                    oconexion.Open();
                    cmd.ExecuteNonQuery();

                    id = Convert.ToString(cmd.Parameters["SALIDA"].Value);
                    Mensaje = cmd.Parameters["MENSAJE"].Value.ToString();
                }
            }
            catch (Exception ex)
            {
                id = string.Empty;
                Mensaje = ex.Message;
            }
            return id;
        }

       //editamos los datos del producto
         
        public bool Editar(Producto obj, out string Mensaje)
        {
            bool result = false;
            Mensaje = string.Empty;

            try
            {
                using (SqlConnection oconexion = new SqlConnection(Conexion.cn))
                {
                    SqlCommand cmd = new SqlCommand("SP_ActualizarProducto", oconexion);
                    cmd.Parameters.AddWithValue("PROD", obj.id_prod);
                    cmd.Parameters.AddWithValue("IDCATE", obj.oCategoria.id_categoria);
                    cmd.Parameters.AddWithValue("NOMBRE", obj.nombre);
                    cmd.Parameters.AddWithValue("DESCRIPCION", obj.descripcion);
                    cmd.Parameters.AddWithValue("STOCK", obj.stock);
                    cmd.Parameters.AddWithValue("PRECIO", obj.precio);
                    cmd.Parameters.AddWithValue("COSTO", obj.costo);
                    cmd.Parameters.AddWithValue("ESTADO", obj.estado);
                    cmd.Parameters.Add("SALIDA", SqlDbType.Bit).Direction = ParameterDirection.Output;
                    cmd.Parameters.Add("MENSAJE", SqlDbType.VarChar, 50).Direction = ParameterDirection.Output;
                    cmd.CommandType = CommandType.StoredProcedure;

                    oconexion.Open();
                    cmd.ExecuteNonQuery();

                    result = Convert.ToBoolean(cmd.Parameters["SALIDA"].Value);
                    Mensaje = Convert.ToString(cmd.Parameters["MENSAJE"].Value);
                }
            }
            catch (Exception ex)
            {
                result = false;
                Mensaje = ex.Message;
            }
            return result;
        }

        /*
        deshabilitamos un producto,realizamos borrado logico para evitar conflictos con
        los registros de la base de datos
        */

        public bool Eliminar(string id_prod)
        {

            using (SqlConnection oconexion = new SqlConnection(Conexion.cn))
            {
                SqlCommand cmd = new SqlCommand("SP_DesactivarProducto", oconexion);
                cmd.Parameters.AddWithValue("IDPROD", id_prod);
                cmd.CommandType = CommandType.StoredProcedure;
                oconexion.Open();
                cmd.ExecuteNonQuery();


            }
            return true;
        }

        /*
         guardamos los datos de la imagen de nuestro producto que seleccionamos de nuestros archivos para subirla
        a la base de datos con un formato especifico
         */
        public bool GuardarDatosImagen(Producto obj, out string Mensaje)
        {
            bool resultado = false;
            Mensaje = string.Empty;


            try
            {
                using (SqlConnection oconexion = new SqlConnection(Conexion.cn))
                {
                    string query = "UPDATE PRODUCTO SET rutafoto = @rutafoto,foto = @foto WHERE id_prod = @id_prod";

                    SqlCommand cmd = new SqlCommand(query, oconexion);

                    cmd.Parameters.AddWithValue("@rutafoto", obj.rutafoto);
                    cmd.Parameters.AddWithValue("@foto", obj.foto);
                    cmd.Parameters.AddWithValue("@id_prod", obj.id_prod);

 
                    cmd.CommandType = CommandType.Text;
                    oconexion.Open();

                    if (cmd.ExecuteNonQuery() > 0)
                    {
                        resultado = true;
                    }
                    else
                    {
                        Mensaje = "no se pudo actualizar imagen";
                    }

         
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
