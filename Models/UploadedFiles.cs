namespace VoiceGradeApi.Models;

public class UploadedFiles
{
    public string JFile { get; set; }
    public string AudioFile { get; set; }

    public UploadedFiles(string jFile, string audioFile)
    {
        this.JFile = jFile;
        this.AudioFile = audioFile;
    }
}