using System.Security.Claims;

namespace VolleyMS.Extensions
{
    public static class ClaimsExtension
    {
        public static Guid GetUserId(this ClaimsPrincipal user)
        {
            var userIdClaim = user.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userIdClaim == null || !Guid.TryParse(userIdClaim, out var userId))
            {
                throw new UnauthorizedAccessException("Invalid or missing user ID claim.");
            }
            return userId;
        }
        public static string GetUserName(this ClaimsPrincipal user)
        {
            var userNameClaim = user.FindFirstValue(ClaimTypes.Name);
            if (userNameClaim == null)
            {
                throw new UnauthorizedAccessException("Invalid or missing user ID claim.");
            }
            return userNameClaim;
        }
    }
}
