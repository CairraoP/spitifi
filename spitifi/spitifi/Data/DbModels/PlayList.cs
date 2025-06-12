using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace spitifi.Data.DbModels;

public class PlayList
{
    public int Id { get; set; }
    
    public string Nome { get; set; }
    
    public string Foto { get; set; }
    /// <summary>
    /// FK para referenciar que utilizador criou a playlist
    /// </summary>
    [Display(Name = "Dono")]
    [ForeignKey(nameof(Dono))]
    public int DonoFK { get; set; }
    
    /// <summary>
    /// Utilizador que criou o albúm
    /// </summary>
    public Utilizadores Dono { get; set; }
    
    public ICollection<Musica> Musicas { get; set; } = [];
}