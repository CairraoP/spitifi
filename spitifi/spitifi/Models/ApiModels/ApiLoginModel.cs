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
    /// <example>ahh@uuh.duh</example>
    [Required]
    public string Username { get; set; }
    
    /// <summary>
    /// Palavra passe do utilizador no identity
    /// </summary>
    /// <example>notversecurepassword</example>
    [Required]
    public string Password { get; set; }
}