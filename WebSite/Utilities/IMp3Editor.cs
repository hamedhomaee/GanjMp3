namespace WebSite.Utilities;

public interface IMp3Editor
{
    public Task GetOutputFile(string userFile, string episodes);
}