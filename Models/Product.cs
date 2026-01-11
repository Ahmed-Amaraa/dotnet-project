using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace InventoryManagement.Web.Models;

public class Product
{
    public int ProductId { get; set; }

    [Required, StringLength(200)]
    public string ProductName { get; set; } = string.Empty;

    [StringLength(100)]
    public string? Category { get; set; }

    [Range(0, int.MaxValue)]
    public int Quantity { get; set; }

    [Column(TypeName = "decimal(18,2)")]
    [Range(0, double.MaxValue)]
    public decimal UnitPrice { get; set; }

    [Range(0, int.MaxValue)]
    public int ReorderLevel { get; set; } = 10;

    [Display(Name = "Supplier")]
    public int SupplierId { get; set; }

    public Supplier? Supplier { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public DateTime? UpdatedAt { get; set; }

    public bool IsLowStock => Quantity <= ReorderLevel;

    public decimal InventoryValue => Quantity * UnitPrice;
}