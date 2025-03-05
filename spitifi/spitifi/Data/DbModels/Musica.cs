namespace spitifi.Data;

public class Musica
{
    public int Id { get; set; }
    
    public string Nome { get; set; }
    
    public string Album { get; set; }
    
    public string Editora { get; set; }
    
    public ICollection<Utilizadores> ListaColab { get; set; }
    
    public ICollection<Playlist> ListaPlaylist { get; set; }
    
    public Utilizadores Dono { get; set; }
}