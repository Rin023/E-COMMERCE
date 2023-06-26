using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using System.Data.SqlClient;
using System.Data;
using CapaEntidad;

namespace CapaDatos
{
    public class CD_Usuario
    {

        //listamos todos los usuarios en general
        public List<Usuario_Admin> ListarUsuarios()
        {
            List<Usuario_Admin> lista = new List<Usuario_Admin>();

            try
            {
                using (SqlConnection oconexion = new SqlConnection(Conexion.cn))
                {
                    oconexion.Open();
                    SqlCommand cmd = new SqlCommand("SP_ListarUsuarios", oconexion);
                    cmd.CommandType = CommandType.StoredProcedure;

                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            lista.Add(new Usuario_Admin()
                            {
                                Ousuario = new Usuario()
                                {
                                    id_usuario = dr["id_usuario"].ToString().Replace(" ", String.Empty),
                                    usuario = dr["usuario"].ToString().Replace(" ", String.Empty),
                                    contraseña = dr["contraseña"].ToString().Replace(" ", String.Empty),
                                    Reestrablecer = Convert.ToBoolean(dr["Reestrablecer"])
                                },                               
                                Opersona = new Persona() {
                                    ObTipoPersona = new TipoPersona()
                                    {
                                        id_tipo = dr["id_tipo"].ToString(),
                                        tipo = dr["tipo"].ToString()
                                    },
                                    id_persona = dr["id_persona"].ToString(),
                                    Nombre = dr["Nombre"].ToString(),
                                    Apellido = dr["Apellido"].ToString(),
                                    Direccion = dr["Direccion"].ToString(),
                                    Telefono = dr["telefono"].ToString(),
                                    correo = dr["correo"].ToString().Replace(" ",String.Empty)
                                },
                                OclienteUser = new Usuario_tienda()
                                {
                                    id_clienteU = dr["id_clienteU"].ToString()
                                },

                                id_admin = dr["id_admin"].ToString(),

                                ORol = new Rol()
                                {
                                    IdRol = dr["IDROL"].ToString(),
                                    Descripcion = dr["ROL"].ToString(),
                                    Estado = Convert.ToInt32(dr["EstadoROL"]),
                                }                                             

                            });

                        }
                    }
                }
            }
            catch
            {
                lista = new List<Usuario_Admin>();
            }
            return lista;

        }

        //registramos un usario el metodo se usa tanto en la tienda como en administracion
        public string Registrar(Usuario_Admin obj, out string Mensaje, out string id_persona)
        {
            string idautogenerado = string.Empty;
            Mensaje = string.Empty;

            try
            {
                using (SqlConnection oconexion = new SqlConnection(Conexion.cn))
                {
                    SqlCommand cmd = new SqlCommand("SP_RegistrarUsuario", oconexion);
                    cmd.Parameters.AddWithValue("TIPO", obj.Opersona.ObTipoPersona.id_tipo);
                    cmd.Parameters.AddWithValue("NOMBRE", obj.Opersona.Nombre);
                    cmd.Parameters.AddWithValue("APELLIDO", obj.Opersona.Apellido);
                    cmd.Parameters.AddWithValue("DIRECCION", obj.Opersona.Direccion);
                    cmd.Parameters.AddWithValue("TELEFONO", obj.Opersona.Telefono);
                    cmd.Parameters.AddWithValue("CORREO", obj.Opersona.correo);
                    cmd.Parameters.AddWithValue("USER", obj.Ousuario.usuario);
                    cmd.Parameters.AddWithValue("PASS", obj.Ousuario.contraseña);
                    cmd.Parameters.AddWithValue("REES", obj.Ousuario.Reestrablecer);
                    
                    if(obj.Opersona.ObTipoPersona.id_tipo == "T001      ")
                        cmd.Parameters.AddWithValue("IDROL", obj.ORol.IdRol);
                    else
                        cmd.Parameters.AddWithValue("IDROL","");

                    cmd.Parameters.Add("IDPERSONA", SqlDbType.VarChar, 6).Direction = ParameterDirection.Output;
                    cmd.Parameters.Add("MENSAJE", SqlDbType.VarChar, 50).Direction = ParameterDirection.Output;
                    cmd.Parameters.Add("RESULTADO", SqlDbType.VarChar, 5).Direction = ParameterDirection.Output;
                    cmd.CommandType = CommandType.StoredProcedure;

                    oconexion.Open();
                    cmd.ExecuteNonQuery();

                    id_persona = cmd.Parameters["IDPERSONA"].Value.ToString();
                    idautogenerado = cmd.Parameters["RESULTADO"].Value.ToString();
                    Mensaje = cmd.Parameters["MENSAJE"].Value.ToString();
                }
            }
            catch (Exception ex)
            {
                idautogenerado = string.Empty;
                Mensaje = ex.Message;
                id_persona = string.Empty;
            }
            return idautogenerado;
        }

        //editamos los datos del usuario y actualizamos
        public bool Editar(Usuario_Admin obj, out string Mensaje)
        {
            bool resultado;
            Mensaje = string.Empty;

            try
            {
                using (SqlConnection oconexion = new SqlConnection(Conexion.cn))
                {
                    SqlCommand cmd = new SqlCommand("SP_ActualizarUsuario", oconexion);

                    cmd.Parameters.AddWithValue("NOMBRE", obj.Opersona.Nombre);
                    cmd.Parameters.AddWithValue("APELLIDO", obj.Opersona.Apellido);
                    cmd.Parameters.AddWithValue("DIRECCION", obj.Opersona.Direccion);
                    cmd.Parameters.AddWithValue("TELEFONO", obj.Opersona.Telefono);
                    cmd.Parameters.AddWithValue("CORREO", obj.Opersona.correo);
                    cmd.Parameters.AddWithValue("USER", obj.Ousuario.usuario);
                    cmd.Parameters.AddWithValue("IDPER", obj.Opersona.id_persona);
                    cmd.Parameters.AddWithValue("IDUSER", obj.Ousuario.id_usuario);

                    if (obj.Opersona.ObTipoPersona.id_tipo == "T001      ")
                        cmd.Parameters.AddWithValue("IDROL", obj.ORol.IdRol);
                    else
                        cmd.Parameters.AddWithValue("IDROL", "");

                    cmd.Parameters.Add("MENSAJE", SqlDbType.VarChar, 50).Direction = ParameterDirection.Output;
                    cmd.Parameters.Add("RESULTADO", SqlDbType.Int).Direction = ParameterDirection.Output;
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


        //activamos el restablecer si es necesario para que el usuario cambie la contraseña generica por una propia
        public bool ReestablecerContra(string id_usuario, string contra, out string Mensaje)
        {
            bool resultado = false;
            Mensaje = string.Empty;

            try
            {
                using (SqlConnection oconexion = new SqlConnection(Conexion.cn))
                {
                    SqlCommand cmd = new SqlCommand("UPDATE USUARIO SET contraseña = @contra, Reestrablecer = 1 where id_usuario = @id", oconexion);
                    cmd.Parameters.AddWithValue("@id", id_usuario);
                    cmd.Parameters.AddWithValue("@contra", contra);
                    cmd.CommandType = CommandType.Text;
                    oconexion.Open();
                    resultado = cmd.ExecuteNonQuery() > 0 ? true : false;


                }

            }
            catch (Exception ex)
            {
                resultado = false;
                Mensaje = ex.Message;
            }

            return true;
        }

        //registramos nueva contraseña restablecida y desactivamos el restablecer
        public bool CambiarContra(string id_usuario, string nuevaContra, out string Mensaje)
        {
            bool resultado = false;
            Mensaje = string.Empty;

            try
            {
                using (SqlConnection oconexion = new SqlConnection(Conexion.cn))
                {
                    SqlCommand cmd = new SqlCommand("UPDATE USUARIO SET contraseña = @nuevaContra, Reestrablecer = 0 where id_usuario = @id", oconexion);
                    cmd.Parameters.AddWithValue("@id", id_usuario);
                    cmd.Parameters.AddWithValue("@nuevaContra", nuevaContra);
                    cmd.CommandType = CommandType.Text;
                    oconexion.Open();
                    resultado = cmd.ExecuteNonQuery() > 0 ? true : false;
                }

            }
            catch (Exception ex)
            {
                resultado = false;
                Mensaje = ex.Message;
            }

            return true;
        }


    }
}
