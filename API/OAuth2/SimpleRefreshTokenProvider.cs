using Microsoft.Owin.Security.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Threading.Tasks;
using System.Web;

namespace API.OAuth2
{
    public class SimpleRefreshTokenProvider : IAuthenticationTokenProvider
    {
        public void Create(AuthenticationTokenCreateContext context)
        {
            var clientid = context.Ticket.Properties.Dictionary["as:client_id"];

            if (string.IsNullOrEmpty(clientid))
            {
                return;
            }

            var refreshTokenId = Guid.NewGuid().ToString("n");
            var refreshTokenLifeTime = context.OwinContext.Get<string>("as:clientRefreshTokenLifeTime");

            //TODO DB ENTITY INSTANCE 
            
            context.Ticket.Properties.IssuedUtc = DateTimeOffset.Now;
            context.Ticket.Properties.ExpiresUtc = DateTimeOffset.Now.AddMonths(1);

            var protectedToken = context.SerializeTicket();

            //TODO DB INSERTION

            context.SetToken(refreshTokenId);
        }

        public void Receive(AuthenticationTokenReceiveContext context)
        {
            var allowedOrigin = context.OwinContext.Get<string>("as:clientAllowedOrigin");
            context.OwinContext.Response.Headers.Add("Access-Control-Allow-Origin", new[] { allowedOrigin });

            string hashedTokenId = GetHash(context.Token);

            // context.DeserializeTicket(protectedToken)
        }


        public Task CreateAsync(AuthenticationTokenCreateContext context)
        {
            Create(context);
            return Task.FromResult<object>(null);
        }

        public Task ReceiveAsync(AuthenticationTokenReceiveContext context)
        {
            Receive(context);
            return Task.FromResult<object>(null);
        }

        public static string GetHash(string input)
        {
            HashAlgorithm hashAlgorithm = new SHA256CryptoServiceProvider();

            byte[] byteValue = System.Text.Encoding.UTF8.GetBytes(input);

            byte[] byteHash = hashAlgorithm.ComputeHash(byteValue);

            return Convert.ToBase64String(byteHash);
        }
    }
}