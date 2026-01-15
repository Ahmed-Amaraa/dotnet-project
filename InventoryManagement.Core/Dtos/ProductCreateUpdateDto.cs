using System.ComponentModel.DataAnnotations;

namespace InventoryManagement.Web.Dtos;

public class ProductCreateUpdateDto
{
    [Required, StringLength(200)]
    public string ProductName { get; set; } = string.Empty;

    [StringLength(100)]
    public string? Category { get; set; }

    [Range(0, int.MaxValue)]
    public int Quantity { get; set; }

    [Range(0, double.MaxValue)]
    public decimal UnitPrice { get; set; }

    [Range(0, int.MaxValue)]
    public int ReorderLevel { get; set; } = 10;

    [Required]
    public int SupplierId { get; set; }
}
