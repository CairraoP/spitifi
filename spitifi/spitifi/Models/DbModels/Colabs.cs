using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace spitifi.Models.DbModels;


/// <summary>
/// Varios artistas podem colaborar numa unica musica
/// Esta classe guarda a informação de quais artistas colaboraram em quais musica
/// A classe musica somente referencia o artista que criou a musica
/// </summary>
[PrimaryKey(nameof(UtilizadorFK),nameof(MusicaFK))]
public class Colabs
{
    /// <summary>
    /// FK para referenciar que artista co-participou numa musica
    /// Proposito: para facilidade de acesso ao ID do utilizador
    /// </summary>
    [ForeignKey(nameof(Utilizador))] public int UtilizadorFK { get; set; }
    
    /// <summary>
    /// Artista participou numa musica
    /// </summary>
    public Utilizadores Utilizador { get; set; }
    
    /// <summary>
    /// FK para referenciar que musica o artista colaborou
    /// Proposito: para facilidade de acesso ao ID da musica
    /// </summary>
    [ForeignKey(nameof(Musica))] public int MusicaFK { get; set; }
    
    /// <summary>
    /// Musica qual artista colaborou
    /// </summary>
    public Musica Musica { get; set; }
}