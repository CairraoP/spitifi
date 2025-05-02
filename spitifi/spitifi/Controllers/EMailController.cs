using Microsoft.AspNetCore.Mvc;
using spitifi.Areas.Identity.Pages.Account;
using spitifi.Services.Email;

namespace spitifi.Controllers;

[ApiController]
[Route("[controller]")]
public class EMailController : ControllerBase
{
    private readonly ICustomMailer _customMailer;

    public EMailController(ICustomMailer customMailer)
    {
        _customMailer = customMailer;
    }
    
    [HttpPost]
    public IActionResult Send(string toAddress, string subject, string body)
    {
        _customMailer.SendEmail(toAddress, subject, body);
        return Ok("Success");
    }
}