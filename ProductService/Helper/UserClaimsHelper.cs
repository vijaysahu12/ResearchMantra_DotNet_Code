using System.Security.Claims;

namespace ProductService.Helper
{
    public static class UserClaimsHelper
    {
        public static string GetClaimValue(this ClaimsPrincipal principal, string claimType)
        {
            return principal?.FindFirst(claimType)?.Value;
        }
    }
    
}
