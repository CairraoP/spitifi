using System.ComponentModel.DataAnnotations;

namespace spitifi.Models.ApiModels;

/// <summary>
/// DTO de entrada para endpoints de autenticação por JWT
/// </summary>
public class ApiLoginModel
{
    /// <summary>
    /// Identificador do utilizador, Identity username
    /// </summary>
    /// <example>nerd</example>
    [Required]
    [MaxLength(50)]
    public string Username { get; set; }
    
    /// <summary>
    /// Palavra passe do utilizador no identity
    /// </summary>
    /// <example>Pedro25#</example>
    [Required]
    [MaxLength(255)]
    public string Password { get; set; }
}