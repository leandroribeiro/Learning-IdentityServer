using System;
using System.IO;
using System.Security.Cryptography.X509Certificates;
using IdentityServer3.Core.Configuration;
using Owin;

namespace Learning.IdentityServer.Demo3.AuthServer
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            app.UseIdentityServer(new IdentityServerOptions
            {
                SiteName = "Embedded IdentityServer",
                SigningCertificate = LoadCertificate(),

                Factory = new IdentityServerServiceFactory()
                    .UseInMemoryUsers(Users.Get())
                    .UseInMemoryClients(Clients.Get())
                    .UseInMemoryScopes(Scopes.Get())

            });
        }

        private X509Certificate2 LoadCertificate()
        {
            var path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"bin\Config\idsrv3test.pfx");

            return 
                new X509Certificate2(path, "idsrv3test");
        }
    }
}