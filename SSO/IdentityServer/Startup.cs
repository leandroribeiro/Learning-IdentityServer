using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Web;
using System.Web.Http;
using IdentityServer3.AccessTokenValidation;
using IdentityServer3.Core.Configuration;
using Owin;

namespace IdentityServer
{

    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            var config = new HttpConfiguration();
            WebApiConfig.Register(config);

            app.UseIdentityServerBearerTokenAuthentication(new IdentityServerBearerTokenAuthenticationOptions
            {
                //Authority = "http://localhost:51761/",//Your url here
                Authority = "https://localhost:44300/",
                RequiredScopes = new[] { "api" }
            });


            app.UseWebApi(config);

            var factory = new IdentityServerServiceFactory()
                .UseInMemoryUsers(Config.GetUsers())
                .UseInMemoryClients(Config.GetClients())
                .UseInMemoryScopes(Config.GetScopes());

            app.UseIdentityServer(new IdentityServerOptions
            {
                IssuerUri = "urn:identity",
                Factory = factory,
                //RequireSsl = false, //DO NOT DO THIS IN PRODUCTION

                LoggingOptions = new LoggingOptions
                {
                    EnableWebApiDiagnostics = true,
                    WebApiDiagnosticsIsVerbose = true
                },


                SigningCertificate = LoadCertificate()
            });

            //app.UseNLog();

        }

        X509Certificate2 LoadCertificate()
        {
            return new X509Certificate2(
                string.Format(@"{0}\bin\idsrv3test.pfx", AppDomain.CurrentDomain.BaseDirectory), "idsrv3test");
        }

    }
}