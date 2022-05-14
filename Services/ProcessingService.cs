﻿using VoiceGradeApi.Models;
using VoiceGradeApi.Services.FileService;
using VoiceGradeApi.Services.AudioConvertService;
using VoiceGradeApi.Util;
using Transcriber = VoiceGradeApi.Util.Transcriber;

namespace VoiceGradeApi.Services;

public class ProcessingService
{
    private IFileService _fileService;
    private TextParser _parser;
    private Correlation _correlation;

    public ProcessingService()
    {
        _parser = new TextParser();
        _correlation = new Correlation();
    }

    public string GetResultedFile(List<string> files, TranscriberModel model)
    {
        string audioFile = "", pupilsFile = "";
        foreach (var file in files)
        {
            var info = new FileInfo(file);
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

        AudioConverter converter = new MpConverter(audioFile);
        audioFile = converter.ConvertAudio();
        var pupils = _fileService.ReadFile(pupilsFile);
        var allData = Transcriber.TranscribeAudio(audioFile, model);
        var transcribedNames = _parser.ParseData(allData);
        _correlation.CorrelateScores(pupils, transcribedNames);
        var createdFilePath = _fileService.CreateFile(pupils);
        return createdFilePath;
    }
}