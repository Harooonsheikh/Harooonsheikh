using Microsoft.Net.Http.Headers;
using System;
using System.Configuration;
using System.Linq;
using System.Net.Http.Formatting;
using System.Web.Http;
using System.Web.Services.Description;
using VSI.EDGEAXConnector.Configuration;
using VSI.EDGEAXConnector.Data;
using VSI.EDGEAXConnector.Web.Common;

namespace VSI.EDGEAXConnector.Web
{
    /// <summary>
    /// Configure Web API.
    /// </summary>
    public static class WebApiConfig
    {
        /// <summary>
        /// Register filters and routes.
        /// </summary>
        /// <param name="config"></param>
        public static void Register(HttpConfiguration config)
        {
            //TODO: Load LogMapper data

            LogMapperHelper.LoadLogMappers();
            LogMapperHelper.SetIsLogginEnabled(ConfigurationManager.AppSettings["IsLoggingEnable"]);
            StoreService.GetAllActiveStores();
            ConfigurationHelper.LoadAllConfigurations();

            // Web API configuration and services

            //TODO: Commented for testing only 
            //config.Filters.Add(new HttpsAttribute());
            //config.Filters.Add(new AuthenticationAttribute());

            config.Formatters.Clear();
            HttpConfiguration httpConfig = new HttpConfiguration();
            config.Formatters.Add(httpConfig.Formatters.JsonFormatter);

            // Web API routes
            config.MapHttpAttributeRoutes();
            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "{controller}/{action}",
                defaults: new { controller="Home", action="Index"}
            );
        }

    }
}
