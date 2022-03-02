using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using WebSite.Utilities;

namespace WebSite.Pages.SignOut;

public class IndexModel : PageModel
{
    private readonly IUserSignIn? _signIn;

    public IndexModel(IUserSignIn signIn)
    {
        _signIn = signIn;
    }

    public async Task<IActionResult> OnPostAsync()
    {
        await _signIn!.SiteSignOutAsync("CustomCookieAuthentication", this.HttpContext);

        return RedirectToPage("/Home/Index");
    }
}