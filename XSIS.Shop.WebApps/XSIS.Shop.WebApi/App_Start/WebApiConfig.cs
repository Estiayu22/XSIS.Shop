using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;

namespace XSIS.Shop.WebApi
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            // Web API configuration and services

            // Web API routes
            config.MapHttpAttributeRoutes();

            config.Routes.MapHttpRoute(
                name: "CustomerApi",
                routeTemplate: "api/CustomerApi/{id}",
                defaults: new { id = RouteParameter.Optional }
            );

            config.Routes.MapHttpRoute(
               name: "CustomerApi1Param",
               routeTemplate: "api/CustomerApi/{action}/{id}",
               defaults: new { action = "get", id = RouteParameter.Optional }
            );

            config.Routes.MapHttpRoute(
              name: "CustomerApi2Param",
              routeTemplate: "api/CustomerApi/{action}/{id}/{id2}",
              defaults: new { action = "get", id = RouteParameter.Optional, id2 = RouteParameter.Optional }
            );

            config.Routes.MapHttpRoute(
                name: "ProductApi",
                routeTemplate: "api/Product/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );
        }
    }
}
