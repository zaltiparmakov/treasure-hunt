using System;
using System.Threading.Tasks;
using LovNaZaklad_WebAPI.App_Start;
using Microsoft.AspNet.Identity;
using Microsoft.Owin;
using Microsoft.Owin.Security;
using Microsoft.Owin.Security.Cookies;
using Microsoft.Owin.Security.OAuth;
using Owin;

[assembly: OwinStartup(typeof(LovNaZaklad_WebAPI.Startup))]

namespace LovNaZaklad_WebAPI
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            app.MapWhen(r =>
            {
                return r.Request.Path.StartsWithSegments(new PathString("/api/token"));
            },
                r => OAuth.Configure(r)
            );

            // Using cookies for authentication
            app.UseCookieAuthentication(new CookieAuthenticationOptions
            {
                AuthenticationMode = AuthenticationMode.Active,
                AuthenticationType = DefaultAuthenticationTypes.ApplicationCookie,
                LoginPath = new PathString("/Home/Login"),
                LogoutPath = new PathString("/Home/Logout"),
                ExpireTimeSpan = TimeSpan.FromMinutes(30)
            });

            app.UseOAuthBearerAuthentication(new OAuthBearerAuthenticationOptions());
        }
    }
}
