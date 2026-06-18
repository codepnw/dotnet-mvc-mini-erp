using System.ComponentModel.DataAnnotations;
using MiniERP.Mvc.Common;
using MiniERP.Mvc.Common.Queries;
using MiniERP.Mvc.DTOs.Responses;

namespace MiniERP.Mvc.ViewModels;

public class ProductIndexVm
{
    public ProductQuery Query { get; set; } = new();
    public PagedResult<ProductDto> Products { get; set; } = new();
    public string? ErrorMessage { get; set; }
}

public class ProductDetailsVm
{
    public ProductDto Product { get; set; } = new();
    public string? ErrorMessage { get; set; }
}

public class ProductFormVm
{
    [Required(ErrorMessage = "Name is required"), StringLength(100)]
    public string Name { get; set; } = "";

    [Required(ErrorMessage = "Sku is required"), StringLength(100)]
    public string Sku { get; set; } = "";

    [Required(ErrorMessage = "Price is required"), Range(1, int.MaxValue)]
    public decimal Price { get; set; }

    [Required(ErrorMessage = "Stock is required"), Range(1, int.MaxValue)]
    public int Stock { get; set; }

    public int MinimumStock { get; set; }
    public int CategoryId { get; set; }

    public string? ErrorMessage { get; set; }
}

public class ProductLowStockVm
{
    public List<ProductDto> Products { get; set; } = [];
    public string? ErrorMessage { get; set; }
}

public class ProductStockVm
{
    public int ProductId { get; set; }

    [Range(1, int.MaxValue)]
    public int Quantity { get; set; }

    public string? ErrorMessage { get; set; }
}

public class ProductAdjustStockVm
{
    public int ProductId { get; set; }

    [Range(0, int.MaxValue)]
    public int NewStock { get; set; }

    [Required(ErrorMessage = "Remark is required")]
    public string Remark { get; set; } = "";
}

public class ProductStockManagementVm
{
    public int ProductId { get; set; }

    public string ProductName { get; set; } = "";
    public string Sku { get; set; } = "";

    public int CurrentStock { get; set; }
    public int MinimumStock { get; set; }

    public string? ErrorMessage { get; set; }
}