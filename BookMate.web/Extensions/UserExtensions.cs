using System.Security.Claims;

namespace BookMate.web.Extensions
{
    public static class UserExtensions
    {
        public static string GetUserId(this ClaimsPrincipal User)
        {
           
            return User.FindFirst(ClaimTypes.NameIdentifier)!.Value;

        }
    }
}
