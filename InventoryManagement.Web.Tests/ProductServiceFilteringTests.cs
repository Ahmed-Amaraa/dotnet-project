using System.Linq;
using System.Threading.Tasks;
using InventoryManagement.Web.Models;
using InventoryManagement.Web.Services;
using Xunit;

namespace InventoryManagement.Web.Tests;

public class ProductServiceFilteringTests
{
    [Fact]
    public async Task GetAllAsync_filters_and_orders()
    {
        using var context = TestDbContextFactory.Create();
        context.Suppliers.AddRange(
            new Supplier { SupplierId = 1, SupplierName = "S1" },
            new Supplier { SupplierId = 2, SupplierName = "S2" });
        context.Products.AddRange(
            new Product { ProductName = "Alpha", Category = "C1", SupplierId = 1, Quantity = 1, UnitPrice = 1m },
            new Product { ProductName = "Bravo", Category = "C2", SupplierId = 2, Quantity = 1, UnitPrice = 1m },
            new Product { ProductName = "Charlie", Category = "C1", SupplierId = 1, Quantity = 1, UnitPrice = 1m });
        await context.SaveChangesAsync();

        var service = new ProductService(context);

        var filtered = (await service.GetAllAsync(search: "a", category: "C1", supplierId: 1)).ToList();

        Assert.Equal(2, filtered.Count); // Alpha, Charlie
        Assert.True(filtered.SequenceEqual(filtered.OrderBy(p => p.ProductName))); // ordered by name
        Assert.All(filtered, p => Assert.Equal("C1", p.Category));
        Assert.All(filtered, p => Assert.Equal(1, p.SupplierId));
    }
}
