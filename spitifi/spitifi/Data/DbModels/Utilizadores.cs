namespace spitifi.Data;

public class Utilizadores
{
    public int Id { get; set; }
    
    public string Username { get; set; }
    
    public string Email { get; set; }
    
    public ICollection<Musica> ListaMusicas { get; set; }
    
}