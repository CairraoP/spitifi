using MailKit.Net.Smtp;
using MailKit.Security;
using MimeKit;

namespace spitifi.Areas.Identity.Pages.Account;

//Criar uma interface com o comportamento para uma boa prática
public interface ICustomMailer
{
    void SendEmail(string to, string subject, string body);
}
//Classe que define como irá agiar o comportamento herdado pela interface
public class EmailSender : ICustomMailer
{
    public void SendEmail(string email, string subject, string htmlMessage)
    {
        var fromEmail = "equipaspitifi@gmail.com";
        var fromPassword = "liaw tckc gjco izlf";

        var message = new MimeMessage();
        message.From.Add(new MailboxAddress("Equipa Spitifi", fromEmail));
        message.To.Add(new MailboxAddress(email, email));
        message.Subject = subject;
        message.Body = new TextPart("plain") { Text = htmlMessage };

        using (var client = new SmtpClient())
        { 
            client.ConnectAsync("smtp.gmail.com", 587, SecureSocketOptions.StartTls);
            client.Authenticate(fromEmail, fromPassword);
            client.Send(message);
            client.Disconnect(true);
        }
    }
}