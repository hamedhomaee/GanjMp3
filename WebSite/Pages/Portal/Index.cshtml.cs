using System.Text.Json;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using WebSite.Data;
using WebSite.Model;

namespace WebSite.Pages.Portal;

[Authorize]
public class IndexModel : PageModel
{
    private readonly IHttpClientFactory? _httpClientFactory;
    private readonly SiteDbContext _context;

    public IEnumerable<Note>? UserNotes { get; set; }

    public IndexModel(IHttpClientFactory httpClientFactory, SiteDbContext context)
    {
        _httpClientFactory = httpClientFactory;
        _context = context;
    }

    public async Task<ActionResult> OnGetAsync()
    {
        SiteUser user = _context.Users!.Single<SiteUser>(u => u.UserName == HttpContext.User.Identity!.Name);

        ViewData["userId"] = user.Id;

        try
        {
            UserNotes = await _httpClientFactory.CreateClient().GetFromJsonAsync<IEnumerable<Note>>($"https://ganjmp3webapi.azurewebsites.net/getnotes/?id={user.Id}");
        }
        catch
        {
            return Page();
        }

        return Page();
    }
}