using System;
using System.Web;
using System.Web.Mvc;
using System.Linq;

namespace Web.UI.Helpers
{
    public class SitePossuiModuloAttribute : AuthorizeAttribute
    {
        private readonly int _idModulo;

        public SitePossuiModuloAttribute(int idModulo)
        {
            _idModulo = idModulo;
        }

        protected override bool AuthorizeCore(HttpContextBase httpContext)
        {
            try
            {
                var idSiteSelecionado = Util.ObterSiteSelecionado();

                var sitesModulos = Util.ObterSiteModuloSelecionado();

                var total = sitesModulos.Where(x => x.IdSite == idSiteSelecionado && x.IdFuncionalidade == _idModulo).Count();

                if (total > 0)
                {
                    return true;
                }

                return false;
            }
            catch (Exception)
            {
                return false;
            }
        }

        protected override void HandleUnauthorizedRequest(AuthorizationContext filterContext)
        {
            filterContext.Result = new RedirectResult("/Home/BloqueioModulo");
        }
    }
}