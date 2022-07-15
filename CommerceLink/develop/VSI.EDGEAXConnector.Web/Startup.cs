using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Owin;
using Microsoft.Owin.Cors;
using Microsoft.Owin.Diagnostics;
using Owin;
using System.Configuration;
using System.Reflection;
using System.Web.Http;
using VSI.EDGEAXConnector.Web.Common;
using System.Web.Configuration;

[assembly: OwinStartup(typeof(VSI.EDGEAXConnector.Web.Startup))]

namespace VSI.EDGEAXConnector.Web
{
    /// <summary>
    /// OWIN Startup
    /// </summary>
    public class Startup
    {
        private IHostingEnvironment env;

        /// <summary>
        /// Default parameterless constructor
        /// </summary>
        public Startup()
        {

        }


        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="env"></param>
        public Startup(IHostingEnvironment env)
        {
            this.env = env;
        }

        /// <summary>
        /// Configure services before 'Configuration'
        /// </summary>
        /// <param name="services"></param>
        public void ConfigureServices(IServiceCollection services)
        {

        }

        /// <summary>
        /// Sets the OWIN configuration
        /// </summary>
        /// <param name="app"></param>
        public void Configuration(IAppBuilder app)
        {
            // ASPNETCORE_ENVIRONMENT should be set to 'Development'
            if (env != null && env.IsDevelopment())
            {
                app.UseErrorPage(ErrorPageOptions.ShowAll);
            }
            
            var config = new HttpConfiguration()
            {
                IncludeErrorDetailPolicy = GetErrorDetailPolicy()
            };

            config.MessageHandlers.Add(new WebApiCustomMessageHandler());
            // Swagger
            //SwaggerConfig.Register(config);
            // Register routes
            WebApiConfig.Register(config);



            // Run other optional steps, like registering filters,
            // per-controller-type services, etc., then set the dependency resolver
            // to be Autofac.
            var erpAssembly = Assembly.Load(ConfigurationManager.AppSettings["ErpAdapter"]);
            var ecomAssembly = Assembly.Load(ConfigurationManager.AppSettings["EcomAdapter"]);
            

            // OWIN WEB API SETUP:

            // Register the Autofac middleware FIRST, then the Autofac Web API middleware,
            // and finally the standard Web API middleware.
            

            app.UseWebApi(config);

            app.UseCors(CorsOptions.AllowAll);
        }

        private IncludeErrorDetailPolicy GetErrorDetailPolicy()
        {
            var config = (CustomErrorsSection)ConfigurationManager.GetSection("system.web/customErrors");

            IncludeErrorDetailPolicy errorDetailPolicy;

            switch (config.Mode)
            {
                case CustomErrorsMode.RemoteOnly:
                    errorDetailPolicy
                        = IncludeErrorDetailPolicy.LocalOnly;
                    break;
                case CustomErrorsMode.On:
                    errorDetailPolicy
                        = IncludeErrorDetailPolicy.Never;
                    break;
                case CustomErrorsMode.Off:
                    errorDetailPolicy
                        = IncludeErrorDetailPolicy.Always;
                    break;
                default:
                    errorDetailPolicy
                        = IncludeErrorDetailPolicy.LocalOnly;
                    break;
            }

            GlobalConfiguration.Configuration.IncludeErrorDetailPolicy
                = errorDetailPolicy;
            return errorDetailPolicy;
        }
    }
}
