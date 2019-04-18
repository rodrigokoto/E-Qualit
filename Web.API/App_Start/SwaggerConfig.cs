using System.Web.Http;
using WebActivatorEx;
using Web.API;
using Swashbuckle.Application;
using System;

[assembly: PreApplicationStartMethod(typeof(SwaggerConfig), "Register")]

namespace Web.API
{
    public class SwaggerConfig
    {
        public static void Register()
        {
            var thisAssembly = typeof(SwaggerConfig).Assembly;

            GlobalConfiguration.Configuration
                .EnableSwagger(c =>
                {
                    c.SingleApiVersion("v1", "Swagger Sample");
                    c.IncludeXmlComments(GetXmlCommentsPath());
                })
                .EnableSwaggerUi(c =>
                {

                });

        }

        protected static string GetXmlCommentsPath()
        {
            return string.Format(@"{0}\bin\Swagger.XML", AppDomain.CurrentDomain.BaseDirectory);
        }
    }
}
