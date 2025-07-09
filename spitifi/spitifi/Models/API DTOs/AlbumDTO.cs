using System.ComponentModel.DataAnnotations;
using spitifi.Models.DbModels;

namespace spitifi.Models.ApiModels;

/// <summary>
/// DTO para output de dados de API
///
/// Utilizado para devolver informação de um albun, contendo o ID do respetivo artista
///
/// Contexto: Não podemos devolver o objeto normal, por causa do atributo Dono
///     Atributo Dono causa loops relacionais
///
/// Os atributos desta classe replicam algumas anotações da classe Album. Cuidado durante uma alteração
/// </summary>
public class AlbumDTO
{
    /// <summary>
    /// identificador unico do albúm
    /// </summary>
    public int Id { get; set; }
    
    /// <summary>
    /// Título da albúm
    /// </summary>
    [Required]
    [MaxLength(64)]
    public string Titulo { get; set; }
    
    // <summary>
    /// Fotografia do albúm
    /// Está guardada em wwwroot/imagens
    /// </summary>
    public string Foto { get; set; }
    
    /// <summary>
    /// FK para referenciar que utilizador criou o albúm
    /// </summary>
    public int DonoFK { get; set; }
    
    /// <summary>
    /// Lista de músicas pertencentes ao albúm
    /// </summary>
    public List<MusicaDTO> Musicas { get; set; }
    
    public AlbumDTO(){}
    public AlbumDTO(Album album)
    {
        Id = album.Id;
        Titulo = album.Titulo;
        Foto = album.Foto;
        DonoFK = album.DonoFK;
        Musicas = album.Musicas.Select(m => new MusicaDTO(m)).ToList();
    }
}