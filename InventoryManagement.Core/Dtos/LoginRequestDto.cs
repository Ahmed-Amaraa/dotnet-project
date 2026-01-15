using System.ComponentModel.DataAnnotations;

namespace InventoryManagement.Web.Dtos;

public class LoginRequestDto
{
    [Required]
    public string Username { get; set; } = string.Empty;

    [Required]
    public string Password { get; set; } = string.Empty;

    public bool RememberMe { get; set; }
}
