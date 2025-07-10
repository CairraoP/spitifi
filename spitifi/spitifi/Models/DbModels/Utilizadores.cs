using System.ComponentModel.DataAnnotations;

namespace spitifi.Models.DbModels;

/// <summary>
/// Utilizadores da aplicação
///
/// Contém informações não relativas ao Identity
///
/// !!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
/// !!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
/// ALTERAÇÕES AOS ATRIBUTOS NESTA CLASSE TÊM DE SER REFLETIDOS NOUTRAS CLASSES, EM PARTICULAR DTOs
///     Nomeadamente:
///        - UserRoleUpdate
/// !!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
/// !!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
/// 
/// </summary>
public class Utilizadores
{
    /// <summary>
    /// identificador único do utilizador
    /// </summary>
    public int Id { get; set; }
    
    /// <summary>
    /// nome do utilizador
    /// </summary>
    [Required(ErrorMessage = "O nome do utilizador é de preenchimento obrigatório.")] 
    [Display(Name = "Nome de Utilizador")]
    public string Username { get; set; }
    
    /// <summary>
    /// foto do perfil do utilizador 
    /// </summary>
    public string Foto { get; set; }
    
    /// <summary>
    /// uid gerado do utilizador criado pelo Dotnet Identity
    /// </summary>
    [StringLength(50)]
    public string? IdentityUser { get; set; }
    
    /// <summary>
    /// utilizador á um artista
    /// </summary>
    [Display(Name = "Artista?")]
    public bool IsArtista { get; set; }
    
    /* *************************
     * Definção dos relacionamentos
     * **************************
     */
    
    /// <summary>
    /// Lista de músicas criadas pelo utilizador
    /// Pendente possuir role de artista
    /// </summary>
    [Display(Name = "Lista de Músicas da sua Auditoria")]
    public List<Musica> ListaDono { get; set; } = [];
    
    /// <summary>
    /// Lista de músicas que este utilizador colaborou
    /// Contexto: Colab são musicas de auditoria de um outro utilizador, mas que este tambem participou
    /// </summary>
    [Display(Name = "Lista de Músicas da sua Auditoria")]
    public List<Colabs> ListaColabs { get; set; } = [];
    
    /// <summary>
    /// Lista e músicas que o utilizador deu "like"
    /// </summary>
    public List<Gostos> ListaGostos { get; set; } = [];
    
    /// <summary>
    /// Albúms criados pelo Utilizador
    /// </summary>
    public List<Album> Albums { get; set; } = new List<Album>();
    
    /// <summary>
    /// Lista de playlists criadas pelo utilizador
    /// </summary>
    public List<PlayList> Playlists { get; set; } = new List<PlayList>();
}