using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CapaEntidad;
using CapaNegocio;
using ClosedXML.Excel;
using CapaPresentacionAdmin.Permisos;
using System.Text.RegularExpressions;
using System.Text;

namespace CapaPresentacionAdmin.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }
      
        [PermisosRol(2)]
        public ActionResult Usuarios()
        {
            return View();
        }

        [PermisosRol(3)]
        public ActionResult Ventas()
        {
           return View();
        }

        private static readonly Regex regex = new Regex(@"\s+");

        #region DASHBOARD
       

        //LISTAR VENTAS
        [HttpGet]
        public JsonResult ListaReporte(string fechaini, string fechafin, string idventa)
        {
            List<Reporte> oLista = new List<Reporte>();
            oLista = new CN_Reporte().Ventas(fechaini, fechafin, idventa);
            return Json(new { data = oLista }, JsonRequestBehavior.AllowGet);
        }
        //VER TOTALES
        [HttpGet]
        public JsonResult VistaDashBoard()
        {
            DashBoard objeto = new CN_Reporte().verDashBoard();
            return Json(new { resultado = objeto }, JsonRequestBehavior.AllowGet);
        }
        //REPORTE
        [HttpPost]
        public FileResult ExportarVenta(string fechaini, string fechafin, string idventa)
        {
           
            idventa = Regex.Replace(idventa, @"\s+", String.Empty);
            List<Reporte> oLista = new List<Reporte>();
            oLista = new CN_Reporte().Ventas(fechaini, fechafin, idventa);

            DataTable dt = new DataTable();

            dt.Locale = new System.Globalization.CultureInfo("es-NI");
            dt.Columns.Add("Fecha de venta", typeof(String));
            dt.Columns.Add("Cliente", typeof(String));
            dt.Columns.Add("Producto", typeof(String));
            dt.Columns.Add("Precio", typeof(String));
            dt.Columns.Add("Cantidad", typeof(String));
            dt.Columns.Add("Total por unidades", typeof(String));
            dt.Columns.Add("Total", typeof(String));
            dt.Columns.Add("ID de venta", typeof(String));

            foreach (Reporte rp in oLista)
            {
                dt.Rows.Add(new object[]
                {
                    rp.FechaCompra,
                    rp.Cliente,
                    rp.Producto,
                    rp.Precio,
                    rp.Cantidad,
                    rp.TotalxUnidades,
                    rp.Total,
                    rp.ID
                });
            }
            dt.TableName = "Datos";
            using (XLWorkbook wb = new XLWorkbook())
            {
                wb.Worksheets.Add(dt);
                using (MemoryStream stream = new MemoryStream())
                {
                    wb.SaveAs(stream);
                    return File(stream.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "ReporteVenta" + DateTime.Now.ToString() + ".xlsx");
                }
            }
        }
        #endregion

        #region Usuarios
        //USUARIO//
        [HttpGet/*,PermisosRol(CapaEntidad.Rol.General)*/]
        public JsonResult ListarUsuarios()
        {
            List<Usuario_Admin> olista = new List<Usuario_Admin>();
            olista = new CN_Usuario().ListarUsuarios();

            return Json(new { data = olista }, JsonRequestBehavior.AllowGet);
        }

        //LISTAR T PERSONA
        [HttpGet]
        public JsonResult Listar()
        {
            List<TipoPersona> olista = new List<TipoPersona>();
            olista = new CN_TipoPersona().Listar();

            return Json(new { data = olista }, JsonRequestBehavior.AllowGet);
        }

        //GUARDAR Y EDITAR USUARIOS
        [HttpPost]
        public JsonResult GuardarUsuario(Usuario_Admin objeto)
        {

            object resultado = string.Empty;
            bool resEditar = false;
            string mensaje = string.Empty;
            string id_persona = string.Empty;

            ViewData["REES"] = objeto.Ousuario.Reestrablecer = true;

            if (string.IsNullOrEmpty(objeto.Ousuario.id_usuario))
            {
                resultado = new CN_Usuario().Registrar(objeto, out mensaje, out id_persona);
            }
            else
            {
                resEditar = new CN_Usuario().Editar(objeto, out mensaje);
            }

            return Json(new { resultado = resultado, id_persona = id_persona, mensaje = mensaje, resEditar = resEditar}, JsonRequestBehavior.AllowGet);

        }
        #endregion

        #region Ventas

        //LISTAR LOS PRODUCTOS
        [HttpGet]
        public JsonResult ListarProducto()
        {
            List<Producto> oLista = new List<Producto>();
            oLista = new CN_Canasta().Listar();
            return Json(new { data = oLista }, JsonRequestBehavior.AllowGet);
        }
        //crea la venta
        [HttpPost]
        public JsonResult LlenarCanasta(string id_producto)
        {  
            string id_admin = ((Usuario_Admin)Session["ADMIN"]).id_admin;
            string id_venta = new CN_Canasta().CrearCanasta(id_admin);
            bool existe = new CN_Canasta().ExisteCanasta(id_venta, id_producto);
            bool respuesta = false;
            string mensaje = string.Empty;

            if (existe)
            {
                mensaje = "El producto ya esta en la canasta";
            }
            else
            {
                respuesta = new CN_Canasta().OperacionCanasta(id_admin,id_producto,id_venta, true, out mensaje);
            }

            return Json(new { respuesta = respuesta,mensaje = mensaje }, JsonRequestBehavior.AllowGet);
        }


        //ver la canasta
        [HttpPost]
        public JsonResult ListarProductosCanasta()
        {  
            string id_admin = ((Usuario_Admin)Session["ADMIN"]).id_admin;
            string id_venta = new CN_Canasta().CrearCanasta(id_admin);
            List<Canasta> oLista = new List<Canasta>();
            bool conversion;

            oLista = new CN_Canasta().ListarCanasta(id_venta, id_admin).Select(oc => new Canasta()
            {
                Oproducto = new Producto()
                {
                    id_prod = oc.Oproducto.id_prod,
                    nombre = oc.Oproducto.nombre,
                    descripcion = oc.Oproducto.descripcion,
                    precio = oc.Oproducto.precio,
                    rutafoto = oc.Oproducto.rutafoto,
                    Base64 = CN_Recursos.ConvertirBase64(Path.Combine(oc.Oproducto.rutafoto, oc.Oproducto.foto), out conversion),
                    extension = Path.GetExtension(oc.Oproducto.foto)

                },

                unidades = oc.unidades
            }).ToList();

            return Json(new { data = oLista }, JsonRequestBehavior.AllowGet);
        }

        //para las unidades del producto
        [HttpPost]
        public JsonResult OperacionCanasta(string id_producto,bool sumar)
        {
            string id_admin = ((Usuario_Admin)Session["ADMIN"]).id_admin;
            string id_venta = new CN_Canasta().CrearCanasta(id_admin);
            bool respuesta = false;
            string mensaje = string.Empty;

            respuesta = new CN_Canasta().OperacionCanasta(id_admin, id_producto, id_venta, sumar, out mensaje);
           
            return Json(new { respuesta = respuesta, mensaje = mensaje }, JsonRequestBehavior.AllowGet);
        }
        //Eliminar un producto de la canasta
        [HttpPost]
        public JsonResult EliminarDeCanasta(string id_producto)
        {
            string id_admin = ((Usuario_Admin)Session["ADMIN"]).id_admin;
            string id_venta = new CN_Canasta().CrearCanasta(id_admin);
            bool respuesta = false;
            string mensaje = string.Empty;

            respuesta = new CN_Canasta().EliminarDeCanasta(id_venta, id_admin, id_producto);

            return Json(new { respuesta = respuesta, mensaje = mensaje }, JsonRequestBehavior.AllowGet);
        }

        //realizar la venta

        [HttpGet]
        public JsonResult RealizarVenta()
        {
            string id_admin = ((Usuario_Admin)Session["ADMIN"]).id_admin;
            string id_venta = new CN_Canasta().CrearCanasta(id_admin);
            bool respuesta = false;
            string mensaje = string.Empty;

            respuesta = new CN_Canasta().RealizarVenta(id_venta, id_admin, out mensaje);

            if (!respuesta)
            {
                id_venta = String.Empty;
            }

            return Json(new { respuesta = respuesta, id_venta = id_venta, mensaje = mensaje }, JsonRequestBehavior.AllowGet);
        }

        //imprimir factura
        [HttpPost]
        public ActionResult ImprimirFactura(string id_venta)
        {
            List<Factura> datos = new List<Factura>();
            datos = new CN_Canasta().Factura(id_venta);
            string contenidoHtml = GenerarContenidoHtml(datos);

            return Content(contenidoHtml, "text/html", Encoding.UTF8);
        }
        //generar html
        private string GenerarContenidoHtml(List<Factura> datos)
        {
            string contenidoHtml = "";
            foreach (Factura dato in datos)
            {
                contenidoHtml += "<div class='factura'>";
                contenidoHtml += "<div class='encabezado'>";
                contenidoHtml += "<img src='"+ Url.Content("~/img/FerreteriaLogo3.png") + "' alt='Logo de la empresa'>";
                contenidoHtml += "<h1>Factura #" + dato.VENTA + "</h1>";
                contenidoHtml += "<p>Fecha: " + dato.FECHA + "</p>";
                contenidoHtml += "</div>";
                contenidoHtml += "<div class='cuerpo'>";
                contenidoHtml += "<p>Cliente: default </p>";
                contenidoHtml += "<table>";
                contenidoHtml += "<thead>";
                contenidoHtml += "<tr>";
                contenidoHtml += "<th>Producto</th>";
                contenidoHtml += "<th>Cantidad</th>";
                contenidoHtml += "<th>Precio unitario</th>";
                contenidoHtml += "<th>Subtotal</th>";
                contenidoHtml += "</tr>";
                contenidoHtml += "</thead>";
                contenidoHtml += "<tbody>";
                foreach (Factura producto in datos)
                {
                    contenidoHtml += "<tr>";
                    contenidoHtml += "<td>" + producto.PRODUCTO + "</td>";
                    contenidoHtml += "<td>" + producto.UNIDADES + "</td>";
                    contenidoHtml += "<td>" + producto.PRECIO + "</td>";
                    contenidoHtml += "<td>" + producto.TOTAL_UNIDADES + "</td>";
                    contenidoHtml += "</tr>";
                }
                contenidoHtml += "</tbody>";
                contenidoHtml += "<tfoot>";
                contenidoHtml += "<tr>";
                contenidoHtml += "<td colspan='3'>Subtotal</td>";
                contenidoHtml += "<td>" + dato.SUB_TOTAL + "</td>";
                contenidoHtml += "</tr>";
                contenidoHtml += "<tr>";
                contenidoHtml += "<td colspan='3'>Impuesto</td>";
                contenidoHtml += "<td>" + dato.IMPUESTO + "</td>";
                contenidoHtml += "</tr>";
                contenidoHtml += "<tr>";
                contenidoHtml += "<td colspan='3'>Total</td>";
                contenidoHtml += "<td>" + dato.TOTAL + "</td>";
                contenidoHtml += "</tr>";
                contenidoHtml += "</tfoot>";
                contenidoHtml += "</table>";
                contenidoHtml += "</div>";
                contenidoHtml += "</div>";
                break;
            }
            return contenidoHtml;
        }



        //cancelar la venta

        [HttpGet]
        public JsonResult CancelarVenta()
        {
            string id_admin = ((Usuario_Admin)Session["ADMIN"]).id_admin;
            string id_venta = new CN_Canasta().CrearCanasta(id_admin);
            bool respuesta = false;
            string mensaje = string.Empty;

            respuesta = new CN_Canasta().CancelarVenta(id_venta, id_admin, out mensaje);

            return Json(new { respuesta = respuesta, mensaje = mensaje }, JsonRequestBehavior.AllowGet);
        }


        #endregion
     
    }
}


