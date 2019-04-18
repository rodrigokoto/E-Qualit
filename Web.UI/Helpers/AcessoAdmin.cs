using Dominio.Enumerado;
using System.Web;
using System.Web.Mvc;

namespace Web.UI.Helpers
{
    public class AcessoAdmin : AuthorizeAttribute
    {
        protected override bool AuthorizeCore(HttpContextBase httpContext)
        {
            var IdPerfil = Util.ObterPerfilUsuarioLogado();
            if (IdPerfil == (int)PerfisAcesso.Administrador)
            {
                return true;
            }

            return false;
        }

        protected override void HandleUnauthorizedRequest(AuthorizationContext filterContext)
        {
            filterContext.Result = new RedirectResult("/Home");
        }
    }
}