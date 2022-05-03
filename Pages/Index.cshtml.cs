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
        string wwwPath = this.Environment.WebRootPath;
        string contentPath = this.Environment.ContentRootPath;
 
        string path = Path.Combine(this.Environment.WebRootPath, "Uploads");
        if (!Directory.Exists(path))
        {
            Directory.CreateDirectory(path);
        }
 
        List<string> uploadedFiles = new List<string>();
        foreach (IFormFile postedFile in postedFiles)
        {
            string fileName = Path.GetFileName(postedFile.FileName);
            using (FileStream stream = new FileStream(Path.Combine(path, fileName), FileMode.Create))
            {
                postedFile.CopyTo(stream);
                uploadedFiles.Add(fileName);
                this.Message += string.Format("<b>{0}</b> uploaded.<br />", fileName);
            }
        }
    }
}