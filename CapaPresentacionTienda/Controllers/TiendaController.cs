using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CapaEntidad;
using CapaEntidad.Paypal;
using CapaNegocio;
using System.IO;
using CapaPresentacionTienda.Filter;
using System.ComponentModel;
using System.Collections;
using System.Threading.Tasks;
using System.Data;
using System.Globalization;

namespace CapaPresentacionTienda.Controllers
{
    public class TiendaController : Controller
    {
        // GET: Tienda
        public ActionResult Index()
        {
            return View();
        }
        [ValidarSession]
        [Authorize]
        public ActionResult Carrito()
        {
            return View();
        }

        #region DETALLE DEL PRODUCTO
        public ActionResult DetalleProducto(string idproducto = " ")
        {

            Producto oProducto = new Producto();
            bool conversion;

            oProducto = new CN_Producto().Listar().Where(p => p.id_prod == idproducto).FirstOrDefault();

            if (oProducto != null)
            {
                oProducto.Base64 = CN_Recursos.ConvertirBase64(Path.Combine(oProducto.rutafoto, oProducto.foto), out conversion);
                oProducto.extension = Path.GetExtension(oProducto.foto);
            }

            return View(oProducto);
        }

        #endregion

        #region CATEGORIAS
        [HttpGet]
        public JsonResult ListarCategoria()
        {
            List<Categoria> lista = new List<Categoria>();

            lista = new CN_Categoria().Listar().Select(c => new Categoria()
            {
                id_categoria = c.id_categoria,
                nombre_categoria = c.nombre_categoria,
                estado = c.estado
            }).Where(c =>
                 c.estado == "HABILITADO  "
           ).ToList();

            return Json(new { data = lista }, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region PINTAR PRODUCTOS
        [HttpPost]
        public JsonResult ListarProducto(string idcategoria)
        {
            List<Producto> lista = new List<Producto>();

            bool conversion;

            lista = new CN_Producto().Listar().Select(p => new Producto()
            {
                id_prod = p.id_prod,
                nombre = p.nombre,
                descripcion = p.descripcion,
                oCategoria = p.oCategoria,
                oCatalogo = p.oCatalogo,
                precio = p.precio,
                stock = p.stock,
                rutafoto = p.rutafoto,
                Base64 = CN_Recursos.ConvertirBase64(Path.Combine(p.rutafoto, p.foto), out conversion),
                extension = Path.GetExtension(p.foto),
                estado = p.estado
            }).Where(p =>
                p.oCategoria.id_categoria == (idcategoria == " " ? p.oCategoria.id_categoria : idcategoria) &&
                p.stock > 0 && p.estado == "HABILITADO  "
            ).ToList();

            var jsonresult = Json(new { data = lista }, JsonRequestBehavior.AllowGet);
            jsonresult.MaxJsonLength = int.MaxValue;

            return jsonresult;

        }
        #endregion

        #region LOGICA CARRITO     

        [HttpPost]
        public JsonResult AgregarCarrito(string idproducto)
        {
            string idclienteU = ((Usuario_Admin)Session["USUARIO"]).OclienteUser.id_clienteU;

            bool existe = new CN_Carrito().ExisteCarrito(idclienteU, idproducto);

            bool respuesta = false;

            string mensaje = string.Empty;

            if (existe)
            {
                mensaje = "El producto ya existe en el carrito";
            }
            else
            {
                respuesta = new CN_Carrito().operacionCarrito(idclienteU, idproducto, true, out mensaje);
            }

            return Json(new { respuesta = respuesta, mensaje = mensaje }, JsonRequestBehavior.AllowGet);
        }


        [HttpGet]
        public JsonResult CantidadCarrito()
        {
            string idclienteU = ((Usuario_Admin)Session["USUARIO"]).OclienteUser.id_clienteU;
            int cantidad = new CN_Carrito().CantidadEnCarrito(idclienteU);
            return Json(new { cantidad = cantidad }, JsonRequestBehavior.AllowGet);

        }

        [HttpPost]
        public JsonResult ListarProductoCarrito()
        {

            string idclienteU = ((Usuario_Admin)Session["USUARIO"]).OclienteUser.id_clienteU;
            List<Carrito> olista = new List<Carrito>();
            bool conversion;


            olista = new CN_Carrito().ListarProducto(idclienteU).Select(oc => new Carrito()
            {
                oProducto = new Producto()
                {
                    id_prod = oc.oProducto.id_prod,
                    nombre = oc.oProducto.nombre,
                    precio = oc.oProducto.precio,
                    descripcion = oc.oProducto.descripcion,
                    rutafoto = oc.oProducto.rutafoto,
                    Base64 = CN_Recursos.ConvertirBase64(Path.Combine(oc.oProducto.rutafoto, oc.oProducto.foto), out conversion),
                    extension = Path.GetExtension(oc.oProducto.foto)
                },
                Cantidad = oc.Cantidad,
            }).ToList();

            return Json(new { data = olista }, JsonRequestBehavior.AllowGet);
        }


        [HttpPost]
        public JsonResult OperacionCarrito(string idproducto, bool sumar)
        {
            string idclienteU = ((Usuario_Admin)Session["USUARIO"]).OclienteUser.id_clienteU;
            bool respuesta = false;
            string mensaje = string.Empty;

            respuesta = new CN_Carrito().operacionCarrito(idclienteU, idproducto, sumar, out mensaje);

            return Json(new { respuesta = respuesta, mensaje = mensaje }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult EliminarCarrito(string idproducto)
        {
            string idclienteU = ((Usuario_Admin)Session["USUARIO"]).OclienteUser.id_clienteU;
            bool respuesta = false;
            string mensaje = string.Empty;

            respuesta = new CN_Carrito().EliminarCarrito(idclienteU, idproducto);

            return Json(new { respuesta = respuesta, mensaje = mensaje }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult ObtenerDepartamento()
        {
            List<Departamento> oLista = new List<Departamento>();

            oLista = new CN_Ubicacion().ObtenerDepartamento();
            return Json(new { lista = oLista }, JsonRequestBehavior.AllowGet);


        }

        [HttpPost]
        public JsonResult ObtenerMunicipio(string IdDepartamento)
        {
            List<Municipio> oLista = new List<Municipio>();

            oLista = new CN_Ubicacion().ObtenerMunicipio(IdDepartamento);
            return Json(new { lista = oLista }, JsonRequestBehavior.AllowGet);


        }

        #endregion

        #region Logica VentaTransac

        [HttpPost]
        public async Task<JsonResult> ProcesarPago(List<Carrito> oListaCarrito, VentaTransac oVentaTransac)
        {
            decimal total = 0;
            DataTable detalle_venta = new DataTable();
            detalle_venta.Locale = new CultureInfo("es-NI");
            detalle_venta.Columns.Add("IdProducto", typeof(string));
            detalle_venta.Columns.Add("Cantidad", typeof(int));
            detalle_venta.Columns.Add("Total", typeof(decimal));

            List<Item> oListaItem = new List<Item>();

            foreach(Carrito oCarrito in oListaCarrito)
            {
                decimal subTotal = Convert.ToDecimal(oCarrito.Cantidad.ToString()) * oCarrito.oProducto.precio;
                total += subTotal;

                //oListaItem.Add(new Item(){
                //    name = oCarrito.oProducto.nombre,
                //    quantity = oCarrito.Cantidad.ToString(),
                //    unit_amount = new UnitAmount()
                //    {
                //        currency_code = "USD",
                //        value = oCarrito.oProducto.precio.ToString("G",new CultureInfo("es-NI"))
                //    } 

                //});

                detalle_venta.Rows.Add(new object[]{
                    oCarrito.oProducto.id_prod,
                    oCarrito.Cantidad,
                    subTotal
                });
            }

            total = (total + (total * ((decimal)(0.15))));//aplicamos iva
            total = decimal.Round(total, 2);

            PurchaseUnit purchaseUnit = new PurchaseUnit()
            {
                amount = new Amount()
                {
                    currency_code = "USD",
                    value = total.ToString("G", new CultureInfo("es-NI")),
                    breakdown = new Breakdown()
                    {
                        item_total = new ItemTotal()
                        {
                            currency_code = "USD",
                            value = total.ToString("G", new CultureInfo("es-NI"))
                        }
                    }
                   
                },
                description = "compra de articulo de mi tienda"
            };

            Checkout_Order oCheckoutOrder = new Checkout_Order()
            {
                intent = "CAPTURE",
                purchase_units = new List<PurchaseUnit>() { purchaseUnit },
                application_context = new ApplicationContext()
                {
                    brand_name = "PedidosFerre.com",
                    landing_page = "NO_PREFERENCE",
                    user_action = "PAY_NOW",
                    return_url = "https://localhost:44343/Tienda/PagoEfectuado",
                    cancel_url = "https://localhost:44343/Tienda/Carrito"
                }

            };

           
            oVentaTransac.MontoTotal = total;
            oVentaTransac.id_clienteU = ((Usuario_Admin)Session["USUARIO"]).OclienteUser.id_clienteU;

            TempData["VentaTransac"] = oVentaTransac;
            TempData["DetalleVenta"] = detalle_venta;

            CN_Paypal opaypal = new CN_Paypal();

            Response_Paypal<Response_Checkout> response_paypal = new Response_Paypal<Response_Checkout>();

            response_paypal = await opaypal.CrearSolicitud(oCheckoutOrder);

            return Json(response_paypal, JsonRequestBehavior.AllowGet);

        }

        [ValidarSession]
        [Authorize]
        public async Task<ActionResult> PagoEfectuado()
        {
            string token = Request.QueryString["token"];

            CN_Paypal opaypal = new CN_Paypal();

            Response_Paypal<Response_Capture> response_paypal = new Response_Paypal<Response_Capture>();

            response_paypal = await opaypal.AprobarPago(token);


            ViewData["Status"] = response_paypal.Status;

            if (response_paypal.Status)
            {
                VentaTransac oVentaTransac = (VentaTransac)TempData["VentaTransac"];

                DataTable detalle_venta = (DataTable)TempData["DetalleVenta"];

                oVentaTransac.IdTransaccion = response_paypal.Response.purchase_units[0].payments.captures[0].id;

                string mensaje = string.Empty;

                bool respuesta = new CN_VentaTransac().Registrar(oVentaTransac, detalle_venta, out mensaje);

                ViewData["idTransaccion"] = oVentaTransac.IdTransaccion;
            }

            return View();

        }

        #endregion

        #region Listar Compras cliente
        [ValidarSession]
        [Authorize]
        public ActionResult MisCompras()
        {

            string idclienteU = ((Usuario_Admin)Session["USUARIO"]).OclienteUser.id_clienteU;
            List<DetalleVentaTransac> olista = new List<DetalleVentaTransac>();
            bool conversion;


            olista = new CN_VentaTransac().ListarCompras(idclienteU).Select(oc => new DetalleVentaTransac()
            {
                oProducto = new Producto()
                {
                    nombre = oc.oProducto.nombre,
                    precio = oc.oProducto.precio,
                    Base64 = CN_Recursos.ConvertirBase64(Path.Combine(oc.oProducto.rutafoto, oc.oProducto.foto), out conversion),
                    extension = Path.GetExtension(oc.oProducto.foto)
                },
                Cantidad = oc.Cantidad,
                Total = oc.Total,
                IdTransaccion = oc.IdTransaccion
            }).ToList();

            return View(olista);
        }

        #endregion
    }
}
