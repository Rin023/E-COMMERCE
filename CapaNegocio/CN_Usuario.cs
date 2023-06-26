using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CapaDatos;
using CapaEntidad;

namespace CapaNegocio
{
    public class CN_Usuario
    {
        private CD_Usuario objCapaDato = new CD_Usuario();

        public List<Usuario_Admin> ListarUsuarios()
        {
            return objCapaDato.ListarUsuarios();
        }

        public string Registrar(Usuario_Admin obj, out string Mensaje, out string id_persona)
        {
            Mensaje = string.Empty;

            if (string.IsNullOrEmpty(obj.Opersona.ObTipoPersona.id_tipo) || string.IsNullOrWhiteSpace(obj.Opersona.ObTipoPersona.id_tipo))
            {
                Mensaje = "Por favor! seleccione el tipo de usuario";
            }
            if (obj.Opersona.ObTipoPersona.id_tipo == "T001      " && (string.IsNullOrEmpty(obj.ORol.IdRol) || string.IsNullOrWhiteSpace(obj.ORol.IdRol))  )
            {
                Mensaje = "Por favor! elija el rol de este usuario";
            }
            if (string.IsNullOrEmpty(obj.Opersona.Nombre) || string.IsNullOrWhiteSpace(obj.Opersona.Nombre))
            {
                Mensaje = "El Nombre no puede estar vacio";
            }
            if (string.IsNullOrEmpty(obj.Opersona.Apellido) || string.IsNullOrWhiteSpace(obj.Opersona.Apellido))
            {
                Mensaje = "El Apellido no puede estar vacio";
            }
            if (string.IsNullOrEmpty(obj.Opersona.Direccion) || string.IsNullOrWhiteSpace(obj.Opersona.Direccion))
            {
                Mensaje = "La Direccion no puede estar vacio";
            }
            if (string.IsNullOrEmpty(obj.Opersona.Telefono) || string.IsNullOrWhiteSpace(obj.Opersona.Telefono))
            {
                Mensaje = "El número de teléfono no puede estar vacio";
            }
            if (string.IsNullOrEmpty(obj.Opersona.correo) || string.IsNullOrWhiteSpace(obj.Opersona.correo))
            {
                Mensaje = "El correo no puede estar vacio";
            }
            if (string.IsNullOrEmpty(obj.Ousuario.usuario) || string.IsNullOrWhiteSpace(obj.Ousuario.usuario))
            {
                Mensaje = "El nombre de usuario no puede estar vacio";
            }
            if (obj.Opersona.ObTipoPersona.id_tipo == "T002" && obj.Ousuario.contraseña != "def$&00" && (string.IsNullOrEmpty(obj.Ousuario.contraseña) || string.IsNullOrWhiteSpace(obj.Ousuario.contraseña)))
            {
                Mensaje = "La contraseña  no puede estar vacia";
            }

            if (string.IsNullOrEmpty(Mensaje))
            {
                if (obj.Ousuario.contraseña != "def$&00")
                {
                    obj.Ousuario.contraseña = CN_Recursos.ConvertirSha256(obj.Ousuario.contraseña);
                    return objCapaDato.Registrar(obj, out Mensaje, out id_persona);
                }
                else
                {

                    String contraseña = CN_Recursos.GenerarClave();
                    obj.Ousuario.contraseña = CN_Recursos.ConvertirSha256(contraseña);
                    string result = objCapaDato.Registrar(obj, out Mensaje, out id_persona);

                  
                    if (result == string.Empty)
                    {
                        return result;
                    }
                    else
                    {
                        string asunto = "CREACIÓN DE CUENTA ";
                        string mensaje_correo = "<h3>Su cuenta fue creada correctamente</h3></br><p>Su contraseña para acceder es: !clave!</p>";
                        mensaje_correo = mensaje_correo.Replace("!clave!", contraseña);
                        bool respuesta = CN_Recursos.EnviarCorreo(obj.Opersona.correo, asunto, mensaje_correo);

                        if (respuesta)
                        {
                            return result;
                        }
                        else
                        {
                            Mensaje = "El usuario ha sido creado pero no se pudo enviar el correo";
                            return result;
                        }

                    }
                      

                  
                }
            }
            else
            {
                id_persona = string.Empty;
                return string.Empty;

            }

        }


        public bool Editar(Usuario_Admin obj, out string Mensaje)
        {
            Mensaje = string.Empty;

            if (string.IsNullOrEmpty(obj.Opersona.ObTipoPersona.id_tipo) || string.IsNullOrWhiteSpace(obj.Opersona.ObTipoPersona.id_tipo))
            {
                Mensaje = "Por favor! seleccione el tipo de usuario";
            }
            if (obj.Opersona.ObTipoPersona.id_tipo == "T001      " && (string.IsNullOrEmpty(obj.ORol.IdRol) || string.IsNullOrWhiteSpace(obj.ORol.IdRol)))
            {
                Mensaje = "Por favor! elija el rol de este usuario";
            }
            if (string.IsNullOrEmpty(obj.Opersona.Nombre) || string.IsNullOrWhiteSpace(obj.Opersona.Nombre))
            {
                Mensaje = "El Nombre no puede estar vacio";
            }
            if (string.IsNullOrEmpty(obj.Opersona.Apellido) || string.IsNullOrWhiteSpace(obj.Opersona.Apellido))
            {
                Mensaje = "El Apellido no puede estar vacio";
            }
            if (string.IsNullOrEmpty(obj.Opersona.Direccion) || string.IsNullOrWhiteSpace(obj.Opersona.Direccion))
            {
                Mensaje = "La Direccion no puede estar vacio";
            }
            if (string.IsNullOrEmpty(obj.Opersona.Telefono) || string.IsNullOrWhiteSpace(obj.Opersona.Telefono))
            {
                Mensaje = "El número de teléfono no puede estar vacio";
            }
            if (string.IsNullOrEmpty(obj.Opersona.correo) || string.IsNullOrWhiteSpace(obj.Opersona.correo))
            {
                Mensaje = "El correo no puede estar vacio";
            }
            if (string.IsNullOrEmpty(obj.Ousuario.usuario) || string.IsNullOrWhiteSpace(obj.Ousuario.usuario))
            {
                Mensaje = "El nombre de usuario no puede estar vacio";
            }
           /* if (string.IsNullOrEmpty(obj.Ousuario.contraseña) || string.IsNullOrWhiteSpace(obj.Ousuario.contraseña))
            {
                Mensaje = "La contraseña  no puede estar vacio";
            }
            */
            if (string.IsNullOrEmpty(Mensaje))
            {
                return objCapaDato.Editar(obj, out Mensaje);
            }
            else
            {
                return false;
            }
        }

        public bool ReestablecerContra(string id_usuario, string correo, out string Mensaje)
        {
            Mensaje = string.Empty;
            string nuevacontra = CN_Recursos.GenerarClave();
            bool resultado = objCapaDato.ReestablecerContra(id_usuario, CN_Recursos.ConvertirSha256(nuevacontra), out Mensaje);

            if (resultado)
            {
                string asunto = "CONTRASEÑA REESTABLECIDA";
                string mensaje_correo = "<h3>Su contraseña fue reestablecida</h3></br><p>Su contraseña para acceder es: !clave!</p>";
                mensaje_correo = mensaje_correo.Replace("!clave!", nuevacontra);

                bool respuesta = CN_Recursos.EnviarCorreo(correo, asunto, mensaje_correo);

                if (respuesta)
                {
                    return true;
                }
                else
                {
                    Mensaje = "No se pudo enviar el correo";
                    return false;
                }
            }
            else
            {
                Mensaje = "NO SE PUDO REESTABLECER LA CONTRASEÑA";
                return false;
            }

        }

        public bool CambiarContra(string id_usuario, string nuevaContra, out string Mensaje)
        {
            return objCapaDato.CambiarContra(id_usuario, nuevaContra, out Mensaje);
        }

    }
}
