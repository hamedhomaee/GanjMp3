namespace WebSite.Utilities;
public interface IUserSignIn
{
    public Task SiteSignInAsync(string name, string email, bool isPersistent, string schemeName, HttpContext context);
    public Task SiteSignOutAsync(string schemeName, HttpContext context);
    public Task ResetPassword(HttpContext context);
}