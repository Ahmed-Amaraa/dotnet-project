using System.Threading.Tasks;
using System.Linq;
using InventoryManagement.Web.Dtos;
using InventoryManagement.Web.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace InventoryManagement.Web.Controllers.Api;

[Authorize(Roles = "Admin")]
[ApiController]
[Route("api/[controller]")]
public class DashboardController : ControllerBase
{
    private readonly IDashboardService _dashboardService;
    private readonly IProductService _productService;

    public DashboardController(IDashboardService dashboardService, IProductService productService)
    {
        _dashboardService = dashboardService;
        _productService = productService;
    }

    [HttpGet("summary")]
    public async Task<IActionResult> GetSummary()
    {
        var summary = await _dashboardService.GetSummaryAsync();
        return Ok(summary);
    }

    [HttpGet("low-stock")]
    public async Task<IActionResult> GetLowStock()
    {
        var lowStock = await _productService.GetLowStockAsync();
        var dtos = lowStock.Select(p => new ProductDto(
            p.ProductId,
            p.ProductName,
            p.Category,
            p.Quantity,
            p.UnitPrice,
            p.ReorderLevel,
            p.SupplierId,
            p.Supplier?.SupplierName ?? string.Empty,
            p.IsLowStock,
            p.InventoryValue));
        return Ok(dtos);
    }
}
