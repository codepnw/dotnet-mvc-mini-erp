using Microsoft.AspNetCore.Mvc;
using MiniERP.Mvc.Services;

namespace MiniERP.Mvc.Controllers;

public class DashboardController(IDashboardService service) : Controller
{
    private readonly IDashboardService _service = service;

    // // GET
    // public IActionResult Index()
    // {
    //     return View();
    // }

    public async Task<IActionResult> Stats()
    {
        var result = await _service.GetStats();
        
        // TODO: View
        return Json(result);
    }
}