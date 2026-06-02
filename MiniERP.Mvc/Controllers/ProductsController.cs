using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using MiniERP.Mvc.Common.Queries;
using MiniERP.Mvc.DTOs.Requests;
using MiniERP.Mvc.Mappings;
using MiniERP.Mvc.Services;
using MiniERP.Mvc.ViewModels;

namespace MiniERP.Mvc.Controllers;

public class ProductsController(IProductService service) : Controller
{
    private readonly IProductService _service = service;

    [Authorize]
    [HttpGet]
    public async Task<IActionResult> Index(ProductQuery query)
    {
        var result = await _service.ListProducts(query);

        var vm = new ProductIndexVm()
        {
            Query = query
        };

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
    public async Task<IActionResult> Create()
    {
        return View(new ProductFormVm());
    }

    [Authorize]
    [HttpPost]
    public async Task<IActionResult> Create(ProductFormVm vm)
    {
        if (!ModelState.IsValid) return View(vm);

        var result = await _service.CreateProduct(vm.ToCreateRequest());

        if (result.IsFailure)
        {
            vm.ErrorMessage = result.ErrorMessage;
            return View(vm);
        }

        return RedirectToAction("Index");
    }

    [Authorize]
    [HttpGet]
    public async Task<IActionResult> Details(int id)
    {
        var result = await _service.GetProduct(id);

        var vm = new ProductDetailsVm();

        if (result.IsFailure)
        {
            vm.ErrorMessage = result.ErrorMessage;
            return View(vm);
        }

        vm.Product = result.Data!;
        return View(vm);
    }

    [Authorize]
    [HttpGet]
    public async Task<IActionResult> Edit(int id)
    {
        var result = await _service.GetProduct(id);

        if (result.IsFailure)
        {
            return View(new ProductFormVm
            {
                ErrorMessage = result.ErrorMessage
            });
        }

        return View(result.Data!.ToViewModel());
    }

    [Authorize]
    [HttpPost]
    public async Task<IActionResult> Edit(int id, ProductFormVm vm)
    {
        if (!ModelState.IsValid) return View(vm);

        var result = await _service.UpdateProduct(id, vm.ToUpdateRequest());

        if (result.IsFailure)
        {
            ModelState.AddModelError("", result.ErrorMessage!);
            return View(vm);
        }

        return RedirectToAction("Index");
    }

    [Authorize]
    [HttpPost]
    public async Task<IActionResult> Delete(int id)
    {
        var result = await _service.DeleteProduct(id);

        return result.IsFailure
            ? View("Error", new ErrorViewModel { ErrorMessage = result.ErrorMessage })
            : RedirectToAction("Index");
    }
}