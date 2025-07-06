using spitifi.Models.DbModels;

namespace spitifi.Models.ApiModels;

public class MusicaDTO
{
    public int Id { get; set; }
    public string Nome { get; set; }
    public int AlbumFK { get; set; }
    public string FilePath { get; set; }
    public int DonoFK { get; set; }
    
    public MusicaDTO() {}
    
    public MusicaDTO(Musica musica)
    {
        Id = musica.Id;
        Nome = musica.Nome;
        FilePath = musica.FilePath;
        DonoFK = musica.DonoFK;
    }
}