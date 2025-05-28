using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using spitifi.Data.DbModels;

namespace spitifi.Data.DbModels;

/// <summary>
/// Utilizadores da aplicação
///
/// Contém informações não relativas ao Identity
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
    /// Utilizador criado pelo Dotnet Identity
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
    /// </summary>
    [Display(Name = "Lista de Músicas da sua Auditoria")]
    public List<Musica> ListaDono { get; set; } = [];
    
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
}