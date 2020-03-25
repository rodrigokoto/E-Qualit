using DAL.Repository;
using Dominio.Interface.Repositorio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Web.UI.Helpers
{
    public class EditarPossuiAcessoSite : AuthorizeAttribute
    {

        private readonly IUsuarioClienteSiteRepositorio _usuarioClienteSiteRepositorio = new UsuarioClienteSiteRepositorio();

        public EditarPossuiAcessoSite()
        {
        }

        public override void OnAuthorization(AuthorizationContext filterContext)
        {

            var httpContext = filterContext.RequestContext.HttpContext;
            var request = httpContext.Request;

            base.OnAuthorization(filterContext);
        }

        protected override bool AuthorizeCore(HttpContextBase httpContext)
        {

            try
            {
                var routeData = httpContext.Request.RequestContext.RouteData;
                string action = routeData.GetRequiredString("action");
                string controller = routeData.GetRequiredString("controller");

            }
            catch (Exception)
            {

                throw;
            }

            return true;
        }

        protected override void HandleUnauthorizedRequest(AuthorizationContext filterContext)
        {
            filterContext.Result = new RedirectResult("/Home/BloqueioModulo");
        }
    }
}