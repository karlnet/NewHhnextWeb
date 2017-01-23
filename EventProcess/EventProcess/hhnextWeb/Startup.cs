using System;
using System.Threading.Tasks;
using Microsoft.Owin;
using Owin;
using Microsoft.AspNet.SignalR;
using System.Web.Http;
using System.Net.Http.Formatting;
using Newtonsoft.Json.Serialization;
using System.Linq;
using hhnextWeb.Data.Entities.Mapping;
using hhnextWeb.Data.Entities;
using Microsoft.Owin.Security.Jwt;
using Microsoft.Owin.Security.OAuth;
using hhnextWeb.Filters;
using hhnextWeb.Providers;
using System.Configuration;
using Microsoft.Owin.Security.DataHandler.Encoder;
using hhnextWeb.Formats;
using hhnextWeb.Data;
using hhnextWeb.Data.Infrastructure;
using Microsoft.Owin.Security;
using Microsoft.Practices.Unity;
using hhnextWeb.Resolvers;

[assembly: OwinStartup(typeof(hhnextWeb.Startup))]

namespace hhnextWeb
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {

#if DEBUG
            app.UseErrorPage();
#endif
            //GlobalHost.DependencyResolver.UseRedis("hhnext.redis.cache.windows.net", 6379, "2a8N8qu0H86t4UmXhWTwCjdmGq0EUZHYBqsCgB23j5Q=", "test");

            app.MapSignalR();

            //app.Run(context =>
            //{
            //    context.Response.ContentType = "text/plain";
            //    return context.Response.WriteAsync("Hello, world.");
            //});

            HttpConfiguration httpConfig = new HttpConfiguration();

            ConfigureOAuthTokenGeneration(app);

            //ConfigureOAuthTokenConsumption(app);

            ConfigureWebApi(httpConfig);

            app.UseCors(Microsoft.Owin.Cors.CorsOptions.AllowAll);

            app.UseWebApi(httpConfig);


        }

        private void ConfigureOAuthTokenConsumption(IAppBuilder app)
        {

            var issuer = "http://hhnext.com";
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

        private void ConfigureOAuthTokenGeneration(IAppBuilder app)
        {
            // Configure the db context and user manager to use a single instance per request
            app.CreatePerOwinContext(AppDbContext.Create);
            app.CreatePerOwinContext<AppUserManager>(AppUserManager.Create);
            app.CreatePerOwinContext<AppRoleManager>(AppRoleManager.Create);

            OAuthAuthorizationServerOptions OAuthServerOptions = new OAuthAuthorizationServerOptions()
            {
                //For Dev enviroment only (on production should be AllowInsecureHttp = false)
                AllowInsecureHttp = true,
                TokenEndpointPath = new PathString("/oauth/token"),
                AccessTokenExpireTimeSpan = TimeSpan.FromDays(1),
                Provider = new CustomOAuthProvider(),
                AccessTokenFormat = new CustomJwtFormat("http://hhnext.com")
            };

            // OAuth 2.0 Bearer Access Token Generation
            app.UseOAuthAuthorizationServer(OAuthServerOptions);
        }


        private void ConfigureWebApi(HttpConfiguration config)
        {
            config.MapHttpAttributeRoutes();

            config.Filters.Add(new ValidateModelAttribute());

            var jsonFormatter = config.Formatters.OfType<JsonMediaTypeFormatter>().First();
            jsonFormatter.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
            jsonFormatter.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;
            config.Formatters.Remove(config.Formatters.XmlFormatter);

            //Web API  services
            var container = new UnityContainer();
            container.RegisterType<IRepository, Repository>(new HierarchicalLifetimeManager());

            config.DependencyResolver = new UnityResolver(container);
        }


    }
}
