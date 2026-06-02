using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MiniERP.Mvc.Common.Queries;
using MiniERP.Mvc.DTOs.Requests;
using MiniERP.Mvc.Mappings;
using MiniERP.Mvc.Services;
using MiniERP.Mvc.ViewModels;

namespace MiniERP.Mvc.Controllers;

public class CategoriesController(ICategoryService service) : Controller
{
    private readonly ICategoryService _service = service;

    [Authorize]
    [HttpGet]
    public async Task<IActionResult> Index(CategoryQuery query)
    {
        var result = await _service.ListCategories(query);

        var vm = new CategoryIndexVm()
        {
            Query = query
        };

        if (result.IsFailure)
        {
            vm.ErrorMessage = result.ErrorMessage!;
            return View(vm);
        }

        vm.Categories = result.Data!;
        return View(vm);
    }

    [Authorize]
    [HttpGet]
    public async Task<IActionResult> Details(int id)
    {
        var result = await _service.GetCategory(id);

        var vm = new CategoryDetailsVm();

        if (result.IsFailure)
        {
            vm.ErrorMessage = result.ErrorMessage;
            return View(vm);
        }

        vm.Category = result.Data!;
        return View(vm);
    }

    [Authorize]
    [HttpGet]
    public IActionResult Create()
    {
        return View(new CategoryFormVm());
    }

    [Authorize]
    [HttpPost]
    public async Task<IActionResult> Create(CategoryFormVm vm)
    {
        if (!ModelState.IsValid) return View(vm);

        var result = await _service.Create(vm.ToCreateRequest());

        if (result.IsFailure)
        {
            ModelState.AddModelError("", result.ErrorMessage!);
            return View(vm);
        }

        return RedirectToAction("Index");
    }

    [Authorize]
    [HttpGet]
    public async Task<IActionResult> Edit(int id)
    {
        var result = await _service.GetCategory(id);

        if (result.IsFailure)
        {
            return View(new CategoryFormVm
            {
                ErrorMessage = result.ErrorMessage
            });
        }

        return View(result.Data!.ToViewModel());
    }

    [Authorize]
    [HttpPost]
    public async Task<IActionResult> Edit(int id, CategoryFormVm vm)
    {
        if (!ModelState.IsValid) return View(vm);

        var result = await _service.UpdateCategory(id, vm.ToUpdateRequest());

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
        var result = await _service.DeleteCategory(id);

        return result.IsFailure
            ? View("Error", new ErrorViewModel { ErrorMessage = result.ErrorMessage })
            : RedirectToAction("Index");
    }
}