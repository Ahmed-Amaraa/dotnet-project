using System.ComponentModel.DataAnnotations;

namespace InventoryManagement.Web.Dtos;

public class SupplierCreateUpdateDto
{
    [Required, StringLength(200)]
    public string SupplierName { get; set; } = string.Empty;

    [Phone, StringLength(50)]
    public string? ContactNumber { get; set; }

    [EmailAddress, StringLength(200)]
    public string? Email { get; set; }

    [StringLength(300)]
    public string? Address { get; set; }
}
