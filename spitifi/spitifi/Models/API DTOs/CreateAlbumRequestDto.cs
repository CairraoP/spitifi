using System.ComponentModel.DataAnnotations;

namespace spitifi.Models.ApiModels;

/// <summary>
/// DTO de entrada para API
/// Usado para criar um album novo
/// </summary>
public class CreateAlbumRequestDto
{
    /// <summary>
    /// Título da albúm
    /// </summary>
    [Required] 
    [MaxLength(64)] 
    public string Titulo { get; set; }
    
    /// <summary>
    /// Fotografia do albúm
    /// </summary>
    [Required] 
    public IFormFile FotoAlbum { get; set; }
    
    
    [MinLength(1, ErrorMessage = "Pelo menos um ID de música é obrigatório")]
    public List<IFormFile> MusicasNovas { get; set; }
}