using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;
using spitifi.Data.DbModels;

namespace spitifi.Data.DbModels;

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
    [Display(Name = "Nome de Utilizador")]
    public string Username { get; set; }
    
    public string Foto { get; set; }
    
    /// <summary>
    /// Lista das músicas em que o utilizador/artista colaborou
    /// </summary>
    [Display(Name = "Lista de Colaborações")]
    public ICollection<Colabs> ListaColab { get; set; } = [];
    
    
    [Display(Name = "Lista de Músicas da sua Auditoria")]
    public ICollection<Musica> ListaDono { get; set; } = [];
    
    /// <summary>
    /// Lista e músicas que o utilizador deu "like"
    /// </summary>
    public ICollection<Gostos> ListaGostos { get; set; } = [];
    
    [StringLength(50)]
    public string? IdentityUser { get; set; }
    
    public List<Album> Albums { get; set; } = new List<Album>();
    
    
    /// <summary>
    /// quais as playlists que o utilizador segue
    /// </summary>
    public ICollection<UtilizadorPlaylist> SeguePlaylist { get; set; } = [];

    /// <summary>
    /// Lista para facilitar a visibilidade sober quais as playlists que o utilizador tem/é dono
    /// </summary>
    [NotMapped]
    public ICollection<Playlist> DonoPlaylists { get; set; } = [];
}