using GanjAudio.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using WebSite.Utilities;

namespace WebSite.Pages.GenerateAudio;

public class IndexModel : PageModel
{
    private readonly IWebHostEnvironment? _webHostEnvironment;
    private readonly IMp3Editor _mp3Editor;
    static char separator = Path.DirectorySeparatorChar;

    public IndexModel(IWebHostEnvironment webHostEnvironment, IMp3Editor mp3Editor)
    {
        _webHostEnvironment = webHostEnvironment;
        _mp3Editor = mp3Editor;
    }

    public async Task<ContentResult> OnPostAsync(GenerateAudioModel model)
    {
        if (System.IO.File.Exists($"{_webHostEnvironment!.WebRootPath}{separator}Mowlana.mp3"))
            System.IO.File.Delete($"{_webHostEnvironment.WebRootPath}{separator}Mowlana.mp3");
        if (System.IO.File.Exists($"{_webHostEnvironment!.WebRootPath}{separator}Episodes.mp3"))
            System.IO.File.Delete($"{_webHostEnvironment.WebRootPath}{separator}Episodes.mp3");

        System.IO.DirectoryInfo directory = new DirectoryInfo($"{Directory.GetCurrentDirectory()}{separator}wwwroot{separator}uploads");
        foreach (FileInfo file in directory.GetFiles())
        {
            file.Delete();
        }

        if (model.AudioFiles != null)
        {
            if (model.UserMp3File != null)
            {
                string filePath;
                string episodes = Path.Combine(_webHostEnvironment!.WebRootPath, "Episodes.mp3");
                string userFileName;

                EpisodeGenerator(episodes, model.AudioFiles, _webHostEnvironment!.WebRootPath);

                var uniqueFileName = Guid.NewGuid().ToString();
                var uploads = Path.Combine(_webHostEnvironment!.WebRootPath, "uploads");
                filePath = Path.Combine(uploads, uniqueFileName + ".mp3");

                userFileName = Path.GetFileName(filePath);

                using (FileStream theFile = new FileStream(filePath, FileMode.Create))
                {
                    await model.UserMp3File.CopyToAsync(theFile);
                }

                await _mp3Editor.GetOutputFile(filePath, episodes);
            }
            else
            {
                string episodes = Path.Combine(_webHostEnvironment!.WebRootPath, "Mowlana.mp3");

                EpisodeGenerator(episodes, model.AudioFiles, _webHostEnvironment!.WebRootPath);
            }

            return Content("success");
        }

        return Content("failed");
    }

    static async void EpisodeGenerator(string episodes, List<string> AudioFiles, string rootPath)
    {
        using (var fs = System.IO.File.OpenWrite(episodes))
        {
            foreach (string item in AudioFiles)
            {
                var buffer = await System.IO.File.ReadAllBytesAsync($"{rootPath}{separator}AudioFiles{separator}{item}.mp3");
                await fs.WriteAsync(buffer, 0, buffer.Length);
                await fs.FlushAsync();
            }
        }
    }
}