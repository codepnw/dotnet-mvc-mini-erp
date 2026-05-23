using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;

namespace MiniERP.Mvc.DTOs.Requests;

public class ProductCreateRequest
{
    [Required(ErrorMessage = "Name is required")]
    [StringLength(100)]
    public required string Name { get; init; }

    [Required(ErrorMessage = "Sku is required")]
    [StringLength(100)]
    public required string Sku { get; init; }

    [Required(ErrorMessage = "Price is required")]
    [Range(1, 1000000, ErrorMessage = "Price must be between 1 and 1,000,000")]
    public required decimal Price { get; init; }

    [Required(ErrorMessage = "Stock is required")]
    [Range(1, 10000, ErrorMessage = "Stock must be between 1 and 10,000")]
    public required int Stock { get; init; }

    [Required(ErrorMessage = "Minimum Stock is required")]
    [Range(1, 100, ErrorMessage = "Minimum Stock must be between 1 and 100")]
    public required int MinimumStock { get; init; }

    [Required(ErrorMessage = "Category id is required")]
    public required int CategoryId { get; init; }
}

public class ProductUpdateRequest
{
    [StringLength(100)] public string? Name { get; init; }

    [StringLength(100)] public string? Sku { get; init; }

    [Range(1, 1000000, ErrorMessage = "Price must be between 1 and 1,000,000")]
    public decimal? Price { get; init; }

    [Range(1, 10000, ErrorMessage = "Stock must be between 1 and 10,000")]
    public int? Stock { get; init; }

    [Range(1, 100, ErrorMessage = "Minimum Stock must be between 1 and 100")]
    public int? MinimumStock { get; init; }

    public int? CategoryId { get; init; }
}

public class ProductStockAdjustRequest
{
    [Required(ErrorMessage = "New Stock is required")]
    [Range(1, 10000, ErrorMessage = "Stock must be between 1 and 10,000")]
    public required int NewStock { get; init; }
    
    [Required(ErrorMessage = "Remark is required")]
    [StringLength(100)]
    public required string Remark { get; init; }
}

public class ProductStockQuantityRequest
{
    [Required(ErrorMessage = "Quantity is required")]
    [Range(1, 10000, ErrorMessage = "Quantity must be between 1 and 10,000")]
    public required int Quantity { get; init; }
}