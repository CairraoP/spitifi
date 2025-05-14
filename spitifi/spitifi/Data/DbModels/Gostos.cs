using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace spitifi.Data.DbModels;

/// <summary>
/// Gostos de músicas
/// </summary>
[PrimaryKey(nameof(UtilizadorFK),nameof(MusicaFK))]
public class Gostos
{
    /// <summary>
    /// FK do utilizador que gostou da música
    /// </summary>
    [ForeignKey(nameof(Utilizador))]
    public int UtilizadorFK { get; set; }
    
    /// <summary>
    /// Utilizador que gostou da música 
    /// </summary>
    public Utilizadores Utilizador { get; set; }
    
    /// <summary>
    /// FK da música que levou gosto
    /// </summary>
    [ForeignKey(nameof(Musica))]
    public int MusicaFK { get; set; }
    
    /// <summary>
    /// Música que levou gosto
    /// </summary>
    public Musica Musica { get; set; }
}