using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;

namespace WebSite.Utilities;

public class FirstUserSignIn : IUserSignIn
{
    private readonly LinkGenerator _linkGenerator;

    public FirstUserSignIn(LinkGenerator linkGenerator)
    {
        _linkGenerator = linkGenerator;
    }

    public async Task SiteSignInAsync(string name, string email, bool isPersistent, string schemeName, HttpContext context)
    {
        List<Claim> claims = new()
        {
            new Claim(ClaimTypes.Name, name),
            new Claim(ClaimTypes.Email, email)
        };

        ClaimsIdentity claimsIdentity = new(claims, schemeName);

        ClaimsPrincipal claimsPrincipal = new(claimsIdentity);

        var authProperties = new AuthenticationProperties()
        {
            IsPersistent = isPersistent
        };

        await context.SignInAsync("CustomCookieAuthentication", claimsPrincipal, authProperties);
    }

    public async Task SiteSignOutAsync(string schemeName, HttpContext context)
    {
        await context.SignOutAsync();
    }

    public async Task ResetPassword(HttpContext context)
    {
        var token = await context.GetTokenAsync("CustomCookieAuthentication", "resetToken");
        string? clickableLink = _linkGenerator.GetPathByPage(context, "ResetPassword");
    }
}