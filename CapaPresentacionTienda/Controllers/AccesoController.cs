using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using CapaEntidad;
using CapaNegocio;

namespace CapaPresentacionTienda.Controllers
{
    public class AccesoController : Controller
    {
        // GET: Acceso
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Registrar()
        {
            return View();
        }

        public ActionResult Reestablecer()
        {
            return View();
        }
        public ActionResult CambiarContra()
        {
            return View();
        }

/***********************      REGISTRAR     ********************************/

        [HttpPost]
        public ActionResult Registrar(Usuario_Admin objeto)
        {
            string resultado;
            string mensaje = string.Empty;
           
            
            ViewData["id_tipo"] = objeto.Opersona.ObTipoPersona.id_tipo = "T002";
            ViewData["REES"] = objeto.Ousuario.Reestrablecer = false;
            ViewData["Nombre"] = string.IsNullOrWhiteSpace(objeto.Opersona.Nombre) ? "" : objeto.Opersona.Nombre;
            ViewData["Apellido"] = string.IsNullOrWhiteSpace(objeto.Opersona.Apellido) ? "" : objeto.Opersona.Apellido;
            ViewData["correo"] = string.IsNullOrWhiteSpace(objeto.Opersona.correo) ? "" : objeto.Opersona.correo;
            ViewData["Direccion"] = string.IsNullOrWhiteSpace(objeto.Opersona.Direccion) ? "" : objeto.Opersona.Direccion;
            ViewData["Telefono"] = string.IsNullOrWhiteSpace(objeto.Opersona.Telefono) ? "" : objeto.Opersona.Telefono;
            ViewData["usuario"] = string.IsNullOrWhiteSpace(objeto.Ousuario.usuario) ? "" : objeto.Ousuario.usuario;

            if(objeto.Ousuario.contraseña != objeto.Ousuario.ConfirmarContra)
            {
                ViewBag.Error = "Las Contraseña no coinciden";
                return View();
            }

            
            resultado = new CN_Usuario().Registrar(objeto,out mensaje, out _);
            
            if(resultado != "")
            {
                ViewBag.Error = null;
                return RedirectToAction("Index", "Acceso");

            }else {
                ViewBag.Error = mensaje;
                return View();
            }    
        }

/***********************      LOGEO    ********************************/
        [HttpPost]
        public ActionResult Index(string usuario, string contra)
        {
            Usuario_Admin oAdmin = new Usuario_Admin();
            oAdmin = new CN_Usuario().ListarUsuarios().Where(u => u.Ousuario.usuario == usuario && u.Ousuario.contraseña == CN_Recursos.ConvertirSha256(contra) && u.Opersona.ObTipoPersona.id_tipo == "T002      ").FirstOrDefault();

            if (oAdmin == null)
            {
                ViewBag.Error = "USUARIO O CONTRASEÑA INCORRECTA";
                return View();
            }
            else
            {
                if (oAdmin.Ousuario.Reestrablecer)
                {
                    TempData["id_usuario"] = oAdmin.Ousuario.id_usuario;
                    return RedirectToAction("CambiarContra", "Acceso");
                }
                else
                {
                    FormsAuthentication.SetAuthCookie(oAdmin.Ousuario.usuario, false);
                    Session["USUARIO"] = oAdmin;
                    ViewBag.Error = null;

                    return RedirectToAction("Index", "Tienda");
                }
            }
        }

 /***********************   Reestablecer   ********************************/
        [HttpPost]
        public ActionResult Reestablecer(string correo)
        {
            Usuario_Admin oAdmin = new Usuario_Admin();
            oAdmin = new CN_Usuario().ListarUsuarios().Where(u => u.Opersona.correo == correo && u.Opersona.ObTipoPersona.id_tipo == "T002      ").FirstOrDefault();

            if (oAdmin == null)
            {
                ViewBag.Error = "No se encontro un usuario con este correo";
                return View();
            }

            string mensaje = string.Empty;
            bool respuesta = new CN_Usuario().ReestablecerContra(oAdmin.Ousuario.id_usuario, correo, out mensaje);

            if (respuesta)
            {
                ViewBag.Error = null;
                TempData["id_usuario"] = oAdmin.Ousuario.id_usuario;
                return RedirectToAction("CambiarContra", "Acceso");
            }
            else
            {
                ViewBag.Error = mensaje;
                return View();
            }
        }

        /***********************   CambiarContraseña   ********************************/
        [HttpPost]
        public ActionResult CambiarContra(string idusuario, string contraActual, string nuevaContra, string ConfirmarContra)
        {
            Usuario_Admin oAdmin = new Usuario_Admin();

            oAdmin = new CN_Usuario().ListarUsuarios().Where(u => u.Ousuario.id_usuario == idusuario && u.Opersona.ObTipoPersona.id_tipo == "T002      ").FirstOrDefault();

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
                ViewBag.Error = "La contraseña actual no coinciden";
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
            /***********************   Cerrar Sesion   ********************************/
            public ActionResult CerrarSesion()
             {
            Session["USUARIO"] = null;
            FormsAuthentication.SignOut();
            return RedirectToAction("Index", "Tienda");
            }

    }
}