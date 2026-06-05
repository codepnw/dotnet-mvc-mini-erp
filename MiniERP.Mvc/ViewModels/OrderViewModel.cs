using System.ComponentModel.DataAnnotations;
using MiniERP.Mvc.Common;
using MiniERP.Mvc.Common.Queries;
using MiniERP.Mvc.DTOs.Responses;

namespace MiniERP.Mvc.ViewModels;

public class OrderIndexVm
{
    public OrderQuery Query { get; set; } = new();
    public PagedResult<OrderDto> Orders { get; set; } = new();
    public string? ErrorMessage { get; set; }
}

public class OrderDetailsVm
{
    public OrderDto Order { get; set; } = new();
    public string? ErrorMessage { get; set; }
}

public class OrderFormVm
{
    public List<ProductSelectVm> Products { get; set; } = [];
    public List<OrderItemVm> Items { get; set; } = [new()];
}

public class ProductSelectVm
{
    public int Id { get; set; }
    public string Name { get; set; } = "";
    public int Stock { get; set; }
    public decimal Price { get; set; }
}

public class OrderItemVm
{
    [Required(ErrorMessage = "Product id is required")]
    public int ProductId { get; set; }

    [Required(ErrorMessage = "Quantity id is required")]
    public int Quantity { get; set; }
}