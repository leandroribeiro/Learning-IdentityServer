using System.Collections.Generic;
using System.Linq;
using System.Web;
using IdentityServer3.Core.Models;

namespace Learning.IdentityServer.Demo3.AuthServer
{
    public class Clients
    {
        public static IEnumerable<Client> Get()
        {
            return new[]
            {
                new Client
                {
                    Enabled = true,
                    ClientName = "JS Client",
                    ClientId = "js",
                    Flow = Flows.Implicit,

                    RedirectUris = new List<string>
                    {
                        "http://localhost:45793/popup.html"
                    },

                    AllowedCorsOrigins = new List<string>
                    {
                        "http://localhost:45793"
                    },

                    AllowAccessToAllScopes = true
                }
            };
        }
    }//class

    //class

    //class

    //class

}