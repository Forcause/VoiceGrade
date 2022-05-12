namespace VoiceGradeApi.Services.AudioConvertService;

public abstract class AudioConverter
{
    private protected readonly string Infile;
    private protected readonly string Outfile;
    private protected const int NeededRate = 32000;

    protected AudioConverter(string filePath)
    {
        this.Infile = filePath;
        this.Outfile = Infile.Substring(0, Infile.LastIndexOf(("\\"), StringComparison.Ordinal)) +
                         @"\converted.wav";
    }

    public abstract string ConvertAudio();
}