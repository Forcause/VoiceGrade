using NAudio.Wave;

namespace VoiceGradeApi.Services.AudioConvertService;

public sealed class MpConverter : AudioConverter
{
    public MpConverter(string filePath) : base(filePath)
    {
    }

    public override string ConvertAudio()
    {
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
}