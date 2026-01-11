using System.Collections.Generic;
using InventoryManagement.Web.Models;

namespace InventoryManagement.Web.ViewModels;

public class DashboardPageViewModel
{
    public DashboardSummaryViewModel Summary { get; set; } = new(default, default, default, default, new List<CategoryBreakdown>());
    public IEnumerable<Product> LowStockProducts { get; set; } = new List<Product>();
}
