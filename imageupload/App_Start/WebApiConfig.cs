using System.Web.Http;
using System.Collections.Generic;
using System.Linq;

class WebApiConfig
{
    public static void Register(HttpConfiguration configuration)
    {
        configuration.Routes.MapHttpRoute("API Default", "api/{controller}/{id}",
            new { id = RouteParameter.Optional });

        var appXmlType = configuration.Formatters.XmlFormatter.SupportedMediaTypes.FirstOrDefault(t => t.MediaType == "application/xml");
        configuration.Formatters.XmlFormatter.SupportedMediaTypes.Remove(appXmlType);
    }
}


