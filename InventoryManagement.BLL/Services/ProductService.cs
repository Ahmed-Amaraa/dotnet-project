using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using InventoryManagement.Web.Data;
using InventoryManagement.Web.Models;
using Microsoft.EntityFrameworkCore;

namespace InventoryManagement.Web.Services;

public class ProductService : IProductService
{
    private readonly ApplicationDbContext _context;

    public ProductService(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Product>> GetAllAsync(string? search = null, string? category = null, int? supplierId = null)
    {
        var query = _context.Products
            .Include(p => p.Supplier)
            .AsNoTracking()
            .AsQueryable();

        if (!string.IsNullOrWhiteSpace(search))
        {
            query = query.Where(p => p.ProductName.Contains(search));
        }

        if (!string.IsNullOrWhiteSpace(category))
        {
            query = query.Where(p => p.Category == category);
        }

        if (supplierId.HasValue)
        {
            query = query.Where(p => p.SupplierId == supplierId.Value);
        }

        return await query
            .OrderBy(p => p.ProductName)
            .ToListAsync();
    }

    public async Task<Product?> GetByIdAsync(int id)
    {
        return await _context.Products
            .Include(p => p.Supplier)
            .AsNoTracking()
            .FirstOrDefaultAsync(p => p.ProductId == id);
    }

    public async Task<Product> CreateAsync(Product product)
    {
        product.CreatedAt = DateTime.UtcNow;
        product.UpdatedAt = DateTime.UtcNow;
        _context.Products.Add(product);
        await _context.SaveChangesAsync();
        return product;
    }

    public async Task UpdateAsync(Product product)
    {
        var existing = await _context.Products.FirstOrDefaultAsync(p => p.ProductId == product.ProductId)
            ?? throw new KeyNotFoundException("Product not found");

        existing.ProductName = product.ProductName;
        existing.Category = product.Category;
        existing.Quantity = product.Quantity;
        existing.UnitPrice = product.UnitPrice;
        existing.ReorderLevel = product.ReorderLevel;
        existing.SupplierId = product.SupplierId;
        existing.UpdatedAt = DateTime.UtcNow;

        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(int id)
    {
        var existing = await _context.Products.FirstOrDefaultAsync(p => p.ProductId == id)
            ?? throw new KeyNotFoundException("Product not found");

        _context.Products.Remove(existing);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateStockAsync(int id, int amount)
    {
        var existing = await _context.Products.FirstOrDefaultAsync(p => p.ProductId == id)
            ?? throw new KeyNotFoundException("Product not found");

        var newQuantity = existing.Quantity + amount;
        if (newQuantity < 0)
        {
            throw new InvalidOperationException("Quantity cannot be negative");
        }

        existing.Quantity = newQuantity;
        existing.UpdatedAt = DateTime.UtcNow;

        await _context.SaveChangesAsync();
    }

    public async Task<IEnumerable<Product>> GetLowStockAsync()
    {
        return await _context.Products
            .Include(p => p.Supplier)
            .AsNoTracking()
            .Where(p => p.Quantity <= p.ReorderLevel)
            .OrderBy(p => p.Quantity)
            .ToListAsync();
    }
}