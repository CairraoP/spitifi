using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace spitifi.Data;

[PrimaryKey(nameof(UtilizadorFK),nameof(PlaylistFK))]
public class UtilizadorPlaylist
{
    [ForeignKey(nameof(Utilizador))]
    public int UtilizadorFK { get; set; }
    public Utilizadores Utilizador { get; set; }
    
    [ForeignKey(nameof(Playlist))]
    public int PlaylistFK { get; set; }
    public Playlist Playlist { get; set; }
}