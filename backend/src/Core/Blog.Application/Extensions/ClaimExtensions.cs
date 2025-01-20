using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Blog.Application.Common.Extensions
{
    public static class ClaimExtensions
    {
        public static string GetUsername(this ClaimsPrincipal user)
        {
            return user.Claims.SingleOrDefault(x => x.Type.Equals("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/givenname")).Value;
        }
    }
}