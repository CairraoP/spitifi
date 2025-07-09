using System.ComponentModel.DataAnnotations;

namespace spitifi.Models.ApiModels;

/// <summary>
/// DTO de entrada para endpoints dedicados a alteração de userroles do identity
/// </summary>
public class UserRoleUpdate
{
    /// <summary>
    /// Identificador do utilizador, Identity username
    /// </summary>
    /// <example>buuuuuuuh</example>
    [Required]
    [MaxLength(50)]
    public string Username { get; set; }
}