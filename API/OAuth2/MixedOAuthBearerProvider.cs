using Microsoft.Owin.Security.OAuth;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace API.OAuth2
{
    public class MixedOAuthBearerProvider : OAuthBearerAuthenticationProvider
    {
        public MixedOAuthBearerProvider() { }
        public override Task RequestToken(OAuthRequestTokenContext context)
        {
            var value = context.Request.Query.Get("__TOKEN__");

            if (!string.IsNullOrEmpty(value))
            {
                context.Token = value;
                return Task.FromResult<object>(null);
            }

            return base.RequestToken(context);
        }

        public override Task ValidateIdentity(OAuthValidateIdentityContext context)
        {


            return base.ValidateIdentity(context);
        }
    }
}