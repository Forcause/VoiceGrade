namespace VoiceGradeApi.Services.AudioConvertService;

public abstract class AudioConverter
{
    private protected readonly string _infile;
    private protected readonly string outfile;
    private protected readonly int neededRate = 32000;

    protected AudioConverter(string filePath)
    {
        this._infile = filePath;
        this.outfile = _infile.Substring(0, _infile.LastIndexOf(("\\"), StringComparison.Ordinal)) +
                         @"\converted.wav";
    }

    public abstract string ConvertAudio();
}