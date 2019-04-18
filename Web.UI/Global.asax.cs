using Newtonsoft.Json;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace Web.UI
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            //configura retorno json
            GlobalConfiguration.Configuration.Formatters.JsonFormatter.SerializerSettings
            .ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
                    GlobalConfiguration.Configuration.Formatters.JsonFormatter.SerializerSettings
            .PreserveReferencesHandling = PreserveReferencesHandling.Objects;

                    GlobalConfiguration.Configuration.Formatters.Clear();

        }
    }
}
