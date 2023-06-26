using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CapaEntidad;
using System.Web;

namespace CapaDatos
{
    public class CD_Rol
    {

        //creamos los metodos de listado y agregacion de roles para la adminitracion
        public List<Rol> Listar()
        {

            List<Rol> lista = new List<Rol>();

            try
            {
                using (SqlConnection oconexion = new SqlConnection(Conexion.cn))
                {
                    oconexion.Open();
                    string query = "select *from rol where Estado = 1";
                    SqlCommand cmd = new SqlCommand(query, oconexion);
                    cmd.CommandType = CommandType.Text;


                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            lista.Add(new Rol()
                            {
                                IdRol = dr["IdRol"].ToString(),
                                Descripcion = dr["Descripcion"].ToString(),
                                Estado = Convert.ToInt32(dr["Estado"])
                            });
                        }
                    }
                }
            }
            catch
            {
                lista = new List<Rol>();
            }

            return lista;
        }

        //listamos los roles con los modulos a los que tienen acceso
        public List<RolPorModulo> ListarRolPorModulo()
        {
            Usuario_Admin admin;
            List<RolPorModulo> lista = new List<RolPorModulo>();

            if (HttpContext.Current.Session["ADMIN"] != null)
            {
                admin = HttpContext.Current.Session["ADMIN"] as Usuario_Admin;
            }
            else
            {
                admin = new Usuario_Admin()
                {
                    id_admin = null,
                    OclienteUser = null,
                    Opersona = null,
                    ORol = new Rol()
                    {
                        IdRol = "",
                        Descripcion = null,
                        Estado = 0
                    },
                    Ousuario = null
                };
            }
          

            try
            {
                using (SqlConnection oconexion = new SqlConnection(Conexion.cn))
                {
                    oconexion.Open();
                    string query = "select * from MODULO_ROL";
                    SqlCommand cmd = new SqlCommand(query, oconexion);
                    cmd.CommandType = CommandType.Text;


                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            if (dr["IdRol"].ToString() == admin.ORol.IdRol.ToString()) {
                                lista.Add(new RolPorModulo()
                                {
                                    IdRol = dr["IdRol"].ToString(),
                                    IdModulo = Convert.ToInt32(dr["IdModulo"]),
                                    Estado = Convert.ToInt32(dr["Estado"]),
                                    AdminRolActual = admin.ORol.IdRol.ToString()
                                });
                            }
                        }
                    }
                }
      
            }
            catch
            {
                lista = new List<RolPorModulo>();
            }

            return lista;
        }

        //registrar un nuevo rol

        public bool RegistrarRol(string nombreRol, int accesoModuloUsuarios, int accesoModuloVentas, int accesoModuloCategorias, int accesoModuloProductos, out string Mensaje)
        {
            bool salida = false;
            Mensaje = string.Empty;

            try
            {
                using (SqlConnection oconexion = new SqlConnection(Conexion.cn))
                {
                    SqlCommand cmd = new SqlCommand("SP_RegistrarRol", oconexion);
                    cmd.Parameters.AddWithValue("nombreRol", nombreRol);
                    cmd.Parameters.AddWithValue("accesoModuloUsuarios", accesoModuloUsuarios);
                    cmd.Parameters.AddWithValue("accesoModuloVentas", accesoModuloVentas);
                    cmd.Parameters.AddWithValue("accesoModuloCategorias", accesoModuloCategorias);
                    cmd.Parameters.AddWithValue("accesoModuloProductos", accesoModuloProductos);

                    cmd.Parameters.Add("salida", SqlDbType.Bit).Direction = ParameterDirection.Output;
                    cmd.Parameters.Add("mensaje", SqlDbType.VarChar, 200).Direction = ParameterDirection.Output;
                    cmd.CommandType = CommandType.StoredProcedure;

                    oconexion.Open();
                    cmd.ExecuteNonQuery();

                    salida = Convert.ToBoolean(cmd.Parameters["salida"].Value);
                    Mensaje = cmd.Parameters["mensaje"].Value.ToString();
                }
            }
            catch (Exception ex)
            {
                salida = false;
                Mensaje = ex.Message;
            }
            return salida;
        }


        public bool eliminarRol(string idRol,out string Mensaje)
        {
            bool salida = false;
            Mensaje = string.Empty;

            try
            {
                using (SqlConnection oconexion = new SqlConnection(Conexion.cn))
                {
                    SqlCommand cmd = new SqlCommand("SP_EliminarRol", oconexion);
                    cmd.Parameters.AddWithValue("idRol", idRol);

                    cmd.Parameters.Add("salida", SqlDbType.Bit).Direction = ParameterDirection.Output;
                    cmd.Parameters.Add("mensaje", SqlDbType.VarChar, 200).Direction = ParameterDirection.Output;
                    cmd.CommandType = CommandType.StoredProcedure;

                    oconexion.Open();
                    cmd.ExecuteNonQuery();

                    salida = Convert.ToBoolean(cmd.Parameters["salida"].Value);
                    Mensaje = cmd.Parameters["mensaje"].Value.ToString();
                }
            }
            catch (Exception ex)
            {
                salida = false;
                Mensaje = ex.Message;
            }
            return salida;
        }


    }
}
