using System.Web.Http;
using WebActivatorEx;
using XSIS.Shop.WebApi;
using System.Linq;
using Swashbuckle.Application;

[assembly: PreApplicationStartMethod(typeof(SwaggerConfig), "Register")]

namespace XSIS.Shop.WebApi
{
    public class SwaggerConfig
    {
        public static void Register()
        {
            var thisAssembly = typeof(SwaggerConfig).Assembly;

            GlobalConfiguration.Configuration
                .EnableSwagger(c =>
                    {
                        c.SingleApiVersion("v1", "XSIS.Shop.WebApi");
                        c.ResolveConflictingActions(apiDescription => apiDescription.First());
                    })
                .EnableSwaggerUi(c =>
                    {

                    });
        }
    }
}
