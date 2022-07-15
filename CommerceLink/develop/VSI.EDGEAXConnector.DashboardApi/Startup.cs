using Autofac;
using Autofac.Integration.WebApi;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Owin;
using Microsoft.Owin.Cors;
using Microsoft.Owin.Diagnostics;
using Microsoft.Owin.Security.DataHandler.Encoder;
using Microsoft.Owin.Security.Jwt;
using Microsoft.Owin.Security.OAuth;
using Newtonsoft.Json.Serialization;
using Owin;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net.Http.Formatting;
using System.Reflection;
using System.Web.Configuration;
using System.Web.Http;
using Newtonsoft.Json;
using VSI.EDGEAXConnector.DashboardApi.CustomMessageHandler;
using VSI.EDGEAXConnector.DashboardApi.Infrastructure;
using VSI.EDGEAXConnector.DashboardApi.Providers;
using AuthenticationMode = Microsoft.Owin.Security.AuthenticationMode;

[assembly: OwinStartup(typeof(VSI.EDGEAXConnector.DashboardApi.Startup))]

namespace VSI.EDGEAXConnector.DashboardApi
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
            app.UseCors(Microsoft.Owin.Cors.CorsOptions.AllowAll);
            ConfigureOAuthTokenGeneration(app);
            ConfigureOAuthTokenConsumption(app);

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

            ConfigureWebApi(config);
            // Swagger
            //SwaggerConfig.Register(config);
            // Register routes
            WebApiConfig.Register(config);

            //var builder = new ContainerBuilder();

            //builder.RegisterApiControllers(Assembly.GetExecutingAssembly());

            //// Run other optional steps, like registering filters,
            //// per-controller-type services, etc., then set the dependency resolver
            //// to be Autofac.
            //var erpAssembly = Assembly.Load(ConfigurationManager.AppSettings["ErpAdapter"]);
            //var ecomAssembly = Assembly.Load(ConfigurationManager.AppSettings["EcomAdapter"]);

            //builder.RegisterAssemblyModules(erpAssembly);
            //builder.RegisterAssemblyModules(ecomAssembly);
            //builder.RegisterType<ErpAdapterFactory>().As<IErpAdapterFactory>().InstancePerRequest();
            //builder.RegisterType<EcomAdapterFactory>().As<IEComAdapterFactory>().InstancePerRequest();

            //var container = builder.Build();
            //var diResolver = new AutofacWebApiDependencyResolver(container);
            //config.DependencyResolver = diResolver;

            ////OWIN WEB API SETUP:

            //// Register the Autofac middleware FIRST, then the Autofac Web API middleware,
            //// and finally the standard Web API middleware.
            //app.UseAutofacMiddleware(container);
            app.UseAutofacWebApi(config);

            app.UseWebApi(config);

            
        }
        private void ConfigureOAuthTokenGeneration(IAppBuilder app)
        {
            // Configure the db context and user manager to use a single instance per request
            app.CreatePerOwinContext(ApplicationDbContext.Create);
            app.CreatePerOwinContext<ApplicationUserManager>(ApplicationUserManager.Create);
            app.CreatePerOwinContext<ApplicationRoleManager>(ApplicationRoleManager.Create);

            OAuthAuthorizationServerOptions OAuthServerOptions = new OAuthAuthorizationServerOptions()
            {
                //For Dev enviroment only (on production should be AllowInsecureHttp = false)
                AllowInsecureHttp = true,
                TokenEndpointPath = new PathString("/oauth/token"),
                AccessTokenExpireTimeSpan = TimeSpan.FromHours(Convert.ToInt32(ConfigurationManager.AppSettings["SeesionTimeoutDurationInHours"])), //TimeSpan.FromDays(1),
                Provider = new CustomOAuthProvider(),
                AccessTokenFormat = new CustomJwtFormat(ConfigurationManager.AppSettings["url"])
            };

            // OAuth 2.0 Bearer Access Token Generation
            app.UseOAuthAuthorizationServer(OAuthServerOptions);
        }

        private void ConfigureOAuthTokenConsumption(IAppBuilder app)
        {

            var issuer = ConfigurationManager.AppSettings["url"];
            string audienceId = ConfigurationManager.AppSettings["as:AudienceId"];
            byte[] audienceSecret = TextEncodings.Base64Url.Decode(ConfigurationManager.AppSettings["as:AudienceSecret"]);

            // Api controllers with an [Authorize] attribute will be validated with JWT
            app.UseJwtBearerAuthentication(
                new JwtBearerAuthenticationOptions
                {
                    AuthenticationMode = AuthenticationMode.Active,
                    AllowedAudiences = new[] { audienceId },
                    IssuerSecurityTokenProviders = new IIssuerSecurityTokenProvider[]
                    {
                        new SymmetricKeyIssuerSecurityTokenProvider(issuer, audienceSecret)
                    }
                });
        }

        private void ConfigureWebApi(HttpConfiguration config)
        {
           // config.MapHttpAttributeRoutes();

            var jsonFormatter = config.Formatters.OfType<JsonMediaTypeFormatter>().First();
            jsonFormatter.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
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
