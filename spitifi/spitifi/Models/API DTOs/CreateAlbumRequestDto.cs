using System.ComponentModel.DataAnnotations;

namespace spitifi.Models.ApiModels;

public class CreateAlbumRequestDto
{
    [Required] public string Titulo { get; set; }
    [Required] public IFormFile FotoAlbum { get; set; }
    [MinLength(1)] public List<IFormFile> MusicasNovas { get; set; }
}