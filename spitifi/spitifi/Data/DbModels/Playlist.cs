namespace spitifi.Data;

public class Playlist
{
    public int Id { get; set; }
    
    public string Nome { get; set; }
    
    public Utilizadores Dono { get; set; }
    
    public ICollection<Musica> ListaMusica { get; set; }
    
}