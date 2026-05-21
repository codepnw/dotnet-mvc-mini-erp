using Microsoft.AspNetCore.Mvc;
using MiniERP.Mvc.Services;

namespace MiniERP.Mvc.Controllers;

public class ReportController(IReportService service) : Controller
{
    private readonly IReportService _service = service;

    // GET
    // public IActionResult Index()
    // {
    //     return View();
    // }

    public async Task<IActionResult> Revenue()
    {
        var result = await _service.GetRevenueSummary();

        // TODO: View
        return result.IsFailure
            ? Json(new { Message = result.ErrorMessage })
            : Json(result);
    }

    public async Task<IActionResult> Product()
    {
        var result = await _service.GetProductTopSell();

        // TODO: View
        return result.IsFailure
            ? Json(new { Message = result.ErrorMessage })
            : Json(result);
    }
}