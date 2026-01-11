using InventoryManagement.Web.Models;

namespace InventoryManagement.Web.Services;

public interface ISupplierService
{
    Task<IEnumerable<Supplier>> GetAllAsync(string? search = null);
    Task<Supplier?> GetByIdAsync(int id);
    Task<Supplier> CreateAsync(Supplier supplier);
    Task UpdateAsync(Supplier supplier);
    Task DeleteAsync(int id);
    Task<bool> HasProductsAsync(int supplierId);
}