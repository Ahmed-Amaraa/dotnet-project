using System;
using System.Threading.Tasks;
using InventoryManagement.Web.Models;
using InventoryManagement.Web.Services;
using Xunit;

namespace InventoryManagement.Web.Tests;

public class ProductServiceStockBoundaryTests
{
    [Fact]
    public async Task Stock_updates_handle_positive_zero_and_negative()
    {
        using var context = TestDbContextFactory.Create();
        context.Suppliers.Add(new Supplier { SupplierId = 1, SupplierName = "Seed" });
        context.Products.Add(new Product
        {
            ProductName = "Stocked",
            Quantity = 2,
            ReorderLevel = 1,
            UnitPrice = 1m,
            SupplierId = 1
        });
        await context.SaveChangesAsync();

        var service = new ProductService(context);

        // Positive delta
        var productId = context.Products.Single().ProductId;

        await service.UpdateStockAsync(productId, 3);
        var afterIncrease = await service.GetByIdAsync(productId);
        Assert.Equal(5, afterIncrease!.Quantity);
        Assert.NotNull(afterIncrease.UpdatedAt);

        // Zero delta (should succeed and update timestamp)
        var beforeZeroUpdate = afterIncrease.UpdatedAt;
        await service.UpdateStockAsync(productId, 0);
        var afterZero = await service.GetByIdAsync(productId);
        Assert.Equal(5, afterZero!.Quantity);
        Assert.True(afterZero.UpdatedAt >= beforeZeroUpdate);

        // Negative below zero should throw
        await Assert.ThrowsAsync<InvalidOperationException>(() => service.UpdateStockAsync(productId, -10));
    }
}
