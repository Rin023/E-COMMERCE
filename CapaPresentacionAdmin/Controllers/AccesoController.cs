using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using CapaEntidad;
using CapaNegocio;


namespace CapaPresentacionAdmin.Controllers
{
    public class AccesoController : Controller
    {
        // GET: Acceso
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult Reestablecer()
        {
            return View();
        }
        public ActionResult CambiarContraseña()
        {
            return View();
        }

        //VALIDAMOS EL INICIO DE SESION TOMANDO LOS DATOS DEL LOGIN
        [HttpPost]
        public ActionResult Index(string usuario, string contra)
        {
            Usuario_Admin oAdmin = new Usuario_Admin();
            oAdmin = new CN_Usuario().ListarUsuarios().Where(u => u.Ousuario.usuario == usuario && u.Ousuario.contraseña == CN_Recursos.ConvertirSha256(contra) && u.Opersona.ObTipoPersona.id_tipo == "T001      ").FirstOrDefault();
            
            if(oAdmin == null)
            {
                ViewBag.Error = "USUARIO O CONTRASEÑA INCORRECTA";
                return View();
            }
            else
            {
                if (oAdmin.Ousuario.Reestrablecer)
                {
                    TempData["id_usuario"] = oAdmin.Ousuario.id_usuario;
                    return RedirectToAction("CambiarContraseña", "Acceso");
                }
                else
                {
                    FormsAuthentication.SetAuthCookie(oAdmin.Ousuario.usuario, false);

                    Session["ADMIN"] = oAdmin;
                    ViewBag.Error = null;
                    return RedirectToAction("Index", "Home");
                }
               
            }
         
        }

        //RESTABLECEMOS LA CONTRASEÑA MEDIANDO LA GENERACION DE UNA NUEVA COMO KEY ENVIADA AL CORREO (EL RESTABLECER SE ACTIVA)
        [HttpPost]
        public ActionResult Reestablecer(string correo)
        {
            Usuario_Admin oAdmin = new Usuario_Admin();
            oAdmin = new CN_Usuario().ListarUsuarios().Where(u => u.Opersona.correo == correo && u.Opersona.ObTipoPersona.id_tipo == "T001      ").FirstOrDefault();

            if (oAdmin == null)
            {
                ViewBag.Error = "No se encontro un Administrador con este correo";
                return View();
            }

            string mensaje = string.Empty;
            bool respuesta = new CN_Usuario().ReestablecerContra(oAdmin.Ousuario.id_usuario, correo, out mensaje);

            if (respuesta)
            {
                ViewBag.Error = null;
                TempData["id_usuario"] = oAdmin.Ousuario.id_usuario;
                return RedirectToAction("CambiarContraseña", "Acceso");

            }
            else
            {
                ViewBag.Error = mensaje;
                return View();
            }

        }
        

        //VALIDAMOS LA CONTRASEÑA GENERADA Y LA NUEVA QUE EL USUARIO VA USAR Y RETORNAMOS AL
        //LOGIN PARA QUE INGRESE CON LA NUEVA PASS (EL RESTABLECER SE DESACTIVA)
        [HttpPost]
        public ActionResult CambiarContraseña(string idusuario, string contraActual, string nuevaContra, string ConfirmarContra)
        {
            Usuario_Admin oAdmin = new Usuario_Admin();

            oAdmin = new CN_Usuario().ListarUsuarios().Where(u => u.Ousuario.id_usuario == idusuario && u.Opersona.ObTipoPersona.id_tipo == "T001      ").FirstOrDefault();

            if (oAdmin.Ousuario.contraseña != CN_Recursos.ConvertirSha256(contraActual))
            {
                TempData["id_usuario"] = idusuario;
                ViewData["vclave"] = "";
                ViewBag.Error = "La contraseña actual no es correcta";
                return View();

            }
            else if (nuevaContra != ConfirmarContra)
            {
                TempData["id_usuario"] = idusuario;
                ViewData["vclave"] = contraActual;
                ViewBag.Error = "Las contraseñas no coinciden";
                return View();
            }
            ViewData["vclave"] = "";

            nuevaContra = CN_Recursos.ConvertirSha256(nuevaContra);

            string mensaje = string.Empty;

            bool respuesta = new CN_Usuario().CambiarContra(idusuario, nuevaContra, out mensaje);

            if (respuesta)
            {
                return RedirectToAction("Index");
            }
            else
            {
                TempData["id_usuario"] = idusuario;

                ViewBag.Error = mensaje;
                return View();
            }

        }


        //METODO PARA TERMINAR LA SESION
        public ActionResult CerrarSesion()
        {
            Session["ADMIN"] = null;
            FormsAuthentication.SignOut();
            return RedirectToAction("Index", "Acceso");
        }

    }
}
