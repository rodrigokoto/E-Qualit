using System.Web;
using System.Web.Mvc;

namespace Web.UI.Helpers
{
    public class VerificaIntegridadeLoginAttribute : AuthorizeAttribute
    {
        protected override bool AuthorizeCore(HttpContextBase httpContext)
        {
            if (httpContext.Request.Cookies.Count > 5)
            {
                return true;
            }

            return false;
        }

        protected override void HandleUnauthorizedRequest(AuthorizationContext filterContext)
        {
            filterContext.Result = new RedirectResult("/Login");
        }
    }
}