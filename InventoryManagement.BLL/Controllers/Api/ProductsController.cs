using System;
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
public class ProductsController : ControllerBase
{
    private readonly IProductService _productService;

    public ProductsController(IProductService productService)
    {
        _productService = productService;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<ProductDto>>> GetProducts([FromQuery] string? search, [FromQuery] string? category, [FromQuery] int? supplierId)
    {
        var products = await _productService.GetAllAsync(search, category, supplierId);
        return Ok(products.Select(MapToDto));
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<ProductDto>> GetProduct(int id)
    {
        var product = await _productService.GetByIdAsync(id);
        if (product is null)
        {
            return NotFound();
        }

        return Ok(MapToDto(product));
    }

    [HttpPost]
    public async Task<ActionResult<ProductDto>> CreateProduct([FromBody] ProductCreateUpdateDto dto)
    {
        if (!ModelState.IsValid)
        {
            return ValidationProblem(ModelState);
        }

        var product = await _productService.CreateAsync(new Product
        {
            ProductName = dto.ProductName,
            Category = dto.Category,
            Quantity = dto.Quantity,
            UnitPrice = dto.UnitPrice,
            ReorderLevel = dto.ReorderLevel,
            SupplierId = dto.SupplierId
        });

        var created = await _productService.GetByIdAsync(product.ProductId)
                      ?? product;

        return CreatedAtAction(nameof(GetProduct), new { id = created.ProductId }, MapToDto(created));
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> UpdateProduct(int id, [FromBody] ProductCreateUpdateDto dto)
    {
        if (!ModelState.IsValid)
        {
            return ValidationProblem(ModelState);
        }

        var existing = await _productService.GetByIdAsync(id);
        if (existing is null)
        {
            return NotFound();
        }

        existing.ProductName = dto.ProductName;
        existing.Category = dto.Category;
        existing.Quantity = dto.Quantity;
        existing.UnitPrice = dto.UnitPrice;
        existing.ReorderLevel = dto.ReorderLevel;
        existing.SupplierId = dto.SupplierId;

        await _productService.UpdateAsync(existing);
        var updated = await _productService.GetByIdAsync(id) ?? existing;
        return Ok(MapToDto(updated));
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> DeleteProduct(int id)
    {
        var existing = await _productService.GetByIdAsync(id);
        if (existing is null)
        {
            return NotFound();
        }

        await _productService.DeleteAsync(id);
        return NoContent();
    }

    [HttpPost("{id:int}/updateStock")]
    public async Task<IActionResult> UpdateStock(int id, [FromBody] StockUpdateDto dto)
    {
        if (!ModelState.IsValid)
        {
            return ValidationProblem(ModelState);
        }

        try
        {
            await _productService.UpdateStockAsync(id, dto.Amount);
        }
        catch (KeyNotFoundException)
        {
            return NotFound();
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new { message = ex.Message });
        }

        var updated = await _productService.GetByIdAsync(id);
        return updated is null ? NotFound() : Ok(MapToDto(updated));
    }

    [HttpGet("low-stock")]
    public async Task<ActionResult<IEnumerable<ProductDto>>> GetLowStock()
    {
        var products = await _productService.GetLowStockAsync();
        return Ok(products.Select(MapToDto));
    }

    private static ProductDto MapToDto(Product product)
    {
        return new ProductDto(
            product.ProductId,
            product.ProductName,
            product.Category,
            product.Quantity,
            product.UnitPrice,
            product.ReorderLevel,
            product.SupplierId,
            product.Supplier?.SupplierName ?? string.Empty,
            product.IsLowStock,
            product.InventoryValue
        );
    }
}
