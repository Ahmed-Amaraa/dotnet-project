using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using InventoryManagement.Web.Dtos;
using InventoryManagement.Web.Models;
using InventoryManagement.Web.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace InventoryManagement.Web.Controllers.Api;

[Authorize(Roles = "Admin")]
[ApiController]
[Route("api/[controller]")]
public class SuppliersController : ControllerBase
{
    private readonly ISupplierService _supplierService;

    public SuppliersController(ISupplierService supplierService)
    {
        _supplierService = supplierService;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<SupplierDto>>> GetSuppliers([FromQuery] string? search)
    {
        var suppliers = await _supplierService.GetAllAsync(search);
        return Ok(suppliers.Select(MapToDto));
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<SupplierDto>> GetSupplier(int id)
    {
        var supplier = await _supplierService.GetByIdAsync(id);
        if (supplier is null)
        {
            return NotFound();
        }

        return Ok(MapToDto(supplier));
    }

    [HttpPost]
    public async Task<ActionResult<SupplierDto>> CreateSupplier([FromBody] SupplierCreateUpdateDto dto)
    {
        if (!ModelState.IsValid)
        {
            return ValidationProblem(ModelState);
        }

        var supplier = new Supplier
        {
            SupplierName = dto.SupplierName,
            ContactNumber = dto.ContactNumber,
            Email = dto.Email,
            Address = dto.Address
        };

        var created = await _supplierService.CreateAsync(supplier);
        return CreatedAtAction(nameof(GetSupplier), new { id = created.SupplierId }, MapToDto(created));
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> UpdateSupplier(int id, [FromBody] SupplierCreateUpdateDto dto)
    {
        if (!ModelState.IsValid)
        {
            return ValidationProblem(ModelState);
        }

        var existing = await _supplierService.GetByIdAsync(id);
        if (existing is null)
        {
            return NotFound();
        }

        existing.SupplierName = dto.SupplierName;
        existing.ContactNumber = dto.ContactNumber;
        existing.Email = dto.Email;
        existing.Address = dto.Address;

        await _supplierService.UpdateAsync(existing);
        var updated = await _supplierService.GetByIdAsync(id) ?? existing;
        return Ok(MapToDto(updated));
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> DeleteSupplier(int id)
    {
        var hasProducts = await _supplierService.HasProductsAsync(id);
        if (hasProducts)
        {
            return Conflict(new { message = "Impossible de supprimer un fournisseur associé à des produits." });
        }

        var existing = await _supplierService.GetByIdAsync(id);
        if (existing is null)
        {
            return NotFound();
        }

        await _supplierService.DeleteAsync(id);
        return NoContent();
    }

    private static SupplierDto MapToDto(Supplier supplier)
    {
        return new SupplierDto(
            supplier.SupplierId,
            supplier.SupplierName,
            supplier.ContactNumber,
            supplier.Email,
            supplier.Address,
            supplier.Products?.Count ?? 0
        );
    }
}
