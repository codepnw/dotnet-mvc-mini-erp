using Microsoft.AspNetCore.Mvc;
using MiniERP.Mvc.Services;
using MiniERP.Mvc.ViewModels;

namespace MiniERP.Mvc.Controllers;

public class DashboardController(IDashboardService service) : Controller
{
    private readonly IDashboardService _service = service;

    [HttpGet]
    public async Task<IActionResult> Index()
    {
        var result = await _service.GetStats();

        if (result.IsFailure)
        {
            return View(new DashboardStatsVm());
        }

        return View(result.Data);
    }
}