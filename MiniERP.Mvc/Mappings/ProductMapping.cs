using MiniERP.Mvc.DTOs;
using MiniERP.Mvc.Entities;
using MiniERP.Mvc.Models;

namespace MiniERP.Mvc.Mappings;

public static class ProductMapping
{
    public static Product ToEntity(this ProductCreateDto dto) => new()
    {
        Name = dto.Name,
        Sku = dto.Sku,
        Price = dto.Price,
        Stock = dto.Stock,
        CategoryId = dto.CategoryId,
    };

    public static ProductViewModel ToViewModel(this Product product) => new()
    {
        Id = product.Id,
        Name = product.Name,
        Sku = product.Sku,
        Price = product.Price,
        Stock = product.Stock,
        CategoryTitle = product.Category?.Title ?? "N/A",
        CreatedAt = product.CreatedAt,
        UpdatedAt = product.UpdatedAt,
    };

    public static void ApplyUpdate(this ProductUpdateDto dto, Product product)
    {
        product.Name = dto.Name ?? product.Name;
        product.Sku = dto.Sku ?? product.Sku;
        product.Price = dto.Price ?? product.Price;
        product.Stock = dto.Stock ?? product.Stock;
        product.CategoryId = dto.CategoryId ?? product.CategoryId;

        product.UpdatedAt = DateTime.UtcNow;
    }
}