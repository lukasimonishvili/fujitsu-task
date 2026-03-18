
namespace Application.DTOs;

public class BasketItemDto
{
    public int ProductId { get; set; }
    public string ProductName { get; set; } = string.Empty;
    public double Weight { get; set; }
    public int Quantity { get; set; }
}