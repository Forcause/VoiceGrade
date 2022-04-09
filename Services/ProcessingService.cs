using VoiceGradeApi.Services.FileService;
using VoiceGradeApi.Util;
using Transcriber = VoiceGradeApi.Util.Transcriber;

namespace VoiceGradeApi.Services;

public class ProcessingService
{
   
    private object _lockTranscriber = new object();
    private IFileService _fileService;
    private TextParser _parser;
    private Correlator _correlator;

    public ProcessingService()
    {
        _parser = new TextParser();
        _correlator = new Correlator();
    }

    public string GetResultedFile(List<string> files)
    {
        string audioFile = "", pupilsFile = "";
        foreach (string file in files)
        {
            FileInfo info = new FileInfo(file);
            switch (info.Extension)
            {
                case ".json":
                    _fileService = new JsonService();
                    pupilsFile = file;
                    break;
                case ".xml":
                    _fileService = new XmlService();
                    pupilsFile = file;
                    break;
                default:
                    audioFile = file;
                    break;
            }
        }
        
        AudioConverter converter = new AudioConverter(audioFile);
        audioFile = converter.ConvertAudio();
        
        
        //Посмотреть, как сделать эксклюзивный доступ
        var allData = Transcriber.TranscribeAudio(audioFile);
        var pupils = _fileService.ReadFile(pupilsFile);
        var transcribedNames = _parser.ParseData(allData);
        _correlator.CorrelateScores(pupils, transcribedNames);
        string createdFilePath = _fileService.CreateFile(pupils);
        return createdFilePath;
    }
}