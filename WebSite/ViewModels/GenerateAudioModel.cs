namespace GanjAudio.ViewModels;

public class GenerateAudioModel
{
    public List<string>? AudioFiles { get; set; }

    public IFormFile? UserMp3File { get; set; }
}