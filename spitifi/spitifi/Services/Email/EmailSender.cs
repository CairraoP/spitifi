using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.Options;
using MimeKit;
using spitifi.Areas.Identity.Pages.Account;

namespace spitifi.Services.Email;

//Criar uma interface com o comportamento para uma boa prática
public interface ICustomMailer
{
    void SendEmail(string to, string subject, string body);
}

//Classe que define como irá agir o comportamento herdado pela interface e que nos irá criar o email e coneão com o client pelo protocolo smtp
public class CustomMailer : ICustomMailer
{
    private EmailSenderConfigModel _mailConfig;

    //Não é importado um serviço, mas é importado uma configuração logo é necessário usar o IOption
    // É diferente da maneira que costumamos importar (O DbContext importa um serviço)
    public CustomMailer(IOptions<EmailSenderConfigModel> mailConfig)
    {
        _mailConfig = mailConfig.Value;
    }

    public void SendEmail(string email, string subject, string htmlMessage)
    {
        //variáveis provenientes da classe do EmailSenderConfigModel
        var fromEmail = _mailConfig.fromEmail;
        var fromPassword = _mailConfig.fromPassword;

        var message = new MimeMessage();
        message.From.Add(new MailboxAddress("Equipa Spitifi", fromEmail));
        message.To.Add(new MailboxAddress(email, email));
        message.Subject = subject;

        // Redefinido a maneira como definimos o Body do email
        // para melhorar a visualização do email e ainda poderemos
        // no futuro usar/passar ficheiros pelo email caso queiramos
        var bodyBuilder = new BodyBuilder();
        bodyBuilder.HtmlBody = htmlMessage;
        message.Body = bodyBuilder.ToMessageBody();

        using (var client = new SmtpClient())
        { 
            client.Connect(_mailConfig.host, _mailConfig.port, SecureSocketOptions.StartTls);
            client.Authenticate(fromEmail, fromPassword);
            client.Send(message);
            client.Disconnect(true);
        }
    }
}