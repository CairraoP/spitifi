using Microsoft.AspNetCore.Mvc;
using spitifi.Areas.Identity.Pages.Account;

namespace spitifi.Controllers;

[ApiController]
[Route("[controller]")]
public class EMailController : ControllerBase
{
    [HttpPost]
    public IActionResult Send(string toAddress, string subject, string body)
    {
        EmailSender m = new EmailSender();
        m.SendEmail(toAddress, subject, body);
        return Ok("Success");
    }
}