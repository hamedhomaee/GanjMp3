using Microsoft.AspNetCore.Identity;
using WebSite.Model;

namespace WebSite.Utilities;
public class SimplePasswordHasher : IPasswordHasher
{
    public string HashPassword(SiteUser user, string password)
    {
        PasswordHasher<SiteUser> hasher = new();

        return hasher.HashPassword(user, password);
    }

    public bool VerifyPassword(SiteUser user, string hashedPassword, string providedPassword)
    {
        PasswordHasher<SiteUser> hasher = new();

        return hasher.VerifyHashedPassword(user, hashedPassword, providedPassword) == PasswordVerificationResult.Success ? true : false;
    }
}