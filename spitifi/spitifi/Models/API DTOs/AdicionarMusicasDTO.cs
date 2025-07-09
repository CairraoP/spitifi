using System.ComponentModel.DataAnnotations;

namespace spitifi.Models.ApiModels;

/// <summary>
/// DTO de entrada para API
/// Usado para especificar que musicas são para ser adicionadas
///
/// Nota: ID da musicas é especificado nos parametros API
/// </summary>
public class AdicionarMusicasDTO
{
    
    /// <summary>
    /// Ficheiros de musicas para serem adicionados
    /// </summary>
    [Required]
    [MinLength(1, ErrorMessage = "Pelo menos uma música é obrigatória")]
    public List<IFormFile> musicasNovas { get; set; }
}