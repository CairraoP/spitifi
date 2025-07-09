using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace spitifi.Models.DbModels;

/// <summary>
/// Albuns são compostos por musicas.
/// Utilizadores com role de artista podem criar albuns.
/// A criação de um album implica o upload de musicas para o servidor
///
/// Para contexto: Ao contrario, a classe Playlist, são simples compilações de musicas criadas por artistas
/// Baseiam-se na musicas que pertencem a albuns
///
/// !!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
/// !!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
/// ALTERAÇÕES AOS ATRIBUTOS NESTA CLASSE TÊM DE SER REFLETIDOS NOUTRAS CLASSES, EM PARTICULAR DTOs 
///     Nomeadamente:
///        - AlbumDTO
///        - AlbumUpdateDto
///        - CreateAlbumRequestDto
/// !!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
/// !!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
/// </summary>
public class Album
{
    /// <summary>
    /// identificador unico do albúm
    /// </summary>
    public int Id { get; set; }
    
    /// <summary>
    /// Título da albúm
    ///
    /// Nota: deveria de ter sido posto um REGEX para a validação do Nome, como não foi e para não deitar a abase de dados abaixo, vamos tratar disso
    /// após a criação do album
    /// </summary>
    [Display(Name = "Albúm")]
    [Required]
    [MaxLength(64)]
    public string Titulo { get; set; }
    
    /// <summary>
    /// Fotografia do albúm
    /// Está guardada em wwwroot/imagens
    /// </summary>
    public string Foto { get; set; }
    
    /* *************************
     * Definição dos relacionamentos
     * **************************
     */
    
    /// <summary>
    /// FK para referenciar que utilizador criou o albúm
    /// Proposito: para facilidade de acesso ao ID do utilizador
    /// </summary>
    [Display(Name = "Artista")]
    [ForeignKey(nameof(Dono))]
    public int DonoFK { get; set; }
    
    /// <summary>
    /// Utilizador que criou o albúm
    /// </summary>
    public Utilizadores Dono { get; set; }
    
    
    /// <summary>
    /// Lista de músicas pertencentes ao albúm
    /// </summary>
    public List<Musica> Musicas { get; set; } = new  List<Musica>();
}