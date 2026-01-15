namespace InventoryManagement.Web.Dtos;

public record SupplierDto(
    int SupplierId,
    string SupplierName,
    string? ContactNumber,
    string? Email,
    string? Address,
    int ProductCount
);
