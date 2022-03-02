using GanjAudio.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using WebSite.Model;

namespace WebSite.Pages.Home;

public class IndexModel : PageModel
{
    private readonly IWebHostEnvironment _webHostEnvironment;
    private readonly IHttpClientFactory? _httpClientFactory;

    public IndexModel(IWebHostEnvironment webHostEnvironment, IHttpClientFactory httpClientFactory)
    {
        _webHostEnvironment = webHostEnvironment;
        _httpClientFactory = httpClientFactory;
    }

    [BindProperty(SupportsGet = true)]
    public IndexViewModel? ViewModel { get; set; }

    public async Task<ActionResult> OnGetAsync()
    {
        try
        {
            ViewModel!.PublicNotes = await _httpClientFactory.CreateClient().GetFromJsonAsync<IEnumerable<Note>>($"https://ganjmp3webapi.azurewebsites.net/getpublicnotes");
        }
        catch
        {
            return Page();
        }

        return Page();
    }

    public ActionResult OnPost()
    {
        return RedirectToPage("signup");
    }
}