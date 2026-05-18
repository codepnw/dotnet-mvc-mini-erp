namespace MiniERP.Mvc.Entities;

public enum OrderStatus
{
    Pending,
    Completed,
    Cancelled,
}

public class Order
{
    public int Id { get; set; }
    public string OrderNumber { get; set; } = string.Empty;
    public decimal TotalAmount { get; set; }
    public OrderStatus Status { get; set; }
    public DateTime CreatedAt { get; set; }
    
    // FK
    public required int UserId { get; set; }
    public User? User { get; set; }

    public List<OrderItem> Items { get; set; } = [];
}