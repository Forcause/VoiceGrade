using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using IHostingEnvironment = Microsoft.AspNetCore.Hosting.IHostingEnvironment;

namespace VoiceGradeApi.Pages;

public class IndexModel : PageModel
{
    private IHostingEnvironment Environment;
    public string Message { get; set; }
 
    public IndexModel(IHostingEnvironment _environment)
    {
        Environment = _environment;
    }
 
    public void OnGet()
    {
 
    }
 
    public void OnPostUpload(List<IFormFile> postedFiles)
    {
        var wwwPath = this.Environment.WebRootPath;
        var contentPath = this.Environment.ContentRootPath;
 
        var path = Path.Combine(this.Environment.WebRootPath, "Uploads");
        if (!Directory.Exists(path))
        {
            Directory.CreateDirectory(path);
        }
 
        var uploadedFiles = new List<string>();
        foreach (var postedFile in postedFiles)
        {
            var fileName = Path.GetFileName(postedFile.FileName);
            using (var stream = new FileStream(Path.Combine(path, fileName), FileMode.Create))
            {
                postedFile.CopyTo(stream);
                uploadedFiles.Add(fileName);
                this.Message += string.Format("<b>{0}</b> uploaded.<br />", fileName);
            }
        }
    }
}