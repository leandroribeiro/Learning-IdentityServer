﻿using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using System.Web;
using System.Web.Configuration;
using System.Web.Helpers;
using IdentityServer3.Core;
using IdentityServer3.Core.Configuration;
using IdentityServer3.Core.Models;
using Learning.IdentiyServer.UI.Web.Models;
using Microsoft.IdentityModel.Protocols;
using Microsoft.Owin.Security;
using Microsoft.Owin.Security.Cookies;
using Microsoft.Owin.Security.OpenIdConnect;
using Owin;

namespace Learning.IdentiyServer.UI.Web
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {

            AntiForgeryConfig.UniqueClaimTypeIdentifier = Constants.ClaimTypes.Subject;
            JwtSecurityTokenHandler.InboundClaimTypeMap = new Dictionary<string, string>();

            app.Map("/identity", idsrvApp =>
            {
                var options = new IdentityServerOptions
                {
                    SiteName = "Embedded IdentityServer",
                    SigningCertificate = LoadCertificate(),

                    Factory = new IdentityServerServiceFactory()
                        .UseInMemoryUsers(Users.Get())
                        .UseInMemoryClients(Clients.Get())
                        .UseInMemoryScopes(Scopes.Get())
                };

                // redireciona automaticamente para a home após logout
                options.AuthenticationOptions.EnablePostSignOutAutoRedirect = true;

                idsrvApp.UseIdentityServer(options);
            });

            app.UseResourceAuthorization(new AuthorizationManager());

            app.UseCookieAuthentication(new CookieAuthenticationOptions
            {
                AuthenticationType = "Cookies"
            });

            app.UseOpenIdConnectAuthentication(new OpenIdConnectAuthenticationOptions
            {
                Authority = "https://localhost:44301/identity",

                ClientId = "mvc",
                Scope = "openid profile roles",
                RedirectUri = "https://localhost:44301/",
                ResponseType = "id_token",

                SignInAsAuthenticationType = "Cookies",
                UseTokenLifetime = false,

                Notifications = new OpenIdConnectAuthenticationNotifications
                {
                    SecurityTokenValidated = n =>
                    {
                        var id = n.AuthenticationTicket.Identity;

                        //we want to keep first name, last name, subject and roles
                        var givenName = id.FindFirst(Constants.ClaimTypes.GivenName);
                        var familyName = id.FindFirst(Constants.ClaimTypes.FamilyName);
                        var sub = id.FindFirst(Constants.ClaimTypes.Subject);
                        var roles = id.FindAll(Constants.ClaimTypes.Role);

                        //create new identity and set name and role claim type
                        var nid = new ClaimsIdentity(
                            id.AuthenticationType,
                            Constants.ClaimTypes.GivenName,
                            Constants.ClaimTypes.Role
                            );

                        nid.AddClaim(givenName);
                        nid.AddClaim(familyName);
                        nid.AddClaim(sub);
                        nid.AddClaims(roles);

                        //add some other app specific claim
                        nid.AddClaim(new Claim("app_specific", "some data"));

                        // keep the id_token for logout
                        nid.AddClaim(new Claim("id_token", n.ProtocolMessage.IdToken));

                        n.AuthenticationTicket = new AuthenticationTicket(
                            nid,
                            n.AuthenticationTicket.Properties);

                        return Task.FromResult(0);

                    },

                    RedirectToIdentityProvider = n =>
                    {
                        if (n.ProtocolMessage.RequestType == OpenIdConnectRequestType.LogoutRequest)
                        {
                            var idTokenHint = n.OwinContext.Authentication.User.FindFirst("id_token");

                            if (idTokenHint != null)
                            {
                                n.ProtocolMessage.IdTokenHint = idTokenHint.Value;
                            }
                        }

                        return Task.FromResult(0);
                    }

                }

            });
        }

        private X509Certificate2 LoadCertificate()
        {
            return new X509Certificate2(string.Format(@"{0}\bin\idsrv3test.pfx", AppDomain.CurrentDomain.BaseDirectory),
                    "idsrv3test");
        }
    }
}