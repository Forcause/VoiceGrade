using System.Text;
using Newtonsoft.Json.Linq;
using Vosk;

namespace VoiceGradeApi.Services.FileReaderServices;

public class AudioReader : IFileReader
{
    private static VoskRecognizer Initialize()
    {
        Vosk.Vosk.SetLogLevel(-1);
        Model model = new Model(Directory.GetCurrentDirectory() + @"\vosk-model-small-ru-0.22");
        VoskRecognizer rec = new VoskRecognizer(model, 16000.0f);
        rec.SetMaxAlternatives(0);
        rec.SetWords(true);
        return rec;
    }

    public string Read(string path)
    {
        VoskRecognizer rec = Initialize();

        using (Stream source = File.OpenRead($@"{path}"))
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

        string resultedText = GetTranscribedText(rec.FinalResult());
        return resultedText;
    }


    private String GetTranscribedText(string finalResult)
    {
        StringBuilder currentName = new StringBuilder();
        dynamic parsedResult = JObject.Parse(finalResult);
        JArray results = parsedResult.result;

        for (int i = 0; i < results.Count; i++)
        {
            currentName.Append(results[i]["word"].ToString());
            currentName.Append(" ");
        }

        return currentName.ToString();
    }
}