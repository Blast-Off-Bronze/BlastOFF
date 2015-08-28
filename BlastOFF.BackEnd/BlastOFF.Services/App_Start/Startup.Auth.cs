namespace BlastOFF.Services
{
    using System;

    using Data;
    using Providers;

    using Microsoft.AspNet.Identity;
    using Microsoft.Owin;
    using Microsoft.Owin.Security.Cookies;
    using Microsoft.Owin.Security.OAuth;

    using Owin;

    public partial class Startup
    {
        public static OAuthAuthorizationServerOptions OAuthOptions { get; private set; }

        public static string PublicClientId { get; private set; }

        public void ConfigureAuth(IAppBuilder app)
        {
            app.CreatePerOwinContext(BlastOFFContext.Create);
            app.CreatePerOwinContext<ApplicationUserManager>(ApplicationUserManager.Create);

            app.UseCookieAuthentication(new CookieAuthenticationOptions());
            app.UseExternalSignInCookie(DefaultAuthenticationTypes.ExternalCookie);

            if (OAuthOptions == null)
            {
                PublicClientId = "self";
                OAuthOptions = new OAuthAuthorizationServerOptions
                    {
                        TokenEndpointPath = new PathString("/Token"),
                        Provider = new ApplicationOAuthProvider(PublicClientId),
                        AccessTokenExpireTimeSpan = TimeSpan.FromHours(12),
                        AllowInsecureHttp = true
                    };
            }

            app.UseOAuthBearerTokens(OAuthOptions);
        }
    }
}