using LovNaZaklad_WebAPI.Auth;
using Microsoft.Owin;
using Microsoft.Owin.Security.OAuth;
using Owin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LovNaZaklad_WebAPI.App_Start
{
    public class OAuth
    {
        public static void Configure(IAppBuilder app)
        {
            app.UseOAuthAuthorizationServer(new OAuthAuthorizationServerOptions
            {
#if DEBUG
                AllowInsecureHttp = true,
#endif
                TokenEndpointPath = new PathString("/api/token"),
                ApplicationCanDisplayErrors = true,
                AccessTokenExpireTimeSpan = TimeSpan.FromHours(5),
                Provider = new OAuthServerProvider()
            });
        }
    }
}