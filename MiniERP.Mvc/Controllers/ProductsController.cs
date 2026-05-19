using Microsoft.AspNetCore.Mvc;
using MiniERP.Mvc.Common.Queries;
using MiniERP.Mvc.DTOs;
using MiniERP.Mvc.Services;

namespace MiniERP.Mvc.Controllers;

public class ProductsController(IProductService service) : Controller
{
    private readonly IProductService _service = service;

    public async Task<IActionResult> Index(ProductQuery req)
    {
        var result = await _service.ListProducts(req);

        // TODO: View
        return Json(result);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] ProductCreateDto dto)
    {
        var result = await _service.CreateProduct(dto);

        // TODO: View
        return Json(result);
    }

    public async Task<IActionResult> Details(int id)
    {
        var result = await _service.GetProduct(id);

        // TODO: View
        return Json(result);
    }

    public async Task<IActionResult> LowStock()
    {
        var result = await _service.GetProductsLowStock();

        // TODO: View
        return Json(result);
    }

    [HttpPost]
    public async Task<IActionResult> Edit(int id, [FromBody] ProductUpdateDto dto)
    {
        var result = await _service.UpdateProduct(id, dto);

        // TODO: View
        return result.IsFailure
            ? Json(new { error = result.ErrorMessage })
            : Json(new { message = "product updated" });
    }

    public async Task<IActionResult> Delete(int id)
    {
        var result = await _service.DeleteProduct(id);

        // TODO: View
        return result.IsFailure
            ? Json(new { error = result.ErrorMessage })
            : Json(new { message = "product deleted" });
    }

    [HttpPost]
    public async Task<IActionResult> Increase(int id, [FromBody] ProductStockQuantityDto dto)
    {
        var result = await _service.IncreaseStock(id, dto);

        // TODO: View
        return result.IsFailure
            ? Json(new { error = result.ErrorMessage })
            : Json(new { message = "product increased" });
    }

    [HttpPost]
    public async Task<IActionResult> Decrease(int id, [FromBody] ProductStockQuantityDto dto)
    {
        var result = await _service.DecreaseStock(id, dto);

        // TODO: View
        return result.IsFailure
            ? Json(new { error = result.ErrorMessage })
            : Json(new { message = "product decreased" });
    }

    [HttpPost]
    public async Task<IActionResult> Adjust(int id, [FromBody] ProductStockAdjustDto dto)
    {
        var result = await _service.AdjustStock(id, dto);

        // TODO: View
        return result.IsFailure
            ? Json(new { error = result.ErrorMessage })
            : Json(new { message = "product updated" });
    }
}