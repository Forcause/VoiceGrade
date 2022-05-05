namespace VoiceGradeApi.Services.AudioConvertService;

public abstract class AudioConverter
{
    private protected readonly string _infile;

    protected AudioConverter(string filePath)
    {
        this._infile = filePath;
    }
}