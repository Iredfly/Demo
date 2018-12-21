using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using System.Web.Http.Cors;

namespace DiYi.Demo.Api
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            // Web API 配置和服务

            // Web API 路由
            config.MapHttpAttributeRoutes();
            // Web API 配置和服务

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{action}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );
            // config.Filters.Add(new ApiExceptionAttribute());
            //config.Filters.Add(new APILogAttribute());
            config.EnableCors(new EnableCorsAttribute("*", "*", "*"));
        }
    }
}
