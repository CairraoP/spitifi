using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using spitifi.Services.Email;

namespace spitifi.Controllers;

[ApiController]
[Route("[controller]")]
[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Administrador")]
public class EmailController : ControllerBase
{
    private readonly ICustomMailer _customMailer;

    public EmailController(ICustomMailer customMailer)
    {
        _customMailer = customMailer;
    }
    
    /// <summary>
    /// Envia um email através do cliente Spitifi
    /// </summary>
    /// <remarks>
    /// Requer que o utilizador esteja logado e tenha role de administrador
    /// </remarks>
    /// <param name="ToAddress">Receiver email</param>
    /// <param name="Subject">Email Subject</param>
    /// <param name="Body">Email body text</param>
    /// <returns>
    /// <para>200 OK: Quando o email foi enviado</para>
    /// <para>400 Bad Request: Quando não foi possivel realizar o envio do email</para>
    /// </returns>
    /// <response code="200">Success</response>
    /// <response code="400">Operation Failed</response>
    [HttpPost]
    [Route("sendmail")]
    public IActionResult Send(string ToAddress, string Subject, string Body)
    {
        try
        {
            _customMailer.SendEmail(ToAddress, Subject, Body);
            return Ok("Success");
        }
        catch (Exception e)
        {
            return BadRequest("Operation Failed");
        } 
    }
}