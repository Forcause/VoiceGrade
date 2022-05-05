using Microsoft.AspNetCore.Mvc;
using VoiceGradeApi.Services;

namespace VoiceGradeApi.API;

[ApiController]
[Route("api")]
public class FileUploadController : ControllerBase
{
    private readonly string _directoryPath;
    private readonly ProcessingService? _processingService;

    public FileUploadController(ProcessingService service)
    {
        _directoryPath = (Directory.GetCurrentDirectory() + ($@"\UploadedFiles\{Guid.NewGuid()}"));
        _processingService = service;
    }

    [HttpPost("upload")]
    public async Task<ActionResult> UploadFiles(IFormFile file1, IFormFile file2)
    {
        if (file1 == null) throw new ArgumentNullException(nameof(file1));
        if (file2 == null) throw new ArgumentNullException(nameof(file2));

        if (!Directory.Exists(_directoryPath)) Directory.CreateDirectory(_directoryPath);
        List<string> downloadedFiles = new List<string>();

        foreach (IFormFile file in new List<IFormFile> {file1, file2})
        {
            var originalName = Path.GetFileName(file.FileName);
            var filePath = Path.Combine(_directoryPath, originalName);

            await using (var stream = System.IO.File.Create(filePath))
            {
                await file.CopyToAsync(stream);
            }

            downloadedFiles.Add(filePath);
        }


        var res = _processingService?.GetResultedFile(downloadedFiles);
        string resultFileName = res.Substring(res.LastIndexOf("\\") + 1);
        byte[] bytes = await System.IO.File.ReadAllBytesAsync(res);
        return File(bytes, "application/json", resultFileName);
    }
}