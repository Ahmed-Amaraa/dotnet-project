using System;
using System.Linq;
using System.Threading.Tasks;
using InventoryManagement.Web.Data;
using InventoryManagement.Web.Models;
using InventoryManagement.Web.Services;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace InventoryManagement.Web.Tests;

public class ProductServiceTests
{
    private static ApplicationDbContext CreateContext(string databaseName)
    {
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(databaseName)
            .Options;
        return new ApplicationDbContext(options);
    }

    [Fact]
    public async Task UpdateStockAsync_ThrowsWhenQuantityWouldGoNegative()
    {
        using var context = CreateContext(nameof(UpdateStockAsync_ThrowsWhenQuantityWouldGoNegative));
        context.Products.Add(new Product
        {
            ProductId = 1,
            ProductName = "Test",
            Quantity = 1,
            ReorderLevel = 0,
            UnitPrice = 10m,
            SupplierId = 1
        });
        await context.SaveChangesAsync();

        var service = new ProductService(context);

        await Assert.ThrowsAsync<InvalidOperationException>(() => service.UpdateStockAsync(1, -2));
    }

    [Fact]
    public async Task UpdateStockAsync_IncrementsQuantityAndSetsUpdatedAt()
    {
        using var context = CreateContext(nameof(UpdateStockAsync_IncrementsQuantityAndSetsUpdatedAt));
        context.Products.Add(new Product
        {
            ProductId = 1,
            ProductName = "Test",
            Quantity = 2,
            ReorderLevel = 0,
            UnitPrice = 10m,
            SupplierId = 1
        });
        await context.SaveChangesAsync();

        var service = new ProductService(context);
        await service.UpdateStockAsync(1, 3);

        var updated = await context.Products.FirstAsync(p => p.ProductId == 1);
        Assert.Equal(5, updated.Quantity);
        Assert.NotNull(updated.UpdatedAt);
    }

    [Fact]
    public async Task GetLowStockAsync_ReturnsOnlyItemsBelowReorder()
    {
        using var context = CreateContext(nameof(GetLowStockAsync_ReturnsOnlyItemsBelowReorder));
        context.Suppliers.Add(new Supplier { SupplierId = 1, SupplierName = "Supplier" });
        context.Products.AddRange(
            new Product
            {
                ProductId = 1,
                ProductName = "Low",
                Quantity = 1,
                ReorderLevel = 2,
                UnitPrice = 5m,
                SupplierId = 1
            },
            new Product
            {
                ProductId = 2,
                ProductName = "Healthy",
                Quantity = 5,
                ReorderLevel = 2,
                UnitPrice = 5m,
                SupplierId = 1
            });
        await context.SaveChangesAsync();

        var service = new ProductService(context);
        var low = (await service.GetLowStockAsync()).ToList();

        Assert.Single(low);
        Assert.Equal(1, low[0].ProductId);
    }
}
