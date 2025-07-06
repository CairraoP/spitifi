using System.ComponentModel.DataAnnotations;

namespace spitifi.Models.ApiModels;

/// <summary>
/// DTO de entrada para endpoint de login por JWT
/// </summary>
public class ApiLoginModel
{
    /// <summary>
    /// Identificador do utilizador, Identity username
    /// </summary>
    /// <example>nerd</example>
    [Required]
    public string Username { get; set; }
    
    /// <summary>
    /// Palavra passe do utilizador no identity
    /// </summary>
    /// <example>Pedro25#</example>
    [Required]
    public string Password { get; set; }
}