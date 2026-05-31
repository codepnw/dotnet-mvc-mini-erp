using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;

namespace MiniERP.Mvc.DTOs.Requests;

public class ProductCreateRequest
{
    public string Name { get; init; } = "";
    public string Sku { get; init; } = "";
    public decimal Price { get; init; }
    public int Stock { get; init; }
    public int MinimumStock { get; init; }
    public int CategoryId { get; init; }
}

public class ProductUpdateRequest
{
    public string? Name { get; init; }
    public string? Sku { get; init; }
    public decimal? Price { get; init; }
    public int? Stock { get; init; }
    public int? MinimumStock { get; init; }
    public int? CategoryId { get; init; }
}

public class ProductStockAdjustRequest
{
    public required int NewStock { get; init; }
    public required string Remark { get; init; }
}

public class ProductStockQuantityRequest
{
    public required int Quantity { get; init; }
}