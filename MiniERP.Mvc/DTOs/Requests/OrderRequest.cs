namespace MiniERP.Mvc.DTOs.Requests;

public class OrderCreateRequest
{
    public List<OrderItemCreateRequest> Items { get; set; } = [];
}

public class OrderItemCreateRequest
{
    public int ProductId { get; set; }
    public int Quantity { get; set; }
    // public decimal UnitPrice { get; set; }
}