using Dominio.Enumerado;
using System.Web;
using System.Web.Mvc;

namespace Web.UI.Helpers
{
    public class AcessoCliente : AuthorizeAttribute
    {
        protected override bool AuthorizeCore(HttpContextBase httpContext)
        {
            var IdPerfil = Util.ObterPerfilUsuarioLogado();
            if (IdPerfil == (int)PerfisAcesso.Administrador || IdPerfil == (int)PerfisAcesso.Suporte)
            {
                return true;
            }

            return false;
        }

        protected override void HandleUnauthorizedRequest(AuthorizationContext filterContext)
        {
            filterContext.Result = new RedirectResult("/Home/Index");
        }
    }
}