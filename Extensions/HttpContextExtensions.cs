namespace MaxsMusicQuiz.Backend.Extensions;

using System.Security.Claims;
using Microsoft.AspNetCore.Http;

public static class HttpContextExtensions
{
    public static int GetUserId(this HttpContext httpContext)
    {
        if (httpContext.User.Identity is ClaimsIdentity identity)
        {
            var userIdClaim = identity.FindFirst("userId");
            if (userIdClaim != null && int.TryParse(userIdClaim.Value, out var userId))
            {
                return userId;
            }
        }

        throw new InvalidOperationException("User ID not found in the current context.");
    }
}