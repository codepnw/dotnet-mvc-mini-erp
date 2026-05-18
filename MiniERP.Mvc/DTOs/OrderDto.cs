namespace MiniERP.Mvc.DTOs;

public class OrderCreateDto
{
    public List<OrderItemCreateDto> Items { get; set; } = [];
}

public class OrderItemCreateDto
{
    public int ProductId { get; set; }
    public int Quantity { get; set; }
    public decimal UnitPrice { get; set; }
}