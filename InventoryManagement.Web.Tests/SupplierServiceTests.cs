using System.Threading.Tasks;
using InventoryManagement.Web.Models;
using InventoryManagement.Web.Services;
using Xunit;

namespace InventoryManagement.Web.Tests;

public class SupplierServiceTests
{
    [Fact]
    public async Task Supplier_crud_and_has_products_work()
    {
        using var context = TestDbContextFactory.Create();
        var service = new SupplierService(context);

        // Create
        var created = await service.CreateAsync(new Supplier { SupplierName = "ACME", Email = "a@b.com" });
        Assert.NotEqual(0, created.SupplierId);
        Assert.True(created.CreatedAt <= created.UpdatedAt);

        // Update
        created.SupplierName = "ACME Inc";
        await service.UpdateAsync(created);
        var updated = await service.GetByIdAsync(created.SupplierId);
        Assert.Equal("ACME Inc", updated!.SupplierName);

        // HasProducts false
        var hasProductsFalse = await service.HasProductsAsync(created.SupplierId);
        Assert.False(hasProductsFalse);

        // Add product directly and re-check
        context.Products.Add(new Product { ProductName = "Widget", SupplierId = created.SupplierId, Quantity = 1, ReorderLevel = 0, UnitPrice = 1m });
        await context.SaveChangesAsync();
        Assert.True(await service.HasProductsAsync(created.SupplierId));

        // Delete
        context.Products.RemoveRange(context.Products);
        await context.SaveChangesAsync();

        await service.DeleteAsync(created.SupplierId);
        Assert.Null(await service.GetByIdAsync(created.SupplierId));

        // Guards
        await Assert.ThrowsAsync<KeyNotFoundException>(() => service.UpdateAsync(new Supplier { SupplierId = 999, SupplierName = "Missing" }));
        await Assert.ThrowsAsync<KeyNotFoundException>(() => service.DeleteAsync(999));
    }

    [Fact]
    public async Task GetAllAsync_filters_and_orders_by_name()
    {
        using var context = TestDbContextFactory.Create();
        context.Suppliers.AddRange(
            new Supplier { SupplierName = "Bravo" },
            new Supplier { SupplierName = "Alpha" },
            new Supplier { SupplierName = "Gamma" });
        await context.SaveChangesAsync();

        var service = new SupplierService(context);
        var filtered = await service.GetAllAsync(search: "a");

        var list = filtered.ToList();
        Assert.Equal(3, list.Count);
        Assert.True(list.SequenceEqual(list.OrderBy(s => s.SupplierName)));
    }
}
