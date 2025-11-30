using System.Web.Http;
using WebActivatorEx;
using RestApi;
using Swashbuckle.Application;
using System.Configuration;

[assembly: PreApplicationStartMethod(typeof(SwaggerConfig), "Register")]

namespace RestApi
{
    public class SwaggerConfig
    {
        public static void Register()
        {
            bool enableSwagger = bool.Parse(ConfigurationManager.AppSettings["EnableSwagger"] ?? "false");
            if (!enableSwagger) return; // 운영에서는 Swagger 등록 안 함

            var thisAssembly = typeof(SwaggerConfig).Assembly;

            GlobalConfiguration.Configuration
            .EnableSwagger(c =>
            {
                c.SingleApiVersion("v1", "RestApi");

                c.ApiKey("Bearer")
                 .Description("JWT token. Example: Bearer {token}")
                 .Name("Authorization")
                 .In("header");
            })
            .EnableSwaggerUi(c =>
            {
                c.EnableApiKeySupport("Authorization", "header");
            });
        }
    }
}
