using InventoryManagement.Web.ViewModels;

namespace InventoryManagement.Web.Services;

public interface IDashboardService
{
    Task<DashboardSummaryViewModel> GetSummaryAsync();
}