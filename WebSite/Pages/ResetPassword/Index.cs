using GanjAudio.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using WebSite.Data;
using WebSite.Utilities;

namespace WebSite.Pages.ResetPassword;

public class IndexModel : PageModel
{
    private readonly SiteDbContext _context;
    private readonly IPasswordHasher _passwordHasher;

    public ResetPasswordViewModel? ViewModel { get; set; }

    public IndexModel(SiteDbContext context, IPasswordHasher passwordHasher)
    {
        _context = context;
        _passwordHasher = passwordHasher;
    }

    public async Task<ActionResult> OnGetAsync([Bind][FromQuery] Guid userId, [Bind][FromQuery] Guid token)
    {
        List<Model.ForgotPassword> forgotPasswords = await _context.ForgotPasswords!
            .Where(fp => fp.Expiration < DateTime.Now).ToListAsync();

        if (forgotPasswords.Count > 0)
        {
            foreach (Model.ForgotPassword item in forgotPasswords)
            {
                _context.ForgotPasswords!.Remove(item);
            }

            await _context.SaveChangesAsync();
        }

        if (token != Guid.Empty)
        {
            ViewData["token"] = token;
            ViewData["userId"] = userId;
            return Page();
        }

        return RedirectToPage("/Home/Index");
    }

    public async Task<ContentResult> OnPostAsync([Bind(Prefix = "ViewModel")] ResetPasswordViewModel model)
    {
        if (ModelState.IsValid)
        {
            try
            {
                var forgotPassword = await _context.ForgotPasswords!.SingleAsync(fp => fp.UserId == model.UserId);

                if(forgotPassword != null && Guid.Equals(forgotPassword.UserToken, model.Token) && forgotPassword.Expiration > DateTime.Now)
                {
                    try
                    {
                        var user = await _context.Users!.SingleAsync(u => u.Id == model.UserId);

                        user.Password = _passwordHasher.HashPassword(user, model.Password!);

                        await _context.SaveChangesAsync();

                        return Content("success");
                    }
                    catch
                    {
                        return Content("");
                    }
                }
            }
            catch
            {
                return Content("");
            }
        }
        return Content("");
    }
}