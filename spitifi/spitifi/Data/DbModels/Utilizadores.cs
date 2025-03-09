namespace spitifi.Data;

public class Utilizadores
{
    public int Id { get; set; }
    
    public string Username { get; set; }
    
    public string Email { get; set; }
    
    public ICollection<Musica> ListaColab { get; set; }
    
    public ICollection<Musica> ListaGostos { get; set; }
    
    public ICollection<Playlist> SeguePlaylist { get; set; }
    
}