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
            var router = request.RequestContext.RouteData;
            string action = router.GetRequiredString("action");
            string controller = router.GetRequiredString("controller");
            var idSite = 0;
            var id = 0;
            if (action == "Editar")
            {
                switch (controller)
                {
                    case "AnaliseCritica":
                        idSite = Convert.ToInt32(request.Params["siteSelecionado"]);
                        break;
                    case "ControlDoc":
                        idSite = Convert.ToInt32(request.Params["siteSelecionado"]);
                        break;
                    case "Instrumento":
                        idSite = Convert.ToInt32(request.Params["siteSelecionado"]);
                        break;
                    case "GestaoDeRisco":
                        idSite = Convert.ToInt32(request.Params["siteSelecionado"]);
                        break;
                    case "Licenca":
                        idSite = Convert.ToInt32(request.Params["siteSelecionado"]);
                        break;
                    default:
                        break;
                }
            }
            

            //ilterContext.Result = new RedirectResult("/Home/BloqueioUnauthorized");

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
            filterContext.Result = new RedirectResult("/Home/BloqueioUnauthorized");
        }
    }
}