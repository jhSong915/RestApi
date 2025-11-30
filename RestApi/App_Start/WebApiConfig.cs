using RestApi.Handlers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;

namespace RestApi
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            // Web API 구성 및 서비스

            // Web API 경로
            config.MapHttpAttributeRoutes();

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/v1/{controller}"
            );
            config.MessageHandlers.Add(new JwtValidationHandler());

        }
    }
}
