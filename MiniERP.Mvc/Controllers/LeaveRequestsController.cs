using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MiniERP.Mvc.DTOs;
using MiniERP.Mvc.Entities;
using MiniERP.Mvc.Services;

namespace MiniERP.Mvc.Controllers;

public class LeaveRequestsController(ILeaveRequestService service) : Controller
{
    private readonly ILeaveRequestService _service = service;

    [HttpGet]
    public async Task<IActionResult> Index()
    {
        var result = await _service.GetAllLeaveRequests();

        return result.IsFailure
            ? Json(new { message = result.ErrorMessage })
            : Json(result.Data);
    }
    
    [HttpGet]
    public async Task<IActionResult> Details(int id)
    {
        var result = await _service.GetLeaveRequest(id);

        return result.IsFailure
            ? Json(new { message = result.ErrorMessage })
            : Json(result.Data);
    }
    
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] LeaveRequestCreateDTO dto)
    {
        if (!ModelState.IsValid) return Json(ModelState);
        
        var result = await _service.CreateLeaveRequest(dto);

        return result.IsFailure
            ? Json(new { message = result.ErrorMessage })
            : Json(new { message = "Created successfully" });
    }
    
    [HttpPatch]
    public async Task<IActionResult> Edit(int id, [FromBody] LeaveRequestUpdateDTO dto)
    {
        if (!ModelState.IsValid) return Json(ModelState);
        
        var result = await _service.UpdateLeaveRequest(id, dto);

        return result.IsFailure
            ? Json(new { message = result.ErrorMessage })
            : Json(new { message = "Updated successfully" });
    }
    
    [HttpDelete]
    public async Task<IActionResult> Delete(int id)
    {
        var result = await _service.DeleteLeaveRequest(id);

        return result.IsFailure
            ? Json(new { message = result.ErrorMessage })
            : Json(new { message = "Deleted successfully" });
    }
}