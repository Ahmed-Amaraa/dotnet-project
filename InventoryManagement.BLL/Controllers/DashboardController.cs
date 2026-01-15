using System.Threading.Tasks;
using InventoryManagement.Web.Services;
using InventoryManagement.Web.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace InventoryManagement.Web.Controllers;

[Authorize(Roles = "Admin")]
public class DashboardController : Controller
{
    private readonly IDashboardService _dashboardService;
    private readonly IProductService _productService;

    public DashboardController(IDashboardService dashboardService, IProductService productService)
    {
        _dashboardService = dashboardService;
        _productService = productService;
    }

    public async Task<IActionResult> Index()
    {
        var summary = await _dashboardService.GetSummaryAsync();
        var lowStockProducts = await _productService.GetLowStockAsync();

        var viewModel = new DashboardPageViewModel
        {
            Summary = summary,
            LowStockProducts = lowStockProducts
        };

        return View(viewModel);
    }
}
