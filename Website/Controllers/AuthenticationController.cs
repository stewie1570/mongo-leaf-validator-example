using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;

namespace Website.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AuthenticationController : ControllerBase
    {
        [HttpPost]
        public async Task Login()
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, "stewie@gmail.com"),
                new Claim("FullName", "Stewart Anderson"),
                new Claim(ClaimTypes.Role, "Administrator"),
            };

            var claimsIdentity = new ClaimsIdentity(
                claims, CookieAuthenticationDefaults.AuthenticationScheme);

            var authProperties = new AuthenticationProperties
            {
                IsPersistent = true
            };

            await HttpContext.SignInAsync(
                CookieAuthenticationDefaults.AuthenticationScheme,
                new ClaimsPrincipal(claimsIdentity),
                authProperties);
        }

        [HttpGet]
        public object WhoAmI()
        {
            return new
            {
                Name = HttpContext.User.Identity.Name,
                FullName = GetClaim("FullName"),
                Role = GetClaim(ClaimTypes.Role)
            };
        }

        [HttpDelete]
        public async Task SignOut()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
        }

        #region Helpers

        private string GetClaim(string claimType)
        {
            return HttpContext
                .User
                .Claims
                .ToList()
                .FirstOrDefault(claim => claim.Type == claimType)
                ?.Value;
        }

        #endregion
    }
}