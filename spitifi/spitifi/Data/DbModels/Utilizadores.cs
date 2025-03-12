using System.ComponentModel.DataAnnotations.Schema;

namespace spitifi.Data;

/// <summary>
/// classe para o utilizador
/// </summary>
public class Utilizadores
{
    /// <summary>
    /// chave prmária auto-increment
    /// </summary>
    public int Id { get; set; }
    
    /// <summary>
    /// Nome do utilizador que irá ser atribuido, o seu "nickname"
    /// </summary>
    public string Username { get; set; }
    
    /// <summary>
    /// Lista das músicas em que o utilizador/artista colaborou
    /// </summary>
    public ICollection<Colabs> ListaColab { get; set; }
    
    public ICollection<Musica> ListaDono { get; set; }
    
    /// <summary>
    /// Lista e músicas que o utilizador deu "like"
    /// </summary>
    public ICollection<Gostos> ListaGostos { get; set; }
    
    /// <summary>
    /// quais as playlists que o utilizador segue
    /// </summary>
    public ICollection<UtilizadorPlaylist> SeguePlaylist { get; set; }
    
    /// <summary>
    /// Lista para facilitar a visibilidade sober quais as playlists que o utilizador tem/é dono
    /// </summary>
    [NotMapped]
    public ICollection<Playlist> DonoPlaylists { get; set; }
    
    //public ICollection<Musica> ListaMusica { get; set;}
}