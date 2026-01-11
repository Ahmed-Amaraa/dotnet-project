using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using InventoryManagement.Web.Data;
using InventoryManagement.Web.ViewModels;
using Microsoft.EntityFrameworkCore;

namespace InventoryManagement.Web.Services;

public class DashboardService : IDashboardService
{
    private readonly ApplicationDbContext _context;

    public DashboardService(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<DashboardSummaryViewModel> GetSummaryAsync()
    {
        var totalProducts = await _context.Products.CountAsync();
        var totalSuppliers = await _context.Suppliers.CountAsync();
        var lowStock = await _context.Products.CountAsync(p => p.Quantity <= p.ReorderLevel);
        var products = await _context.Products.ToListAsync();

        var inventoryValue = products.Sum(p => p.Quantity * p.UnitPrice);
        var categories = products
            .GroupBy(p => p.Category ?? "Uncategorized")
            .Select(g => new CategoryBreakdown(
                g.Key,
                g.Count(),
                g.Sum(p => p.Quantity * p.UnitPrice)))
            .OrderByDescending(c => c.InventoryValue)
            .Take(5)
            .ToList();

        return new DashboardSummaryViewModel(
            totalProducts,
            totalSuppliers,
            lowStock,
            inventoryValue,
            categories
        );
    }
}