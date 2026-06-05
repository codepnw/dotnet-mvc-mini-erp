using MiniERP.Mvc.DTOs.Requests;
using MiniERP.Mvc.DTOs.Responses;
using MiniERP.Mvc.ViewModels;

namespace MiniERP.Mvc.Mappings;

public static class OrderMappings
{
    public static OrderItemCreateRequest ToItemRequest(this OrderItemVm vm) => new()
    {
        ProductId = vm.ProductId,
        Quantity = vm.Quantity
    };

    public static OrderCreateRequest ToRequest(this OrderFormVm vm) => new()
    {
        Items = [.. vm.Items.Select(x => x.ToItemRequest())]
    };

    public static ProductSelectVm ToSelectVm(this ProductDto dto) => new()
    {
        Id = dto.Id,
        Name = dto.Name,
        Stock = dto.Stock,
        Price = dto.Price,
    };
}