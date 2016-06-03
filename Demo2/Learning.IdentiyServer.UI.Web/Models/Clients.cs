using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using IdentityServer3.Core.Models;

namespace Learning.IdentiyServer.UI.Web.Models
{
    public static class Clients
    {
        public static IEnumerable<Client> Get()
        {
            return new[]
            {
                new Client
                {
                    Enabled = true,
                    ClientName = "MVC Client",
                    ClientId = "mvc",
                    Flow = Flows.Implicit,
                    RedirectUris = new List<string>
                    {
                        "https://localhost:44301/"
                    }
                    ,
                    PostLogoutRedirectUris = new List<string>
                    {
                        "https://localhost:44301/"
                    },
                    AllowAccessToAllScopes = true
                }
            };
        }
    }
}