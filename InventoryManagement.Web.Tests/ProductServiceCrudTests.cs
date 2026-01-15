using System;
using System.Threading.Tasks;
using InventoryManagement.Web.Models;
using InventoryManagement.Web.Services;
using Xunit;

namespace InventoryManagement.Web.Tests;

public class ProductServiceCrudTests
{
    [Fact]
    public async Task Product_crud_and_guards_work()
    {
        using var context = TestDbContextFactory.Create();
        context.Suppliers.Add(new Supplier { SupplierId = 1, SupplierName = "Seed" });
        await context.SaveChangesAsync();
        var service = new ProductService(context);

        // Create
        var created = await service.CreateAsync(new Product
        {
            ProductName = "Widget",
            Category = "Tools",
            Quantity = 5,
            ReorderLevel = 2,
            UnitPrice = 10m,
            SupplierId = 1
        });

        Assert.NotEqual(0, created.ProductId);
        Assert.True(created.CreatedAt <= created.UpdatedAt);

        // Get
        var fetched = await service.GetByIdAsync(created.ProductId);
        Assert.NotNull(fetched);
        Assert.Equal("Widget", fetched!.ProductName);

        // Update
        created.ProductName = "Widget Pro";
        created.Quantity = 8;
        await service.UpdateAsync(created);

        var updated = await service.GetByIdAsync(created.ProductId);
        Assert.Equal("Widget Pro", updated!.ProductName);
        Assert.Equal(8, updated.Quantity);
        Assert.True(updated.UpdatedAt >= created.CreatedAt);

        // Delete
        await service.DeleteAsync(created.ProductId);
        var afterDelete = await service.GetByIdAsync(created.ProductId);
        Assert.Null(afterDelete);

        // Guard: update missing
        await Assert.ThrowsAsync<KeyNotFoundException>(() => service.UpdateAsync(new Product { ProductId = 999, ProductName = "Missing" }));
        // Guard: delete missing
        await Assert.ThrowsAsync<KeyNotFoundException>(() => service.DeleteAsync(999));
    }
}
