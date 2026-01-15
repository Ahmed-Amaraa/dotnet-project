namespace InventoryManagement.Web.ViewModels;

public record DashboardSummaryViewModel(
    int TotalProducts,
    int TotalSuppliers,
    int LowStockCount,
    decimal InventoryValue,
    IReadOnlyCollection<CategoryBreakdown> TopCategories
);

public record CategoryBreakdown(string Category, int ProductCount, decimal InventoryValue);