using spitifi.Models.DbModels;

namespace spitifi.Models.ApiModels;

public class AlbumDTO
{
    public int Id { get; set; }
    public string Titulo { get; set; }
    public string Foto { get; set; }
    public int DonoFK { get; set; }
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