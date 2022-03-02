using WebSite.Model;

namespace GanjAudio.ViewModels;
public class IndexViewModel
{
    public SignInModel? SignIn { get; set; }

    public SignUpModel? SignUp { get; set; }

    public GenerateAudioModel? GenerateAudio { get; set; }

    public IEnumerable<Note>? PublicNotes { get; set; }
}