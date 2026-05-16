using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;

namespace MiniERP.Mvc.DTOs;

public class ProductCreateDto
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

    [Required(ErrorMessage = "Category id is required")]
    public required int CategoryId { get; init; }
}

public class ProductUpdateDto
{
    [StringLength(100)] public string? Name { get; init; }

    [StringLength(100)] public string? Sku { get; init; }

    [Range(1, 1000000, ErrorMessage = "Price must be between 1 and 1,000,000")]
    public decimal? Price { get; init; }

    [Range(1, 10000, ErrorMessage = "Stock must be between 1 and 10,000")]
    public int? Stock { get; init; }

    public int? CategoryId { get; init; }
}