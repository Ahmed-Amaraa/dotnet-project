using System.Linq;
using System.Threading.Tasks;
using InventoryManagement.Web.Models;
using InventoryManagement.Web.Services;
using Xunit;

namespace InventoryManagement.Web.Tests;

public class ProductServiceLowStockOrderingTests
{
    [Fact]
    public async Task GetLowStockAsync_orders_by_quantity_and_filters()
    {
        using var context = TestDbContextFactory.Create();
        context.Suppliers.Add(new Supplier { SupplierId = 1, SupplierName = "Seed" });
        context.Products.AddRange(
            new Product { ProductName = "Low1", Quantity = 1, ReorderLevel = 2, UnitPrice = 1m, SupplierId = 1 },
            new Product { ProductName = "Low2", Quantity = 0, ReorderLevel = 1, UnitPrice = 1m, SupplierId = 1 },
            new Product { ProductName = "Ok", Quantity = 5, ReorderLevel = 2, UnitPrice = 1m, SupplierId = 1 });
        await context.SaveChangesAsync();

        var service = new ProductService(context);
        var low = (await service.GetLowStockAsync()).ToList();

        Assert.Equal(2, low.Count); // Low2, Low1
        Assert.Equal(new[] { "Low2", "Low1" }, low.Select(p => p.ProductName).ToArray());
    }
}
