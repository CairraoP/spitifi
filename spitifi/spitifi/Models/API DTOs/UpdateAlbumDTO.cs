using System.ComponentModel.DataAnnotations;

namespace spitifi.Models.ApiModels;

/// <summary>
/// DTO de entrada para API
/// Usado para alterar dados do Album
///
/// Nota: ID do album é especificado nos parametros API
/// </summary>
public class AlbumUpdateDto
{
    
    /// <summary>
    /// Título da albúm
    /// </summary>
    [MaxLength(64)]
    public string? Titulo { get; set; }
    
    /// <summary>
    /// Fotografia do albúm
    /// Está guardada em wwwroot/imagens
    /// </summary>
    public IFormFile? FotoAlbum { get; set; }
    
    /// <summary>
    /// Username do utilizador do Identity
    /// </summary>
    
    [MaxLength(255)]
    public string? ArtistaUsername { get; set; } 
}