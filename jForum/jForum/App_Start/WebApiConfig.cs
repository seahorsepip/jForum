using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Web.Http;
using System.Web.Http.Cors;

namespace jForum
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            /*
            var cors = new EnableCorsAttribute("http://jpopup.seapip.com", "*", "*");
            config.EnableCors(cors);
            */

            config.MapHttpAttributeRoutes();

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "{controller}/{id}",
                defaults: new {
                    id = RouteParameter.Optional
                }
            );
            config.Formatters.JsonFormatter.SupportedMediaTypes.Add(new MediaTypeHeaderValue("application/json"));
        }
    }
}
