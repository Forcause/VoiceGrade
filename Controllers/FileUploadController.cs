using Microsoft.AspNetCore.Mvc;
using VoiceGradeApi.Services.FileReaderServices;

namespace VoiceGradeApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class FileUploadController : ControllerBase
{
    private readonly string _directoryPath = (Directory.GetCurrentDirectory() + ($@"\UploadedFiles\{Guid.NewGuid()}"));
    private ProcessingService? _processingService;

    [HttpPost("upload")]
    public async Task<ActionResult> UploadFiles(IFormFileCollection files)
    {
        if (!Directory.Exists(_directoryPath)) Directory.CreateDirectory(_directoryPath);
        List<string> downloadedFiles = new List<string>();
        foreach (IFormFile file in files)
        {
            var originalName = Path.GetFileName(file.FileName);
            var filePath = Path.Combine(_directoryPath, originalName);

            await using var stream = System.IO.File.Create(filePath);
            await file.CopyToAsync(stream);
            downloadedFiles.Add(filePath);
        }

        Ok("Successfully uploaded");
        _processingService = HttpContext.RequestServices.GetService<ProcessingService>();
        var res = _processingService.GetResultedFile(downloadedFiles);
        byte[] bytes = await System.IO.File.ReadAllBytesAsync(res);
        return File(bytes, "application/octet-stream", res);
    }
}