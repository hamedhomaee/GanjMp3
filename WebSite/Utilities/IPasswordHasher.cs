using WebSite.Model;

namespace WebSite.Utilities;

public interface IPasswordHasher
{
    public string HashPassword(SiteUser user, string password);
    public bool VerifyPassword(SiteUser user, string hashedPassword, string providedPassword);
}