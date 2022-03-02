using GanjAudio.ViewModels;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using WebSite.Data;
using WebSite.Model;
using WebSite.Utilities;

namespace WebSite.Pages.ForgotPassword;

public class IndexModel : PageModel
{
    private readonly IEmailSender? _emailSender;
    private readonly SiteDbContext _context;
    private readonly LinkGenerator _linkGenerator;

    public ForgotPasswordViewModel? Model { get; set; }

    public IndexModel(IEmailSender emailSender, SiteDbContext context, LinkGenerator linkGenerator)
    {
        _context = context;
        _emailSender = emailSender;
        _linkGenerator = linkGenerator;
    }

    public ActionResult OnGet()
    {
        return Page();
    }

    public async Task<ContentResult> OnPostAsync([Bind(Prefix = "Model")] ForgotPasswordViewModel model)
    {
        if (ModelState.IsValid)
        {
            try
            {
                var user = _context.Users!.Single<SiteUser>(u => u.Email == model.Email);

                WebSite.Model.ForgotPassword forgotPassword = new()
                {
                    UserId = user.Id,
                    UserToken = Guid.NewGuid(),
                    Expiration = DateTime.Now + new TimeSpan(1, 0, 0)
                };

                await _context.ForgotPasswords!.AddAsync(forgotPassword);

                await _context.SaveChangesAsync();

                var link = $"{_linkGenerator.GetUriByPage(this.HttpContext, "/ResetPassword/Index")}/?userid={forgotPassword.UserId}&token={forgotPassword.UserToken}";

                string htmlContent = @"
                    <!DOCTYPE html>
                    <html>
                        <body>
                            <div style='margin: 10px auto, text-align: center;'>
                                <p>برای بازیابی کلمه عبور خود بر روی لینک زیر کلیک کنید</p>
                                <a href='" + link + @"'>بازیابی کلمه عبور</a>
                            </div>
                        </body>
                    </html>
                ";

                _emailSender!.Send("Send_Grid", "support@ganjmp3.tk", "GanjMP3", "بازیابی کلمه عبور", user.Email!, user.UserName!, "", htmlContent);

                return Content("success");
            }
            catch (System.Exception)
            {
                return Content("");
            }
        }

        return Content("");
    }
}