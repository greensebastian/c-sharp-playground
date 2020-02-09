using System.Collections.Generic;
using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authentication;

namespace Playground
{
    public class Authentication
    {
        public static IActionResult Authenticate(HttpContext httpContext)
        {
            var playgroundClaims = new List<Claim>()
            {
                new Claim(ClaimTypes.Name, "John Doe"),
                new Claim(ClaimTypes.Email, "john.doe@gmail.com"),
                new Claim(CustomClaimTypes.UserId, "123")
            };

            var playgroundIdentity = new ClaimsIdentity(playgroundClaims, "Identity");

            var userPrincipal = new ClaimsPrincipal(new[] { playgroundIdentity });

            httpContext.SignInAsync(userPrincipal);
        }
    }

    public static class CustomClaimTypes
    {
        public const string UserId = "UniqueId";
    }
}
