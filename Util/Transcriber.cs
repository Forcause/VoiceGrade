using System.Text;
using Newtonsoft.Json.Linq;
using Vosk;

namespace VoiceGradeApi.Util;

public static class Transcriber
{
    private static readonly Model
        model = new(Directory.GetCurrentDirectory() + @"\AudioModel\vosk-model-small-ru-0.22");

    private static readonly VoskRecognizer rec = new(model, 16000.0f);

    private static VoskRecognizer Initialize()
    {
        Vosk.Vosk.SetLogLevel(-1);
        rec.SetMaxAlternatives(0);
        rec.SetWords(true);
        return rec;
    }

    private static String GetTranscribedText(string finalResult)
    {
        StringBuilder currentName = new StringBuilder();
        dynamic parsedResult = JObject.Parse(finalResult);
        JArray results = parsedResult.result;

        for (int i = 0; i < results.Count; i++)
        {
            currentName.Append(results[i]["word"]);
            currentName.Append(" ");
        }

        return currentName.ToString();
    }

    public static String TranscribeAudio(string audioFilePath)
    {
        VoskRecognizer rec = Initialize();

        using (Stream source = File.OpenRead($@"{audioFilePath}"))
        {
            byte[] buffer = new byte[4096];
            int bytesRead;
            while ((bytesRead = source.Read(buffer, 0, buffer.Length)) > 0)
            {
                if (rec.AcceptWaveform(buffer, bytesRead))
                {
                    rec.Result();
                }
                else
                {
                    rec.PartialResult();
                }
            }
        }

        return GetTranscribedText(rec.FinalResult());
    }
}