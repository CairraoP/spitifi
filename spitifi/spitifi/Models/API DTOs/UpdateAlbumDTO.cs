namespace spitifi.Models.ApiModels;

public class AlbumUpdateDto
{
    public string? Titulo { get; set; }
    public IFormFile? FotoAlbum { get; set; }
    public string? ArtistaUsername { get; set; } // username do Identity
}