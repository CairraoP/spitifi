namespace spitifi.Services.Email;


//Classe criada para "transportar" as variáveis globais definidas na secção "EmailConf" da appsentings para usar na Interface do ICustomEmail
public class EmailSenderConfigModel
{
    public string FromEmail { get; set; }
    public string FromPassword { get; set; }
    public string Host { get; set; }
    public int Port { get; set; }
}