using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IdentityServer3.Core.Configuration;
using Microsoft.Owin;
using Owin;

[assembly: OwinStartup(typeof(Learning.IdentityServer.UI.Console.Startup))]
namespace Learning.IdentityServer.UI.Console
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            var options = new IdentityServerOptions()
            {
                Factory = new IdentityServerServiceFactory()
                    .UseInMemoryClients(Clients.Get())
                    .UseInMemoryScopes(Scopes.Get())
                    .UseInMemoryUsers(Users.Get()),

                RequireSsl = false
            };

            app.UseIdentityServer(options);
        }
    }
}
