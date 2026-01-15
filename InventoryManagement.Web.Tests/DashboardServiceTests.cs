using System.Linq;
using System.Threading.Tasks;
using InventoryManagement.Web.Data;
using InventoryManagement.Web.Models;
using InventoryManagement.Web.Services;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace InventoryManagement.Web.Tests;

public class DashboardServiceTests
{
    private static ApplicationDbContext CreateContext(string databaseName)
    {
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(databaseName)
            .Options;
        return new ApplicationDbContext(options);
    }

    [Fact]
    public async Task GetSummaryAsync_ReturnsExpectedAggregates()
    {
        using var context = CreateContext(nameof(GetSummaryAsync_ReturnsExpectedAggregates));

        context.Suppliers.AddRange(
            new Supplier { SupplierId = 1, SupplierName = "Alpha" },
            new Supplier { SupplierId = 2, SupplierName = "Beta" });

        context.Products.AddRange(
            new Product
            {
                ProductId = 1,
                ProductName = "A",
                Category = "C1",
                Quantity = 2,
                ReorderLevel = 1,
                UnitPrice = 10m,
                SupplierId = 1
            },
            new Product
            {
                ProductId = 2,
                ProductName = "B",
                Category = "C1",
                Quantity = 1,
                ReorderLevel = 2,
                UnitPrice = 5m,
                SupplierId = 1
            },
            new Product
            {
                ProductId = 3,
                ProductName = "C",
                Category = "C2",
                Quantity = 4,
                ReorderLevel = 3,
                UnitPrice = 2m,
                SupplierId = 2
            });

        await context.SaveChangesAsync();

        var service = new DashboardService(context);
        var summary = await service.GetSummaryAsync();

        Assert.Equal(3, summary.TotalProducts);
        Assert.Equal(2, summary.TotalSuppliers);
        Assert.Equal(1, summary.LowStockCount);
        Assert.Equal(2 * 10m + 1 * 5m + 4 * 2m, summary.InventoryValue);

        var topCategories = summary.TopCategories.ToList();
        Assert.Equal(2, topCategories.Count);
        Assert.Equal("C1", topCategories[0].Category);
        Assert.True(topCategories[0].InventoryValue >= topCategories[1].InventoryValue);
    }

    [Fact]
    public async Task GetSummaryAsync_ReturnsZerosWhenEmpty()
    {
        using var context = CreateContext(nameof(GetSummaryAsync_ReturnsZerosWhenEmpty));
        var service = new DashboardService(context);

        var summary = await service.GetSummaryAsync();

        Assert.Equal(0, summary.TotalProducts);
        Assert.Equal(0, summary.TotalSuppliers);
        Assert.Equal(0, summary.LowStockCount);
        Assert.Equal(0m, summary.InventoryValue);
        Assert.Empty(summary.TopCategories);
    }
}
