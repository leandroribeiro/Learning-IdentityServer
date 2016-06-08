using System.Collections.Generic;
using IdentityServer3.Core.Models;

namespace Learning.IdentityServer.Demo3.AuthServer
{
    public static class Scopes
    {
        public static List<Scope> Get()
        {
            return new List<Scope>
            {
                StandardScopes.OpenId,
                StandardScopes.Profile,
                //New scope
                StandardScopes.Email
            };
        }
    }
}