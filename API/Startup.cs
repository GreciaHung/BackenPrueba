using Microsoft.Owin;
using Microsoft.Owin.Cors;
using Microsoft.Owin.Security.OAuth;
using Owin;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Web.Http;
using System.Web.Http.Cors;
using System.Web.Routing;
using API.Core;
using API.Models;
using API.App_Start;
using FluentValidation.WebApi;

[assembly: OwinStartup(typeof(API.Startup))]
namespace API
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            app.UseCors(CorsOptions.AllowAll);

            ConfigureOAuth(app);
            ConfigureWebApi(app);
        }

        public void ConfigureWebApi(IAppBuilder app)
        {
            var config = new HttpConfiguration();
            config.IncludeErrorDetailPolicy = IncludeErrorDetailPolicy.Always;
            config.MapHttpAttributeRoutes();

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );

            config.Filters.Add(new ValidateModelStateFilter());

            config.EnableCors(new EnableCorsAttribute(
                "*",
                "*",
                "*",
                "DataServiceVersion, MaxDataServiceVersion" 
            ));

            FluentValidationModelValidatorProvider.Configure(config);

            app.UseWebApi(config);
        }

        public void ConfigureOAuth(IAppBuilder app)
        {
            OAuthAuthorizationServerOptions OAuthServerOptions = new OAuthAuthorizationServerOptions()
            {

                AllowInsecureHttp = true,
                TokenEndpointPath = new PathString("/token"),
                AccessTokenExpireTimeSpan = TimeSpan.FromHours(12),
                Provider = new OAuth2.SimpleAuthorizationServerProvider(),
                RefreshTokenProvider = new OAuth2.SimpleRefreshTokenProvider()               
            };

            // Token Generation
            app.UseOAuthAuthorizationServer(OAuthServerOptions);
            app.UseOAuthBearerAuthentication(new OAuthBearerAuthenticationOptions
            {
                Provider = new OAuth2.MixedOAuthBearerProvider()
            });
        }
    }
}
