using System.Collections.Generic;
using InventoryManagement.Web.Models;

namespace InventoryManagement.Web.ViewModels;

public class ProductListViewModel
{
    public IEnumerable<Product> Products { get; set; } = new List<Product>();
    public IEnumerable<Supplier> Suppliers { get; set; } = new List<Supplier>();
    public string? Search { get; set; }
    public string? Category { get; set; }
    public int? SupplierId { get; set; }
}
