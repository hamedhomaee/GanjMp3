using GanjAudio.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using WebSite.Data;
using Microsoft.EntityFrameworkCore;
using WebSite.Model;
using WebSite.Utilities;

namespace WebSite.Pages.SignIn;

public class IndexModel : PageModel
{
    private readonly SiteDbContext _context;
    private readonly IPasswordHasher _hasher;
    private readonly IUserSignIn _signIn;

    public IndexModel(SiteDbContext context, IPasswordHasher hasher, IUserSignIn signIn)
    {
        _context = context;
        _hasher = hasher;
        _signIn = signIn;
    }

    public async Task<ContentResult> OnPostAsync([Bind(Prefix = "ViewModel.SignIn")] SignInModel model)
    {
        if (ModelState.IsValid)
        {
            SiteUser? user = new();

            try
            {
                user = await _context!.Users!.SingleOrDefaultAsync<SiteUser>(u => u.UserName == model.NameOrEmail);
            }
            catch
            {
            }

            try
            {
                if (user == null)
                    user = await _context!.Users!.SingleAsync<SiteUser>(u => u.Email == model.NameOrEmail);
            }
            catch
            {
            }

            if (user != null)
            {
                var userpass = user!.Password!;
                var usercheckpass = _hasher.HashPassword(user, model.Password!);

                if (_hasher.VerifyPassword(user, user.Password!, model.Password!))
                {
                    await _signIn.SiteSignInAsync(user.UserName!, user.Email!, model.IsPersistent, "CustomCookieAuthentication", this.HttpContext);
                    return Content("/portal");
                }
            }
        }

        return Content("");
    }
}