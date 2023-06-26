using CapaEntidad;
using CapaNegocio;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CapaPresentacionAdmin.Permisos;
using Microsoft.Ajax.Utilities;
using WebGrease.Css.Extensions;

namespace CapaPresentacionAdmin.Controllers
{
    [Authorize]
    public class MantenedorController : Controller
    {

        // GET: Mantenedor
        [PermisosRol(4)]
        public ActionResult Productos()
        {
            return View();
        }

        [PermisosRol(5)]
        public ActionResult Categoria()
        {
            return View();
        }

        [PermisosRol(6)]
        public ActionResult Roles()
        {
            return View();
        }


        #region PRODUCTO

        [HttpGet/*,PermisosRol(CapaEntidad.Rol.General)*/]
        public JsonResult ListarProducto()
        {
            List<Producto> oLista = new List<Producto>();
            oLista = new CN_Producto().Listar();
            return Json(new { data = oLista }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult GuardarProducto(String objeto, HttpPostedFileBase archivoImagen)
        {

            string mensaje = string.Empty;
            bool operacion_exitosa = true;
            bool Guardar_imagen_exito = true;

            Producto oProducto = new Producto();
            oProducto = JsonConvert.DeserializeObject<Producto>(objeto);

            decimal precio, costo;


            if (decimal.TryParse(oProducto.precioTexto, NumberStyles.AllowDecimalPoint, new CultureInfo("es-NI"), out precio))
            {
                oProducto.precio = precio;
            }
            else
            {
                return Json(new { operacion_exitosa = false, mensaje = "Error con el formato de precio debe ser ##.##" }, JsonRequestBehavior.AllowGet);
            }

            if (decimal.TryParse(oProducto.costoTexto, NumberStyles.AllowDecimalPoint, new CultureInfo("es-NI"), out costo))
            {
                oProducto.costo = costo;
            }
            else
            {
                return Json(new { operacion_exitosa = false, mensaje = "Error con el formato de costo debe ser ##.##" }, JsonRequestBehavior.AllowGet);
            }



            if (oProducto.id_prod == string.Empty)
            {

                string idproducto = new CN_Producto().Registrar(oProducto, out mensaje);
                if (idproducto != string.Empty)
                {
                    oProducto.id_prod = idproducto;
                }
                else
                {
                    operacion_exitosa = false;
                }
            }
            else
            {
                operacion_exitosa = new CN_Producto().Editar(oProducto, out mensaje);
            }


            if (operacion_exitosa)
            {
                if (archivoImagen != null)
                {
                    string ruta_guardar = ConfigurationManager.AppSettings["ServidorFotos"];
                    string extension = Path.GetExtension(archivoImagen.FileName);
                    string nombre_imagen = string.Concat(oProducto.id_prod.ToString(), extension);

                    try
                    {
                        archivoImagen.SaveAs(Path.Combine(ruta_guardar, nombre_imagen));
                    }
                    catch (Exception ex)
                    {
                        mensaje = ex.Message;
                        Guardar_imagen_exito = false;
                    }

                    if (Guardar_imagen_exito)
                    {
                        oProducto.rutafoto = ruta_guardar;
                        oProducto.foto = nombre_imagen;
                        bool rspta = new CN_Producto().GuardarDatosImagen(oProducto, out mensaje);
                    }
                    else
                    {
                        mensaje = "Producto guardado pero hubo error con la imagen";
                    }
                }
            }

            return Json(new { operacionExitosa = operacion_exitosa, idproducto = oProducto.id_prod, mensaje = mensaje }, JsonRequestBehavior.AllowGet);
        }

        //unimos el nombre de la foto y la ruta y la convertimos a texto base64 para pintarla
        [HttpPost]
        public JsonResult ImagenProducto(String id)
        {
            bool conversion;
            Producto oProducto = new CN_Producto().Listar().Where(P => P.id_prod == id).FirstOrDefault();

            string textoBase64 = CN_Recursos.ConvertirBase64(Path.Combine(oProducto.rutafoto, oProducto.foto), out conversion);

            return Json(new
            {

                conversion = conversion,
                textoBase64 = textoBase64,
                extension = Path.GetExtension(oProducto.foto)

            },
                JsonRequestBehavior.AllowGet
            );
        }

        [HttpPost]

        public JsonResult EliminarProducto(string id)
        {
            bool resultado = false;
            resultado = new CN_Producto().Eliminar(id);
            return Json(new { resultado }, JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region CATEGORIA

        [HttpGet/*,PermisosRol(CapaEntidad.Rol.General)*/]
        public JsonResult ListarCategoria()
        {
            List<Categoria> oLista = new List<Categoria>();
            oLista = new CN_Categoria().Listar();
            return Json(new { data = oLista }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult GuardarCategoria(String objeto)
        {

            //bool resEditar = false;
            string mensaje = string.Empty;
            bool salida = true;

            Categoria ocategoria = new Categoria();
            ocategoria = JsonConvert.DeserializeObject<Categoria>(objeto);

            if (ocategoria.id_categoria == "")
            {
                string idcat = new CN_Categoria().Registrar(ocategoria, out mensaje);
                
                if (idcat != string.Empty)
                {
                    ocategoria.id_categoria = idcat;
                }
                else
                {
                    salida = false;
                }
            }
            else
            {
                salida = new CN_Categoria().Editar(ocategoria, out mensaje);
            }

            return Json(new { salida = salida, idcat = ocategoria.id_categoria, mensaje = mensaje }, JsonRequestBehavior.AllowGet);

           
        }

        [HttpPost]
        public JsonResult EliminarCategoria(string id)
        {
            bool resultado = false;
            resultado = new CN_Categoria().Eliminar(id);
            return Json(new { resultado = resultado }, JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region Roles
        [HttpGet]
        public JsonResult ListarRoles()
        {
            List<Rol> oLista = new List<Rol>();
            oLista = new CN_Rol().Listar();
            return Json(new { data = oLista }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult ListarModulosPorRol()
        {
           
            //string rolAdmin = ((Usuario_Admin)Session["ADMIN"]).ORol.IdRol;
            List<RolPorModulo> oLista = new List<RolPorModulo>();
            oLista = new CN_Rol().ListarRolPorModulo();

            return Json(new { data = oLista }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult GuardarRol(string rol,int idu,int idv,int idc,int idp)
        {
            string mensaje = string.Empty;
            bool salida = false;

            
                salida = new CN_Rol().RegistrarRol(rol,idu,idv, idc, idp, out mensaje);
            return Json(new { salida = salida,  mensaje = mensaje }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult eliminarRol(string idRol)
        {
            string mensaje = string.Empty;
            bool resultado = false;

            resultado = new CN_Rol().eliminarRol(idRol, out mensaje);
            return Json(new { resultado = resultado, mensaje = mensaje }, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region CATALOGO
        [HttpGet/*,PermisosRol(CapaEntidad.Rol.General)*/]
        public JsonResult ListarCatalogo()
        {
            List<Catalogo> oLista = new List<Catalogo>();
            oLista = new CN_Catalogo().Listar();
            return Json(new { data = oLista }, JsonRequestBehavior.AllowGet);
        }
        #endregion

    }
}