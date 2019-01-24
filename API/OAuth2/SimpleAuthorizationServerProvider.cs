using Microsoft.Owin.Security;
using Microsoft.Owin.Security.OAuth;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;

namespace API.OAuth2
{
    public class SimpleAuthorizationServerProvider : OAuthAuthorizationServerProvider
    {
        public override Task ValidateClientAuthentication(OAuthValidateClientAuthenticationContext context)
        {
            string clientId = string.Empty;
            string clientSecret = string.Empty;

            if (!context.TryGetBasicCredentials(out clientId, out clientSecret))
            {
                context.TryGetFormCredentials(out clientId, out clientSecret);
            }

            if (context.ClientId == null)
            {
                context.SetError("invalid_clientId", "ClientId should be sent.");
                return Task.FromResult<object>(null);
            }

            using (var db = new Models.APIContext())
            {
                if(context.ClientId != "test")
                {
                    context.SetError("invalid_clientId", "ClientId should be sent.");
                    return Task.FromResult<object>(null);
                }

                //TODO DB - Validation

                context.OwinContext.Set<string>("as:clientAllowedOrigin", "*");
                context.OwinContext.Set<string>("as:clientRefreshTokenLifeTime", "83000");

                context.Validated();
                return Task.FromResult<object>(null);
            }
        }

        public override Task GrantResourceOwnerCredentials(OAuthGrantResourceOwnerCredentialsContext context)
        {

            var allowedOrigin = context.OwinContext.Get<string>("as:clientAllowedOrigin");

            if (allowedOrigin == null) allowedOrigin = "*";


            using (var db = new Models.APIContext())
            {
                var user = db.Users
                             .Where(u => u.UserName == context.UserName)
                             .FirstOrDefault();

                if (user == null)
                {
                    context.SetError("invalid_grant", "The user name or password is incorrect.");
                    return Task.FromResult<object>(null);
                }

                if(user.Hash != PasswordHasherService.HashPassword(context.Password, user.Salt))
                {
                    context.SetError("invalid_grant", "The user name or password is incorrect.");
                    return Task.FromResult<object>(null);
                }

                var identity = GetIdentityFromUser(db, user, context.Options.AuthenticationType);

                var ticket = new AuthenticationTicket(identity, BuildTicketProperties(db, user, context.ClientId == null ? String.Empty : context.ClientId));
                context.Validated(ticket);

                return Task.FromResult<object>(null);
            }
        }

        public override Task TokenEndpoint(OAuthTokenEndpointContext context)
        {
            foreach (KeyValuePair<string, string> property in context.Properties.Dictionary)
            {
                context.AdditionalResponseParameters.Add(property.Key, property.Value);
            }

            return Task.FromResult<object>(null);
        }

        public override Task GrantRefreshToken(OAuthGrantRefreshTokenContext context)
        {
            var originalClient = context.Ticket.Properties.Dictionary["as:client_id"];
            var currentClient = context.ClientId;

            if (originalClient != currentClient)
            {
                context.SetError("invalid_clientId", "Refresh token is issued to a different clientId.");
                return Task.FromResult<object>(null);
            }

            using (var db = new Models.APIContext())
            {
                var user = db.Users
                              .Where(u => u.Id == int.Parse(context.Ticket.Identity.FindFirst(ClaimTypes.NameIdentifier).Value))
                              .FirstOrDefault();

                if (user == null)
                {
                    context.SetError("invalid_grant", "The user is invalid");
                    return Task.FromResult<object>(null); ;
                }

                // Change auth ticket for refresh token requests
                var newIdentity = GetIdentityFromUser(db, user, context.Options.AuthenticationType);

                var newTicket = new AuthenticationTicket(newIdentity, BuildTicketProperties(db, user, context.ClientId == null ? String.Empty : context.ClientId));
                context.Validated(newTicket);

                return Task.FromResult<object>(null);
            }
        }

        private ClaimsIdentity GetIdentityFromUser(Models.APIContext db, Core.User user, string authenticationType)
        {
            var identity = new ClaimsIdentity(authenticationType);

            identity.AddClaim(new System.Security.Claims.Claim(ClaimTypes.Name, string.Format("{0} {1}", user.FirstName, user.LastName)));            
            identity.AddClaim(new System.Security.Claims.Claim(ClaimTypes.Email, user.EmailAddress));
            identity.AddClaim(new System.Security.Claims.Claim(ClaimTypes.NameIdentifier, user.Id.ToString()));

            return identity;
        }

        private AuthenticationProperties BuildTicketProperties(Models.APIContext db, Core.User user, string clientId)
        {
            var props = new AuthenticationProperties(new Dictionary<string, string>
                {
                    {
                        "as:client_id", clientId
                    },
                    {
                        "userId", user.Id.ToString()
                    },
                    {
                        "firstName", user.FirstName ?? String.Empty
                    },
                    {
                        "lastName", user.LastName ?? String.Empty
                    },
                    {
                        "emailAddress", user.EmailAddress
                    }
                });

            return props;
        }
        
    }
}