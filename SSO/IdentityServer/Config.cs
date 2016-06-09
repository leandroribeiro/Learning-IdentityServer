using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using IdentityServer3.Core.Models;
using IdentityServer3.Core.Services.InMemory;

namespace IdentityServer
{
    public class Config
    {
        public static List<InMemoryUser> GetUsers()
        {
            return new List<InMemoryUser>
            {
                new InMemoryUser
                {
                    Subject = "admin", // this is a unique identifier
                    Username = "admin",
                    Password = "pass"
                }
            };
        }

        public static IEnumerable<Client> GetClients()
        {
            var clients = new List<Client>();
            clients.Add(new Client
            {
                ClientName = "TestClient",
                ClientId = "test",
                Flow = Flows.Implicit,
                ClientSecrets = new List<Secret>
                    {
                        new Secret("secret".Sha256())
                    },
                RedirectUris = new List<string>
                {
                    "Http://example.local/" //using a fake url for now
                }

            });
            return clients;
        }

        public static IEnumerable<Scope> GetScopes()
        {
            var scopes = new List<Scope>();
            scopes.Add(new Scope
            {
                DisplayName = "Api Access",
                Name = "api",
                Type = ScopeType.Resource
            });
            return scopes;
        }


    }
}