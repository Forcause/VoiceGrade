using Microsoft.AspNetCore.Mvc;
using VoiceGradeApi.Services;

namespace VoiceGradeApi.API;

[ApiController]
[Route("api")]
public class FileUploadController : ControllerBase
{
    private readonly string _directoryPath;
    private readonly ProcessingService? _processingService;
    private readonly TranscriberService? _transcriberService;

    public FileUploadController(ProcessingService processingService, TranscriberService transcriberService)
    {
        _directoryPath = (Directory.GetCurrentDirectory() + ($@"\UploadedFiles\{Guid.NewGuid()}"));
        _processingService = processingService;
        _transcriberService = transcriberService;
    }

    /// <summary>
    /// Получает аудиофайл (MP3/WAV/M4A) и список учеников (JSON/XML)
    /// </summary>
    /// <remarks>
    /// Пример JSON элемента файла:
    /// 
    ///     POST api/upload
    ///       {
    ///         "Name": "Илья",
    ///         "Surname": "Воробьев",
    ///         "MiddleName": "Викторович"
    ///       },
    ///
    /// 
    /// Пример XML элемента файла:
    ///
    ///     POST api/upload
    ///     pupil
    ///         Name = "Юрий"
    ///         Surname = "Булгаков"
    ///         MiddleName = "Сергеевич"
    ///
    /// Загружаемый аудиофайл должен быть в формате mp3, wav, m4a.
    /// В качестве ответа будет отправлен файл в формате JSON/XML, с проставленными оценками
    /// </remarks>
    /// <response code="200"> Итоговый файл создан</response>
    /// <response code="400"> Проблемы с загрузкой файлов</response>
    /// <returns>Возвращает файл с учениками и их отметками (json/xml)</returns>
    // POST: api/upload
    
    [HttpPost("upload")]
    public async Task<ActionResult> UploadFiles(IFormFile file1, IFormFile file2)
    {
        
        if (file1 == null) throw new ArgumentNullException(nameof(file1));
        if (file2 == null) throw new ArgumentNullException(nameof(file2));

        if (!Directory.Exists(_directoryPath)) Directory.CreateDirectory(_directoryPath);
        var downloadedFiles = new List<string>();

        foreach (var file in new List<IFormFile> { file1, file2 })
        {
            var originalName = Path.GetFileName(file.FileName);
            var filePath = Path.Combine(_directoryPath, originalName);

            await using (var stream = System.IO.File.Create(filePath))
            {
                await file.CopyToAsync(stream);
            }

            downloadedFiles.Add(filePath);
        }

        var res = _processingService?.GetResultedFile(downloadedFiles, _transcriberService);
        var resultFileName = res[(res.LastIndexOf("\\") + 1)..];
        var bytes = await System.IO.File.ReadAllBytesAsync(res);
        return File(bytes, "application/json", resultFileName);
    }
}