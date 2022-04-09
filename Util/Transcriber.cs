using System.Text;
using Newtonsoft.Json.Linq;
using VoiceGradeApi.Models;
using Vosk;

namespace VoiceGradeApi.Util;

internal class Transcriber
{
    private static AutoResetEvent _transcriberEvent = new AutoResetEvent(true);
    
    //Initialize parameters of recognizer
    private static void Initialize(VoskRecognizer rec)
    {
        Vosk.Vosk.SetLogLevel(-1);
        Vosk.Vosk.GpuInit();
        Vosk.Vosk.GpuThreadInit();
        rec.SetMaxAlternatives(0);
        rec.SetWords(true);
    }

    private static String GetTranscribedText(string finalResult)
    {
        StringBuilder currentElement = new StringBuilder();
        //Parse JSON string to elements
        var parsedResult = JArray.Parse(finalResult);

        foreach (var results in parsedResult)
        {
            for (int i = 0; i < results["result"].Count(); i++)
            {
                //Get needed element in JSON object and add it in string
                currentElement.Append(results["result"][i]["word"].ToString());
                currentElement.Append(" ");
            }
        }

        return currentElement.ToString();
    }

    public static string TranscribeAudio(string audioFilePath)
    {
        //Initialize recognizer Model
        Model model = TranscriberModel.GetInstance.Model;
        
        //Synchronize concurrent session to concurrent model
        _transcriberEvent.WaitOne();
        
        //Create and initialize parameters of recognizer object
        VoskRecognizer rec = new(model, 32000.0f);
        Initialize(rec);
        
        StringBuilder transcribedText = new StringBuilder("[");
        
        //Transcribe audiofile and add results to StringBuilder
        using (Stream source = File.OpenRead($@"{audioFilePath}"))
        {
            byte[] buffer = new byte[4096];
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
        _transcriberEvent.Set();
        //get and return parsed from JSON text 
        return GetTranscribedText(transcribedText.ToString());
    }
}