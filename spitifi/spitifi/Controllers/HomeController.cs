using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using spitifi.Models;

namespace spitifi.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;

    
    public HomeController(ILogger<HomeController> logger)
    {
        _logger = logger;
    }

    // to allow access to this endpoint, the request must come from the respective view
   // [ValidateAntiForgeryToken]
    public IActionResult Index()
    {
        return View();
    }

    [ValidateAntiForgeryToken]
    public IActionResult Privacy()
    {
        return View();
    }
    
    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}