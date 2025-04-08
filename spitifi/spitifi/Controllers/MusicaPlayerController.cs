using Microsoft.AspNetCore.Mvc;

namespace spitifi.Controllers;

public class MusicaPlayerController : Controller
{
    // GET
    public IActionResult Index()
    {
        return View();
    }
}