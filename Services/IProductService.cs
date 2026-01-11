using InventoryManagement.Web.Models;

namespace InventoryManagement.Web.Services;

public interface IProductService
{
    Task<IEnumerable<Product>> GetAllAsync(string? search = null, string? category = null, int? supplierId = null);
    Task<Product?> GetByIdAsync(int id);
    Task<Product> CreateAsync(Product product);
    Task UpdateAsync(Product product);
    Task DeleteAsync(int id);
    Task UpdateStockAsync(int id, int amount);
    Task<IEnumerable<Product>> GetLowStockAsync();
}