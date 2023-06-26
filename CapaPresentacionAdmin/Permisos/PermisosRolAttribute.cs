using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CapaEntidad;
using CapaNegocio;

namespace CapaPresentacionAdmin.Permisos
{
    public class PermisosRolAttribute : ActionFilterAttribute
    {
        private int idModulo;
        public PermisosRolAttribute(int _idModulo)
        {
            idModulo = _idModulo;
        }

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            if(HttpContext.Current.Session["ADMIN"] != null)
            {
                Usuario_Admin admin = HttpContext.Current.Session["ADMIN"] as Usuario_Admin;
                RolPorModulo oRolxMod = new RolPorModulo();

                oRolxMod = new CN_Rol().ListarRolPorModulo().Where(R => R.IdRol == admin.ORol.IdRol && R.IdModulo == idModulo).FirstOrDefault();

                if (oRolxMod.Estado == 0)
                {
                    filterContext.Result = new RedirectResult("~/Home/Index");
                }


            }
            else
            {
                filterContext.Result = new RedirectResult("~/Acceso/Index");
               
            }
            
            base.OnActionExecuting(filterContext);
        }


    }
}