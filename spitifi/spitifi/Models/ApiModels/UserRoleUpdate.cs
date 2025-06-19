using System.ComponentModel.DataAnnotations;

namespace spitifi.Models.ApiModels;

public class UserRoleUpdate
{
    [Required]
    public string username { get; set; }
}