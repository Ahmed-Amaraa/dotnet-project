using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using InventoryManagement.Web.Data;
using InventoryManagement.Web.Models;
using Microsoft.EntityFrameworkCore;

namespace InventoryManagement.Web.Services;

public class SupplierService : ISupplierService
{
    private readonly ApplicationDbContext _context;

    public SupplierService(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Supplier>> GetAllAsync(string? search = null)
    {
        var query = _context.Suppliers
            .Include(s => s.Products)
            .AsNoTracking()
            .AsQueryable();

        if (!string.IsNullOrWhiteSpace(search))
        {
            query = query.Where(s => s.SupplierName.Contains(search));
        }

        return await query
            .OrderBy(s => s.SupplierName)
            .ToListAsync();
    }

    public async Task<Supplier?> GetByIdAsync(int id)
    {
        return await _context.Suppliers
            .Include(s => s.Products)
            .AsNoTracking()
            .FirstOrDefaultAsync(s => s.SupplierId == id);
    }

    public async Task<Supplier> CreateAsync(Supplier supplier)
    {
        supplier.CreatedAt = DateTime.UtcNow;
        supplier.UpdatedAt = DateTime.UtcNow;
        _context.Suppliers.Add(supplier);
        await _context.SaveChangesAsync();
        return supplier;
    }

    public async Task UpdateAsync(Supplier supplier)
    {
        var existing = await _context.Suppliers.FirstOrDefaultAsync(s => s.SupplierId == supplier.SupplierId)
            ?? throw new KeyNotFoundException("Supplier not found");

        existing.SupplierName = supplier.SupplierName;
        existing.ContactNumber = supplier.ContactNumber;
        existing.Email = supplier.Email;
        existing.Address = supplier.Address;
        existing.UpdatedAt = DateTime.UtcNow;

        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(int id)
    {
        var existing = await _context.Suppliers.FirstOrDefaultAsync(s => s.SupplierId == id)
            ?? throw new KeyNotFoundException("Supplier not found");

        _context.Suppliers.Remove(existing);
        await _context.SaveChangesAsync();
    }

    public async Task<bool> HasProductsAsync(int supplierId)
    {
        return await _context.Products.AnyAsync(p => p.SupplierId == supplierId);
    }
}