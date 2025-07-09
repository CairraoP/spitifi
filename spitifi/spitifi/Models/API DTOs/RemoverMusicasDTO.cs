using System.ComponentModel.DataAnnotations;

namespace spitifi.Models.ApiModels;

/// <summary>
/// DTO de entrada para API
/// Usado para especificar que musicas são para ser removidas de um determinado album
///
/// Nota: ID da musicas é especificado nos parametros API
/// </summary>
public class RemoverMusicasDTO
{
    [Required]
    [MinLength(1, ErrorMessage = "Pelo menos um ID de música é obrigatório")]
    public List<int> idsMusicas { get; set; }
}