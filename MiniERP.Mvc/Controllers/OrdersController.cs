using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
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

    [Authorize]
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

    [Authorize]
    [HttpGet]
    public async Task<IActionResult> Create()
    {
        var products = await GetSelectProducts();

        var vm = new OrderFormVm();

        if (products.IsFailure)
        {
            ModelState.AddModelError("", products.ErrorMessage!);
            return View(vm);
        }

        vm.Products = products.Data!;
        return View(vm);
    }

    [Authorize]
    [HttpPost]
    public async Task<IActionResult> Create(OrderFormVm vm)
    {
        if (!ModelState.IsValid) return View(vm);

        var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

        var result = await _service.CreateOrder(userId, vm.ToRequest());

        if (result.IsFailure)
        {
            var products = await GetSelectProducts();
            if (products.IsFailure)
            {
                ModelState.AddModelError("", result.ErrorMessage!);
                return View(vm);
            }
            vm.Products = products.Data!;

            ModelState.AddModelError("", result.ErrorMessage!);
            return View(vm);
        }

        return RedirectToAction("Details", new { id = result.Data });
    }

    [Authorize]
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

    [Authorize]
    [HttpPost]
    public async Task<IActionResult> Cancel(int id)
    {
        var result = await _service.CancelOrder(id);

        return result.IsFailure
            ? View("Error", new ErrorViewModel { ErrorMessage = result.ErrorMessage })
            : RedirectToAction("Index");
    }

    private async Task<Result<List<ProductSelectVm>>> GetSelectProducts()
    {
        var result = await _productService.ListProducts(new ProductQuery());

        if (result.IsFailure)
        {
            return Result<List<ProductSelectVm>>.Failure(result.ErrorMessage!, ErrorCode.InternalServerError);
        }

        var products = result.Data!.Items.Select(x => x.ToSelectVm()).ToList();

        return Result<List<ProductSelectVm>>.Success(products);
    }
}