using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace FormPostClient.Controllers
{
    public class AccountController : Controller
    {
        private const string ClientUri = @"https://localhost:44304";
        private const string CallbackEndpoint = ClientUri + @"/account/signInCallback";
        private const string IdServBaseUri = @"https://localhost:44300/core";
        private const string AuthorizeUri = IdServBaseUri + @"/connect/authorize";

        public ActionResult SignIn()
        {
            var state = Guid.NewGuid().ToString("N");
            var nonce = Guid.NewGuid().ToString("N");

            var url = AuthorizeUri +
                "?client_id=implicitclient" +
                "&response_type=id_token" +
                "&scope=openid email profile" +
                "&redirect_uri=" + CallbackEndpoint +
                "&response_mode=form_post" +
                "&state=" + state +
                "&nonce=" + nonce;

            this.SetTempCookie(state, nonce);
            return this.Redirect(url);
        }

        [HttpPost]
        public async Task<ActionResult> SignInCallback()
        {
            var token = this.Request.Form["id_token"];
            var state = this.Request.Form["state"];

            var claims = await ValidateIdentityTokenAsync(token, state);
            var id = new ClaimsIdentity(claims, "Cookies");
            this.Request.GetOwinContext().Authentication.SignIn(id);

            return this.Redirect("/");
        }

        private void SetTempCookie(string state, string nonce)
        {
            var tempId = new ClaimsIdentity("TempCookie");
            tempId.AddClaim(new Claim("state", state));
            tempId.AddClaim(new Claim("nonce", nonce));

            this.Request.GetOwinContext().Authentication.SignIn(tempId);
        }
    }
}