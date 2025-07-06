using System.ComponentModel.DataAnnotations;

namespace spitifi.Models.ApiModels;

public class AdicionarMusicasDTO
{
    [Required]
    [MinLength(1, ErrorMessage = "Pelo menos uma música é obrigatória")]
    public List<IFormFile> musicasNovas { get; set; }
}