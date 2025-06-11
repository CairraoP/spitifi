using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace spitifi.Data.DbModels;

public class AlbumPlayList
{
    public int Id { get; set; }
    
    [ForeignKey(nameof(PlayList))]
    public int PlayListFK { get; set; }
    
    [Display(Name = "PlayList")]
    public Album PlayList { get; set; }
    
    public List<Musica> Musicas { get; set; }
}