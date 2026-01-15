namespace InventoryManagement.Web.Dtos;

public record ProductDto(
    int ProductId,
    string ProductName,
    string? Category,
    int Quantity,
    decimal UnitPrice,
    int ReorderLevel,
    int SupplierId,
    string SupplierName,
    bool IsLowStock,
    decimal InventoryValue
);
