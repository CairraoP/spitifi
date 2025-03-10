using System.ComponentModel.DataAnnotations.Schema;

namespace spitifi.Data;

public class Musica
{
    public int Id { get; set; }
    
    public string Nome { get; set; }
    
    public string Album { get; set; }
    
    public ICollection<Utilizadores> ListaColab { get; set; }
    
    public ICollection<Playlist> ListaPlaylist { get; set; }

    public ICollection<Utilizadores> ListaGostos { get; set; }

    [ForeignKey(nameof(Dono))]
    public int DonoFK { get; set; }
    
    public Utilizadores Dono { get; set; }
}