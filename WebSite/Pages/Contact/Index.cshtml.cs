using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using WebSite.Data;
using WebSite.Model;

namespace WebSite.Pages.Contact;

public class IndexModel : PageModel
{
    private readonly SiteDbContext _context;

    public ContactMessage? Message { get; set; }

    public IndexModel(SiteDbContext context)
    {
        _context = context;
    }

    public ActionResult OnGet()
    {
        return Page();
    }

    public async Task<ActionResult> OnPostAsync([Bind("Email, Message", Prefix = "Message")] ContactMessage model)
    {
        if (ModelState.IsValid)
        {
            ContactMessage contactMessage = new()
            {
                Email = model.Email,
                Message = model.Message
            };

            await _context.ContactMessages!.AddAsync(contactMessage);

            await _context.SaveChangesAsync();

            return RedirectToPage("/Home/Index", new { message = "sent" });
        }
        else
        {
            return RedirectToPage("/Contact/Index", new { error = true });
        }
    }
}