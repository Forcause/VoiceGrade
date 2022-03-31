using NAudio.Wave;

namespace VoiceGradeApi.Util;

public class AudioConverter
{
    private string _infile;

    public AudioConverter(string filePath)
    {
        this._infile = filePath;
    }
    public string ConvertAudio()
    {
        string outfile = _infile.Substring(0,_infile.LastIndexOf(("\\"))) + @"\converted.wav";
        int neededRate = 16000;
        using (var reader = new AudioFileReader(_infile))
        {
            reader.ToMono(1.0f, 0.0f);
            var outFormat = new WaveFormat(neededRate, reader.WaveFormat.Channels);
            using (var resampler = new MediaFoundationResampler(reader, outFormat))
            {
                WaveFileWriter.CreateWaveFile(outfile, resampler);
            }
        }

        return outfile;
    }
}