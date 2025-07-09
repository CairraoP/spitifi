using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace spitifi.Models.DbModels;

/// <summary>
/// Playlists são compilações de musicas existentes (criadas por artistas)
/// Qualquer utilizador com autenticado pode criar uma compilação
///
/// Para contexto: A criação de musicas novas tem de ser via um album. Utilizador tem de ter role de artista
/// </summary>
public class PlayList
{
    /// <summary>
    /// identificador unico da playlist
    /// </summary>
    public int Id { get; set; }
    
    /// <summary>
    /// Título da playlist
    /// </summary>
    [Display(Name = "Playlist")]
    [Required]
    [MaxLength(64)]
    public string Nome { get; set; }
    
    public string Foto { get; set; }
    
    /// <summary>
    /// FK para referenciar que utilizador criou o albúm
    /// Proposito: para facilidade de acesso ao ID do utilizador
    /// </summary>
    [Display(Name = "Dono")]
    [ForeignKey(nameof(Dono))]
    public int DonoFK { get; set; }
    
    /// <summary>
    /// Utilizador que criou o albúm
    /// </summary>
    public Utilizadores Dono { get; set; }
    
    /// <summary>
    /// Lista de músicas pertencentes ao albúm
    /// </summary>
    public ICollection<Musica> Musicas { get; set; } = [];
}