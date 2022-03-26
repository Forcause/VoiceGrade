using NAudio.Wave;

namespace VoiceGradeApi.Util;

public class AudioConverter
{
    public string ConvertAudio(string infile)
    {
        //Сделать файлы уникальными для пользователей
        string outfile = infile.Substring(0,infile.LastIndexOf(("\\"))) + @"\converted.wav";
        int neededRate = 16000;
        using (var reader = new AudioFileReader(infile))
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