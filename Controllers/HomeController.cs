using Microsoft.AspNetCore.Mvc;

namespace VoiceGradeApi.Controllers;

public class HomeController : Controller
{
    // GET
    public IActionResult Index()
    {
        return View();
    }
}