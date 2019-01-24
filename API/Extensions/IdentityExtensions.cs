using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Security.Principal;
using System.Web;

namespace API
{
    public static class IdentityExtensions
    {
        public static int GetUserId(this IIdentity identity)
        {
            var claimsIdentity = identity as ClaimsIdentity;
            if(claimsIdentity == null)
            {
                throw new ArgumentException("This method only works for Claims based Identity");
            }

            return int.Parse(claimsIdentity.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier).Value);
        }

    }
}