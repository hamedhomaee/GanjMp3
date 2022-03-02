using System.Security.Claims;
using GanjAudio.ViewModels;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using WebSite.Data;
using WebSite.Model;
using WebSite.Utilities;

namespace WebSite.Pages.SignUp;

public class IndexModel : PageModel
{
    private readonly SiteDbContext? _context;
    private readonly IPasswordHasher? _passwordHasher;
    private readonly IUserSignIn _signIn;

    public IndexModel(SiteDbContext context, IPasswordHasher passwordHasher, IUserSignIn signIn)
    {
        _context = context;
        _passwordHasher = passwordHasher;
        _signIn = signIn;
    }

    public async Task<ContentResult> OnPostAsync([Bind(Prefix = "ViewModel.SignUp")]SignUpModel SignUp)
    {
        if (ModelState.IsValid)
        {
            SiteUser user = new()
            {
                UserName = SignUp!.UserName,
                Email = SignUp!.Email
            };

            user.Password = _passwordHasher!.HashPassword(user, SignUp!.Password!);

            await _context!.Users!.AddAsync(user);

            var result = await _context!.SaveChangesAsync();

            if (result > 0)
            {
                await _signIn.SiteSignInAsync(SignUp!.UserName!, SignUp!.Email!, false, "CustomCookieAuthentication", this.HttpContext);

                return Content("/portal");
            }
        }
        
        return Content("");
    }
}