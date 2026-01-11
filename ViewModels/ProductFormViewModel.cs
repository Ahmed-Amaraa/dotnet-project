using System.Collections.Generic;
using InventoryManagement.Web.Models;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace InventoryManagement.Web.ViewModels;

public class ProductFormViewModel
{
    public Product Product { get; set; } = new();
    public IEnumerable<SelectListItem> Suppliers { get; set; } = new List<SelectListItem>();
}