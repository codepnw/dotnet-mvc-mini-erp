using Microsoft.AspNetCore.Mvc;
using MiniERP.Mvc.Mappings;
using MiniERP.Mvc.Services;
using MiniERP.Mvc.ViewModels;
using MiniERP.Mvc.DTOs.Requests;
using Microsoft.AspNetCore.Mvc.Diagnostics;
using Microsoft.AspNetCore.Authorization;

namespace MiniERP.Mvc.Controllers;

public class ProductStocksController(IProductService service) : Controller
{
    private readonly IProductService _service = service;

    [Authorize]
    [HttpGet]
    public async Task<IActionResult> Manage(int id)
    {
        var result = await _service.GetProduct(id);

        var vm = new ProductStockManagementVm();

        if (result.IsFailure)
        {
            vm.ErrorMessage = result.ErrorMessage;
            return View(vm);
        }

        return View(result.Data!.ToStockViewModel());
    }

    [Authorize]
    [HttpGet]
    public async Task<IActionResult> LowStock()
    {
        var result = await _service.GetProductsLowStock();

        var vm = new ProductLowStockVm();

        if (result.IsFailure)
        {
            vm.ErrorMessage = result.ErrorMessage;
            return View(vm);
        }

        vm.Products = result.Data!;
        return View(vm);
    }

    [Authorize]
    [HttpGet]
    public async Task<IActionResult> Increase(int id)
    {
        return View(new ProductStockVm
        {
            ProductId = id
        });
    }

    [Authorize]
    [HttpPost]
    public async Task<IActionResult> Increase(int id, ProductStockVm vm)
    {
        if (!ModelState.IsValid) return View(vm);

        var result = await _service.IncreaseStock(id, vm.ToRequest());

        if (result.IsFailure)
        {
            ModelState.AddModelError("", result.ErrorMessage!);
            return View(vm);
        }

        return RedirectToProducts();
    }

    [Authorize]
    [HttpGet]
    public async Task<IActionResult> Decrease(int id)
    {
        return View(new ProductStockVm
        {
            ProductId = id
        });
    }

    [Authorize]
    [HttpPost]
    public async Task<IActionResult> Decrease(int id, ProductStockVm vm)
    {
        if (!ModelState.IsValid) return View(vm);

        var result = await _service.DecreaseStock(id, vm.ToRequest());

        if (result.IsFailure)
        {
            ModelState.AddModelError("", result.ErrorMessage!);
            return View(vm);
        }

        return RedirectToProducts();
    }

    [Authorize]
    [HttpGet]
    public async Task<IActionResult> NewStock(int id)
    {
        return View(new ProductAdjustStockVm
        {
            ProductId = id
        });
    }

    [Authorize]
    [HttpPost]
    public async Task<IActionResult> NewStock(int id, ProductAdjustStockVm vm)
    {
        if (!ModelState.IsValid) return View(vm);

        var result = await _service.AdjustStock(id, new ProductStockAdjustRequest
        {
            NewStock = vm.NewStock,
            Remark = vm.Remark
        });

        if (result.IsFailure)
        {
            ModelState.AddModelError("", result.ErrorMessage!);
            return View(vm);
        }

        return RedirectToProducts();
    }

    private RedirectToActionResult RedirectToProducts()
    {
        return RedirectToAction("Index", "Products");
    }
}