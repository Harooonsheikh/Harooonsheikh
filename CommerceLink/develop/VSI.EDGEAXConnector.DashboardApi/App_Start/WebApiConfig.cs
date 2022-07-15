using Microsoft.Net.Http.Headers;
using System;
using System.Net.Http.Formatting;
using System.Web.Http;
using System.Web.Services.Description;
using VSI.EDGEAXConnector.DashboardApi.ActionFilters;
using VSI.EDGEAXConnector.DashboardApi.Common;

namespace VSI.EDGEAXConnector.DashboardApi
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
            // Web API configuration and services

            //TODO: Commented for testing only 
            //config.Filters.Add(new HttpsAttribute());
            config.Filters.Add(new AuthenticationAttribute());
            config.Filters.Add(new SanitizeInputAttribute());

            config.Formatters.Clear();
            HttpConfiguration httpConfig = new HttpConfiguration();
            config.Formatters.Add(httpConfig.Formatters.JsonFormatter);

            // Web API routes
            config.MapHttpAttributeRoutes();
            var route = config.Routes.MapHttpRoute(
                name: "dashboardApi",
                routeTemplate: "api/{controller}/{action}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );
            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "{controller}/{action}",
                defaults: new { controller = "Home", action = "Index" }
            );
            //            route.DataTokens["Namespaces"] = new string[] { "VSI.EDGEAXConnector.DashboardApi" };
        }

    }
}
