namespace spitifi.Services.Email;


//Classe criada para "transportar" as variáveis globais definidas na secção "EmailConf" da appsentings para usar na Interface do ICustomEmail
public class EmailSenderConfigModel
{
    public string fromEmail { get; set; }
    public string fromPassword { get; set; }
    public string host { get; set; }
    public int port { get; set; }
}