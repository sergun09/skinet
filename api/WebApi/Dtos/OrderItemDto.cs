namespace WebApi.Dtos;

public class OrderItemDto
{
    public int ProductId { get; set; }
    public required string ProductName { get; set; }
    public required string PictureUrl { get; set; }
    public required decimal Price { get; set; }
    public required int Quantity { get; set; }
}