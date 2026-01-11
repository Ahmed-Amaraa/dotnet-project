using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace InventoryManagement.Web.Models;

public class Supplier
{
    public int SupplierId { get; set; }

    [Required, StringLength(200)]
    public string SupplierName { get; set; } = string.Empty;

    [Phone, StringLength(50)]
    public string? ContactNumber { get; set; }

    [EmailAddress, StringLength(200)]
    public string? Email { get; set; }

    [StringLength(300)]
    public string? Address { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public DateTime? UpdatedAt { get; set; }

    public ICollection<Product> Products { get; set; } = new List<Product>();
}