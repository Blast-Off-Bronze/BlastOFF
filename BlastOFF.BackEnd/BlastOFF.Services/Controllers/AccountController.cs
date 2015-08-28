namespace BlastOFF.Services.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Net.Http;
    using System.Security.Cryptography;
    using System.Threading.Tasks;
    using System.Web;
    using System.Web.Http;

    using BlastOFF.Models.UserModel;
    using Models;

    using Microsoft.AspNet.Identity;
    using Microsoft.AspNet.Identity.Owin;
    using Microsoft.Owin.Security;
    using Microsoft.Owin.Security.Cookies;

    [Authorize]
    [RoutePrefix("api/account")]
    public class AccountController : ApiController
    {
        private const string LocalLoginProvider = "Local";

        private ApplicationUserManager _userManager;

        public AccountController()
        {
        }

        public AccountController(
            ApplicationUserManager userManager,
            ISecureDataFormat<AuthenticationTicket> accessTokenFormat)
        {
            this.UserManager = userManager;
            this.AccessTokenFormat = accessTokenFormat;
        }

        public ApplicationUserManager UserManager
        {
            get
            {
                return this._userManager ?? this.Request.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
            private set
            {
                this._userManager = value;
            }
        }

        public ISecureDataFormat<AuthenticationTicket> AccessTokenFormat { get; private set; }

        //// POST api/account/register
        [AllowAnonymous]
        [Route("register")]
        public async Task<IHttpActionResult> Register(RegisterBindingModel model)
        {
            if (model == null)
            {
                return this.BadRequest("Cannot create an empty user model.");
            }

            if (!this.ModelState.IsValid)
            {
                return this.BadRequest(this.ModelState);
            }

            var user = new ApplicationUser { UserName = model.Username, Email = model.Email };

            IdentityResult result = await this.UserManager.CreateAsync(user, model.Password);

            if (!result.Succeeded)
            {
                return this.GetErrorResult(result);
            }

            var loginResult =
                await this.Login(new LoginBindingModel { Username = model.Username, Password = model.Password });

            return loginResult;
        }

        // POST api/account/login
        [HttpPost]
        [AllowAnonymous]
        [Route("login")]
        public async Task<IHttpActionResult> Login(LoginBindingModel model)
        {
            if (model == null)
            {
                return this.BadRequest("Invalid user data");
            }

            var requestParams = new List<KeyValuePair<string, string>>
                {
                    new KeyValuePair<string, string>("grant_type", "password"),
                    new KeyValuePair<string, string>("username", model.Username),
                    new KeyValuePair<string, string>("password", model.Password)
                };

            var requestParamsFormUrlEncoded = new FormUrlEncodedContent(requestParams);

            var client = new HttpClient();

            using (client)
            {
                var tokenServiceResponse =
                    await client.PostAsync("http://localhost:1505/Token", requestParamsFormUrlEncoded);

                return this.ResponseMessage(tokenServiceResponse);
            }
        }

        // POST api/account/logout
        [Route("logout")]
        public IHttpActionResult Logout()
        {
            this.Authentication.SignOut(CookieAuthenticationDefaults.AuthenticationType);
            return this.Ok();
        }

        // POST api/Account/ChangePassword
        [Route("ChangePassword")]
        public async Task<IHttpActionResult> ChangePassword(ChangePasswordBindingModel model)
        {
            if (!this.ModelState.IsValid)
            {
                return this.BadRequest(this.ModelState);
            }

            IdentityResult result =
                await
                this.UserManager.ChangePasswordAsync(
                    this.User.Identity.GetUserId(),
                    model.OldPassword,
                    model.NewPassword);

            if (!result.Succeeded)
            {
                return this.GetErrorResult(result);
            }

            return this.Ok();
        }

        // POST api/Account/SetPassword
        [Route("SetPassword")]
        public async Task<IHttpActionResult> SetPassword(SetPasswordBindingModel model)
        {
            if (!this.ModelState.IsValid)
            {
                return this.BadRequest(this.ModelState);
            }

            IdentityResult result =
                await this.UserManager.AddPasswordAsync(this.User.Identity.GetUserId(), model.NewPassword);

            if (!result.Succeeded)
            {
                return this.GetErrorResult(result);
            }

            return this.Ok();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && this._userManager != null)
            {
                this._userManager.Dispose();
                this._userManager = null;
            }

            base.Dispose(disposing);
        }

        #region Helpers

        private IAuthenticationManager Authentication
        {
            get
            {
                return this.Request.GetOwinContext().Authentication;
            }
        }

        private IHttpActionResult GetErrorResult(IdentityResult result)
        {
            if (result == null)
            {
                return this.InternalServerError();
            }

            if (!result.Succeeded)
            {
                if (result.Errors != null)
                {
                    foreach (string error in result.Errors)
                    {
                        this.ModelState.AddModelError("", error);
                    }
                }

                if (this.ModelState.IsValid)
                {
                    return this.BadRequest();
                }

                return this.BadRequest(this.ModelState);
            }

            return null;
        }

        private static class RandomOAuthStateGenerator
        {
            private static RandomNumberGenerator _random = new RNGCryptoServiceProvider();

            public static string Generate(int strengthInBits)
            {
                const int bitsPerByte = 8;

                if (strengthInBits % bitsPerByte != 0)
                {
                    throw new ArgumentException("strengthInBits must be evenly divisible by 8.", "strengthInBits");
                }

                int strengthInBytes = strengthInBits / bitsPerByte;

                byte[] data = new byte[strengthInBytes];
                _random.GetBytes(data);
                return HttpServerUtility.UrlTokenEncode(data);
            }
        }

        #endregion
    }
}