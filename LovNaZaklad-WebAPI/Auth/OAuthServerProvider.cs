using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using System.Web.Helpers;
using LovNaZaklad_WebAPI.Models;
using Microsoft.Owin.Security;
using Microsoft.Owin.Security.OAuth;

namespace LovNaZaklad_WebAPI.Auth
{
    public class OAuthServerProvider : OAuthAuthorizationServerProvider
    {
        private const string CLIENT_ID = "test";
        private const string CLIENT_SECRET = "test";

        public override async Task ValidateClientAuthentication(OAuthValidateClientAuthenticationContext context)
        {
            string userId = "";
            string clientId = "";

            if(!context.TryGetBasicCredentials(out userId, out clientId))
            {
                context.TryGetFormCredentials(out userId, out clientId);
            }

            if(userId == CLIENT_ID && clientId == CLIENT_SECRET)
            {
                context.Validated();
            } else
            {
                context.SetError("invalid_client_secret", "Incorrect ClientID or ClientSecret.");
            }

            context.OwinContext.Set<string>("as:client_id", CLIENT_ID);
        }

        public override async Task GrantResourceOwnerCredentials(OAuthGrantResourceOwnerCredentialsContext context)
        {
            using (var db = new LovNaZakladDbContext())
            {
                var user = db.Users.SingleOrDefault(u => u.Username == context.UserName);
                if(Crypto.VerifyHashedPassword(user.Password, context.Password))
                {
                    var identity = new ClaimsIdentity(context.Options.AuthenticationType);
                    identity.AddClaims(new List<Claim>
                    {
                        new Claim(ClaimTypes.Name, user.Username),
                        new Claim(ClaimTypes.Email, user.Email),
                        new Claim(ClaimTypes.PrimarySid, user.UserID.ToString())
                    });

                    if(context.Scope.Count != 0)
                    {
                        identity.AddClaims(context.Scope.First()?.Split(',')?.Select(s => new Claim("as:scope", s)));
                    }

                    var properties = new AuthenticationProperties(new Dictionary<string, string> {
                        {"client_id", context.ClientId },
                        {"username", context.UserName}
                    });

                    var ticket = new AuthenticationTicket(identity, properties);
                    context.Validated(ticket);
                } else
                {
                    context.Rejected();
                    context.SetError("invalid_grant", "Username or Password is not correct");
                }
            }
        }

        public override async Task TokenEndpoint(OAuthTokenEndpointContext context)
        {
            foreach (KeyValuePair<string, string> property in context.Properties.Dictionary)
            {
                context.AdditionalResponseParameters.Add(property.Key, property.Value);
            }
        }
    }
}