using System.ComponentModel.DataAnnotations;

namespace InventoryManagement.Web.Dtos;

public class StockUpdateDto
{
    [Required]
    public int Amount { get; set; }
}
