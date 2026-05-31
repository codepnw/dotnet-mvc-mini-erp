using MiniERP.Mvc.DTOs.Requests;
using MiniERP.Mvc.DTOs.Responses;
using MiniERP.Mvc.Entities;
using MiniERP.Mvc.ViewModels;

namespace MiniERP.Mvc.Mappings;

public static class ProductMapping
{
    public static Product ToEntity(this ProductCreateRequest request) => new()
    {
        Name = request.Name,
        Sku = request.Sku,
        Price = request.Price,
        Stock = request.Stock,
        MinimumStock = request.MinimumStock,
        CategoryId = request.CategoryId,
    };

    public static ProductDto ToProductDto(this Product product) => new()
    {
        Id = product.Id,
        Name = product.Name,
        Sku = product.Sku,
        Price = product.Price,
        Stock = product.Stock,
        MinimumStock = product.MinimumStock,
        CategoryTitle = product.Category?.Title ?? "N/A",
        CreatedAt = product.CreatedAt,
        UpdatedAt = product.UpdatedAt,
    };

    public static void ApplyUpdate(this ProductUpdateRequest request, Product product)
    {
        product.Name = request.Name ?? product.Name;
        product.Sku = request.Sku ?? product.Sku;
        product.Price = request.Price ?? product.Price;
        product.Stock = request.Stock ?? product.Stock;
        product.MinimumStock = request.MinimumStock ?? product.MinimumStock;
        product.CategoryId = request.CategoryId ?? product.CategoryId;

        product.UpdatedAt = DateTime.UtcNow;
    }

    public static ProductCreateRequest ToCreateRequest(this ProductFormVm vm) => new()
    {
        Name = vm.Name,
        Sku = vm.Sku,
        Price = vm.Price,
        Stock = vm.Stock,
        MinimumStock = vm.MinimumStock,
        CategoryId = vm.CategoryId
    };

    public static ProductUpdateRequest ToUpdateRequest(this ProductFormVm vm) => new()
    {
        Name = vm.Name,
        Sku = vm.Sku,
        Price = vm.Price,
        Stock = vm.Stock,
        MinimumStock = vm.MinimumStock,
        CategoryId = vm.CategoryId
    };

    public static ProductFormVm ToViewModel(this ProductDto dto) => new()
    {
        Name = dto.Name,
        Sku = dto.Sku,
        Price = dto.Price,
        Stock = dto.Stock,
        MinimumStock = dto.MinimumStock
    };

    public static ProductStockQuantityRequest ToRequest(this ProductStockVm vm) => new()
    {
        Quantity = vm.Quantity
    };
}