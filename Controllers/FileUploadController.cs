using Microsoft.AspNetCore.Mvc;
using VoiceGradeApi.Models;

namespace VoiceGradeApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class FileUploadController : ControllerBase
{
    [HttpPost("upload")]
    public async Task<ActionResult> Get(IFormFileCollection files)
    {
        foreach (IFormFile file in files)
        {
            var originalName = Path.GetFileName(file.FileName);
            var filePath = Path.Combine(Directory.GetCurrentDirectory() + (@"\UploadedFiles"), originalName);
            await using var stream = System.IO.File.Create(filePath);
            file.CopyToAsync(stream);
        }
        
        return Ok("Successfully uploaded");
    }
    
}