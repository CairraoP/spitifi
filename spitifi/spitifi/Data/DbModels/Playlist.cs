using System.ComponentModel.DataAnnotations.Schema;

namespace spitifi.Data;

public class Playlist
{
    public int Id { get; set; }
    
    public string Nome { get; set; }
    
    public ICollection<Musica> ListaPlaylist { get; set; }
    
    public ICollection<Utilizadores> SeguePlaylist { get; set; }
    
    [ForeignKey(nameof(Dono))]
    public int DonoFK { get; set; }
    
    public Utilizadores Dono { get; set; }
}