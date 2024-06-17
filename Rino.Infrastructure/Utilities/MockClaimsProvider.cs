using System.Collections.Generic;
using System.Security.Claims;
using Rino.Domain.Entities;

namespace Rino.Infrastructure.Utilities
{
    public static class MockClaimsProvider
    {
        public static List<Claim> GetDefaultClaims(User user)
        {
            return new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Name, user.Login),
                new Claim(ClaimTypes.Email, user.Email),
                //new Claim(ClaimTypes.Role, user.Role),
                new Claim("CustomClaimType", "CustomClaimValue") 
            };
        }
    }
}
