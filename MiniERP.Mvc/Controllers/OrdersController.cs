using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using MiniERP.Mvc.Common;
using MiniERP.Mvc.Common.Queries;
using MiniERP.Mvc.Mappings;
using MiniERP.Mvc.Services;
using MiniERP.Mvc.ViewModels;

namespace MiniERP.Mvc.Controllers;


public class OrdersController(IOrderService service, IProductService productService) : Controller
{
    private readonly IOrderService _service = service;
    private readonly IProductService _productService = productService;

    [HttpGet]
    public async Task<IActionResult> Index(OrderQuery query)
    {
        var result = await _service.ListOrders(query);

        var vm = new OrderIndexVm()
        {
            Query = query
        };

        if (result.IsFailure)
        {
            vm.ErrorMessage = result.ErrorMessage;
            return View(vm);
        }

        vm.Orders = result.Data!;
        return View(vm);
    }

    [HttpGet]
    public async Task<IActionResult> Create()
    {
        var productResult = await _productService.ListProducts(new ProductQuery());

        var vm = new OrderFormVm();

        if (productResult.IsFailure)
        {
            ModelState.AddModelError("", productResult.ErrorMessage!);
            return View(vm);
        }

        vm.Products = [.. productResult.Data!.Items.Select(x => x.ToSelectVm())];
        return View(vm);
    }

    [HttpPost]
    public async Task<IActionResult> Create(OrderFormVm vm)
    {
        if (!ModelState.IsValid) return View(vm);

        var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

        var result = await _service.CreateOrder(userId, vm.ToRequest());

        if (result.IsFailure)
        {
            // TODO: Get Products if fail

            ModelState.AddModelError("", result.ErrorMessage!);
            return View(vm);
        }

        return RedirectToAction("Details", new { id = result.Data });
    }

    [HttpGet]
    public async Task<IActionResult> Details(int id)
    {
        var result = await _service.GetOrderDetails(id);
        var vm = new OrderDetailsVm();

        if (result.IsFailure)
        {
            vm.ErrorMessage = result.ErrorMessage;
            return View(vm);
        }

        vm.Order = result.Data!;
        return View(vm);
    }

    // private async Task<List<ProductSelectVm>> GetSelectProducts()
    // {
    //     var result = await _productService.ListProducts(new ProductQuery());

    //     return result.Data!.Items.Select(x => x.ToSelectVm());
    // }
}