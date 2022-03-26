using System.Text.RegularExpressions;
using VoiceGradeApi.Util;

namespace VoiceGradeApi.Services.FileReaderServices;

public class ProcessingService
{
    private Regex checkAudioFormat = new Regex(@"\w*\.wav");
    private AudioConverter converter = new AudioConverter();
    private Transcriber transcriber = new Transcriber();
    private JsonReader jsonReader = new JsonReader();
    private JsonWriter writer = new JsonWriter();
    private TextParser parser = new TextParser();
    private Correlator correlator = new Correlator();

    public string GetResultedFile(List<string> files)
    {
        string audioFile = "", jsonFile = "";
        foreach (string file in files)
        {
            if (file.Contains(".json")) jsonFile = file;
            else audioFile = file;
        }
        if (!checkAudioFormat.IsMatch(audioFile)) audioFile = converter.ConvertAudio(audioFile);
        var allData = transcriber.TranscribeAudio(audioFile);
        var pupils = jsonReader.Read(jsonFile);
        var transcribedNames = parser.ParseData(allData);
        correlator.CorrelateScores(pupils, transcribedNames);
        string createdFilePath = writer.CreateFile(pupils);
        return createdFilePath;
    }
}