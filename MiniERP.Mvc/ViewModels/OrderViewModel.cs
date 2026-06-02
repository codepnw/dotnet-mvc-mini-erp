using MiniERP.Mvc.Entities;

namespace MiniERP.Mvc.ViewModels;

// public class OrderViewModel
// {
//     public int Id { get; set; }
//     public string OrderNumber { get; set; } = string.Empty;
//     public decimal TotalAmount { get; set; }
//     public OrderStatus Status { get; set; }
//     public DateTime CreatedAt { get; set; }
// }

public class OrderDetailsViewModel
{
    public int Id { get; set; }
    public string OrderNumber { get; set; } = string.Empty;
    public decimal TotalAmount { get; set; }
    public OrderStatus Status { get; set; }
    public DateTime CreatedAt { get; set; }
    public List<OrderItemViewModel> Items { get; set; } = [];
}

public class OrderItemViewModel
{
    public string ProductName { get; set; } = string.Empty;
    public int Quantity { get; set; }
    public decimal UnitPrice { get; set; }
    public decimal TotalPrice { get; set; }
}