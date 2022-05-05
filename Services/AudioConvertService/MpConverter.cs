using NAudio.Wave;

namespace VoiceGradeApi.Services.AudioConvertService;

public sealed class MpConverter : AudioConverter
{
    public string ConvertMpAudio()
    {
        string outfile = _infile.Substring(0, _infile.LastIndexOf(("\\"), StringComparison.Ordinal)) +
                         @"\converted.wav";
        int neededRate = 32000;
        try
        {
            using var reader = new AudioFileReader(_infile);
            reader.ToMono(1.0f, 0.0f);
            var outFormat = new WaveFormat(neededRate, reader.WaveFormat.Channels);
            using var resample = new MediaFoundationResampler(reader, outFormat);
            WaveFileWriter.CreateWaveFile(outfile, resample);
        }
        catch (Exception e)
        {
            throw new Exception("Error with audio file, try another one");
        }

        return outfile;
    }

    public MpConverter(string filePath) : base(filePath)
    {
    }
}