using System.ComponentModel.DataAnnotations;

namespace spitifi.Models.ApiModels;

public class RemoverMusicasDTO
{
    [Required]
    [MinLength(1, ErrorMessage = "Pelo menos um ID de música é obrigatório")]
    public List<int> idsMusicas { get; set; }
}