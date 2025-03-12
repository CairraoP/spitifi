using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace spitifi.Data;

[PrimaryKey(nameof(UtilizadorFK),nameof(MusicaFK))]
public class Gostos
{
    [ForeignKey(nameof(Utilizador))]
    public int UtilizadorFK { get; set; }
    public Utilizadores Utilizador { get; set; }
    
    [ForeignKey(nameof(Musica))]
    public int MusicaFK { get; set; }
    public Musica Musica { get; set; }
}