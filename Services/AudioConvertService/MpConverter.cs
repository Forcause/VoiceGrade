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
            using var waveFileReader = new AudioFileReader(Infile);
            var outFormat = new WaveFormat(NeededRate, 1);
            using var resample = new MediaFoundationResampler(waveFileReader, outFormat);
            WaveFileWriter.CreateWaveFile(Outfile, resample);
        }
        catch (Exception e)
        {
            throw new Exception("Error with audio file, try another one");
        }

        return Outfile;
    }
}