using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;

namespace spitifi.Data.DbModels;

public class Album
{
    public int Id { get; set; }
    
    [Display(Name = "Albúm")]
    public string Titulo { get; set; }
    
    public string Foto { get; set; }
    
    
    [Display(Name = "Artista")]
    [ForeignKey(nameof(Dono))]
    public int DonoFK { get; set; }
    public Utilizadores Dono { get; set; }

    public List<Musica> Musicas { get; set; } = new  List<Musica>();
}