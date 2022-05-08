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
            using var waveFileReader = new AudioFileReader(_infile);
            var outFormat = new WaveFormat(neededRate, 1);
            using var resample = new MediaFoundationResampler(waveFileReader, outFormat);
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