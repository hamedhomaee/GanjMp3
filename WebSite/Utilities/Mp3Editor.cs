using System.Diagnostics;
using System.Globalization;

namespace WebSite.Utilities;

public class Mp3Editor : IMp3Editor
{
    static char separator = Path.DirectorySeparatorChar;

    public async Task GetOutputFile(string userFile, string episodes)
    {
        int userFileDuration = await GetDurationInSecondsAsync(userFile);
        int episodesDuration = await GetDurationInSecondsAsync(episodes);
        bool userFileSmaller = false;

        label_check:
        if (userFileDuration > episodesDuration)
        {
            await CutMp3File(episodesDuration, userFileSmaller, userFile);
            await DecreaseVolume();
            await MixWithEpisodes(episodes);
        }
        else
        {
            await ConcatenateMp3s(userFile, ExtensionAmount(userFileDuration, episodesDuration));
            userFileDuration = await GetDurationInSecondsAsync(
                $"{Directory.GetCurrentDirectory()}{separator}wwwroot{separator}uploads{separator}Concatenated.mp3");
            userFileSmaller = true;
            goto label_check;
        }
    }
    
    static async Task<int> GetMaxDurationAsync(string inputMp3, string outputMp3)
    {
        return (await GetDurationInSecondsAsync(inputMp3)) > (await GetDurationInSecondsAsync(outputMp3)) ? (await GetDurationInSecondsAsync(inputMp3)) :
            (await GetDurationInSecondsAsync(outputMp3));
    }

    static async Task<int> GetDurationInSecondsAsync(string mp3File)
    {
        DateTime dateTime = new();

        using (Process process = new Process())
        {
            process.StartInfo.FileName = "ffmpeg";
            process.StartInfo.Arguments = $"-i {mp3File}";
            process.StartInfo.UseShellExecute = false;
            process.StartInfo.RedirectStandardOutput = true;
            process.StartInfo.RedirectStandardError = true;

            process.Start();

            string output = await process.StandardOutput.ReadToEndAsync();

            string error = await process.StandardError.ReadToEndAsync();


            if (output != "")
            {
                if (output.Contains("Duration"))
                {

                    dateTime = DateTime.ParseExact(output.Substring(output.IndexOf("Duration") + 10, 8), "HH:mm:ss", CultureInfo.InvariantCulture);
                }
            }
            else
            {
                if (error.Contains("Duration"))
                {
                    dateTime = DateTime.ParseExact(error.Substring(error.IndexOf("Duration") + 10, 8), "HH:mm:ss", CultureInfo.InvariantCulture);
                }
            }

            await process.WaitForExitAsync();

            return (dateTime.Minute * 60) + dateTime.Second + 1;
        }
    }

    static async Task CutMp3File(int duration, bool userFileSmaller, string userFile = "")
    {
        string concatenatedFile = $"{Directory.GetCurrentDirectory()}{separator}wwwroot{separator}uploads{separator}Concatenated.mp3";

        Process process = new Process();
        process.StartInfo.FileName = "ffmpeg";
        process.StartInfo.Arguments = $"-y -i {(userFileSmaller ? concatenatedFile : userFile)} -ss 0 -to {duration} {Directory.GetCurrentDirectory()}{separator}wwwroot{separator}uploads{separator}Cut.mp3";
        process.StartInfo.UseShellExecute = false;
        process.StartInfo.CreateNoWindow = true;
        process.StartInfo.RedirectStandardOutput = false;
        process.StartInfo.RedirectStandardError = false;

        process.Start();

        await process.WaitForExitAsync();
    }

    static async Task MixWithEpisodes(string episodes)
    {
        Process process = new Process();
        process.StartInfo.FileName = "ffmpeg";
        process.StartInfo.Arguments = $"-i {Directory.GetCurrentDirectory()}{separator}wwwroot{separator}uploads{separator}DecreasedVolume.mp3 -i {episodes} -filter_complex amerge {Directory.GetCurrentDirectory()}{separator}wwwroot{separator}Mowlana.mp3";
        process.StartInfo.UseShellExecute = false;
        process.StartInfo.CreateNoWindow = true;
        process.StartInfo.RedirectStandardOutput = false;
        process.StartInfo.RedirectStandardError = false;

        process.Start();

        await process.WaitForExitAsync();
    }

    static async Task ConcatenateMp3s(string userFile, int durationAmount)
    {
        string arguments = "-i \"concat:";

        for (int i = 0; i < durationAmount; i++)
        {
            if(i == durationAmount - 1)
            {
                arguments += $"{userFile}\"";
            }
            else 
            {
                arguments += $"{userFile}|";
            }
        }

        arguments += $" -acodec copy .{separator}wwwroot{separator}uploads{separator}Concatenated.mp3";

        Process process = new Process();
        process.StartInfo.FileName = "ffmpeg";
        process.StartInfo.Arguments = arguments;
        process.StartInfo.UseShellExecute = false;
        process.StartInfo.CreateNoWindow = true;
        process.StartInfo.RedirectStandardOutput = false;
        process.StartInfo.RedirectStandardError = false;

        process.Start();

        await process.WaitForExitAsync();
    }

    static async Task DecreaseVolume()
    {
        Process process = new Process();
        process.StartInfo.FileName = "ffmpeg";
        process.StartInfo.Arguments = $"-y -i .{separator}wwwroot{separator}uploads{separator}Cut.mp3 -filter:a volume=0.30 .{separator}wwwroot{separator}uploads{separator}DecreasedVolume.mp3";
        process.StartInfo.UseShellExecute = false;
        process.StartInfo.CreateNoWindow = true;
        process.StartInfo.RedirectStandardOutput = false;
        process.StartInfo.RedirectStandardError = false;

        process.Start();

        await process.WaitForExitAsync();
    }

    static int ExtensionAmount(int userFileDuration, int episodesDuration)
    {
        int extensionAmount = userFileDuration; 
        int extensionAmountMultiplicaiton = 1;

        while(extensionAmount < episodesDuration)
        {
            extensionAmountMultiplicaiton++; 
            extensionAmount = userFileDuration * extensionAmountMultiplicaiton;
        }

        return extensionAmountMultiplicaiton;
    }
}