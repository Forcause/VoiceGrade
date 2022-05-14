using System.Text;
using Newtonsoft.Json.Linq;
using VoiceGradeApi.Models;
using Vosk;

namespace VoiceGradeApi.Util;

internal class Transcriber
{
    private static readonly AutoResetEvent _busyModel = new(true);

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

    public static string TranscribeAudio(string audioFilePath, TranscriberModel model)
    {
        //Initialize recognizer Model
        //var model = _model.Instance; 
        //Synchronize concurrent session to concurrent model
        _busyModel.WaitOne();

        var transcribedText = new StringBuilder("[");
        
        //Create and initialize parameters of recognizer object
        VoskRecognizer rec = new(model.Model, 32000.0f);
        Initialize(rec);
        
        //Transcribe audiofile and add results to StringBuilder
        using (Stream source = File.OpenRead($@"{audioFilePath}"))
        {
            var buffer = new byte[4096];
            int bytesRead;
            while ((bytesRead = source.Read(buffer, 0, buffer.Length)) > 0)
            {
                if (rec.AcceptWaveform(buffer, bytesRead))
                {
                    transcribedText.Append(rec.Result() + ',');
                }
                else
                {
                    rec.PartialResult();
                }
            }
        }

        transcribedText.Append(rec.FinalResult() + ']');

        //Reset recognizer for other session
        rec.Reset();
        _busyModel.Set();
        //get and return parsed from JSON text 
        return GetTranscribedText(transcribedText.ToString());
    }
}