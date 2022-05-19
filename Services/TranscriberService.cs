using System.Text;
using Newtonsoft.Json.Linq;
using VoiceGradeApi.Models;
using Vosk;

namespace VoiceGradeApi.Services;

public class TranscriberService
{
    private static VoskRecognizer _recognizer;

    public TranscriberService()
    {
        var model = TranscriberModel.Instance;
        _recognizer = new VoskRecognizer(model.Model, 32000.0f);
        Initialize(_recognizer);
    }

    //Initialize parameters of recognizer
    private static void Initialize(VoskRecognizer rec)
    {
        Vosk.Vosk.SetLogLevel(-1);
        rec.SetMaxAlternatives(0);
        rec.SetWords(true);
    }

    private static String GetTranscribedText(string finalResult)
    {
        var currentElement = new StringBuilder();
        //Parse JSON string to elements
        var parsedResult = JArray.Parse(finalResult);

        foreach (var results in parsedResult)
        {
            for (var i = 0; i < results["result"].Count(); i++)
            {
                //Get needed element in JSON object and add it in string
                currentElement.Append(results["result"]?[i]?["word"]);
                currentElement.Append(' ');
            }
        }

        return currentElement.ToString();
    }


    public string TranscribeAudio(string audioFilePath)
    {

        var transcribedText = new StringBuilder("[");

        //Transcribe audiofile and add results to StringBuilder
        using Stream source = File.OpenRead($@"{audioFilePath}");
        var buffer = new byte[4096];
        int bytesRead;
        lock ("test")
        {
            while ((bytesRead = source.Read(buffer, 0, buffer.Length)) > 0)
            {
                if (_recognizer.AcceptWaveform(buffer, bytesRead))
                {
                    transcribedText.Append(_recognizer.Result() + ',');
                }
            }

            transcribedText.Append(_recognizer.FinalResult() + ']');
            //Reset recognizer for other session
            _recognizer.Reset();
        }
        //get and return parsed from JSON text 
        return GetTranscribedText(transcribedText.ToString());
    }
}