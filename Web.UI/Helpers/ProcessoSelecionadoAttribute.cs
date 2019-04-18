using System;
using System.Web;
using System.Web.Mvc;

namespace Web.UI.Helpers
{
    public class ProcessoSelecionadoAttribute : AuthorizeAttribute
    {
        protected override bool AuthorizeCore(HttpContextBase httpContext)
        {
            try
            {
                string cookieProcesso = Convert.ToString(Util.ObterProcessoSelecionado());

                if (string.IsNullOrEmpty(cookieProcesso) || cookieProcesso == "0")
                {
                    return false;
                }
                else
                {
                    return true;
                }
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        protected override void HandleUnauthorizedRequest(AuthorizationContext filterContext)
        {
            filterContext.Result = new RedirectResult("/Home/BloqueioProcesso");
        }

    }
}