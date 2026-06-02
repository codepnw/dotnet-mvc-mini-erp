using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MiniERP.Mvc.Common.Queries;
using MiniERP.Mvc.DTOs.Requests;
using MiniERP.Mvc.Entities;
using MiniERP.Mvc.Mappings;
using MiniERP.Mvc.Services;
using MiniERP.Mvc.ViewModels;

namespace MiniERP.Mvc.Controllers;

public class LeaveRequestsController(ILeaveRequestService service) : Controller
{
    private readonly ILeaveRequestService _service = service;

    [Authorize]
    [HttpGet]
    public async Task<IActionResult> Index(LeaveRequestQuery query)
    {
        var result = await _service.ListLeaveRequests(query);

        var vm = new LeaveRequestIndexVm
        {
            Query = query
        };

        if (result.IsFailure)
        {
            vm.ErrorMessage = result.ErrorMessage;
            return View(vm);
        }

        vm.LeaveRequests = result.Data!;
        return View(vm);
    }

    [Authorize]
    [HttpGet]
    public async Task<IActionResult> Details(int id)
    {
        var result = await _service.GetLeaveRequest(id);

        var vm = new LeaveRequestDetailsVm();

        if (result.IsFailure)
        {
            vm.ErrorMessage = result.ErrorMessage;
            return View(vm);
        }

        vm.LeaveRequest = result.Data!;
        return View(vm);
    }

    [Authorize]
    [HttpGet]
    public async Task<IActionResult> Create()
    {
        var vm = new LeaveRequestFormVm
        {
            // Load Select Options
            EmployeesOptions = await _service.ListEmployees(),
            LeaveTypeOptions = await _service.ListLeaveTypes(),
        };
        return View(vm);
    }

    [Authorize]
    [HttpPost]
    public async Task<IActionResult> Create(LeaveRequestFormVm vm)
    {
        if (!ModelState.IsValid)
        {
            // Load Select Options
            vm.EmployeesOptions = await _service.ListEmployees();
            vm.LeaveTypeOptions = await _service.ListLeaveTypes();
            return View(vm);
        }

        var result = await _service.CreateLeaveRequest(vm.ToCreateRequest());

        if (result.IsFailure)
        {
            ModelState.AddModelError("", result.ErrorMessage!);
            return View(vm);
        }

        return RedirectToAction("index");
    }

    [Authorize]
    [HttpGet]
    public async Task<IActionResult> Edit(int id)
    {
        var result = await _service.GetLeaveRequest(id);

        if (result.IsFailure)
        {
            return View(new LeaveRequestFormVm
            {
                ErrorMessage = result.ErrorMessage,
            });
        }

        return View(result.Data!.ToViewModel());
    }

    [Authorize]
    [HttpPost]
    public async Task<IActionResult> Edit(int id, LeaveRequestFormVm vm)
    {
        if (!ModelState.IsValid) return View(vm);

        var result = await _service.UpdateLeaveRequest(id, vm.ToEditRequest());

        if (result.IsFailure)
        {
            ModelState.AddModelError("", result.ErrorMessage!);
            return View(vm);
        }

        return RedirectToAction("index");
    }

    [Authorize]
    [HttpPost]
    public async Task<IActionResult> Approve(int id)
    {
        var result = await _service.UpdateLeaveRequestStatus(id, LeaveStatus.Approved);

        return result.IsFailure
            ? View("Error", new ErrorViewModel { ErrorMessage = result.ErrorMessage })
            : RedirectToAction("index");
    }

    [Authorize]
    [HttpPost]
    public async Task<IActionResult> Reject(int id)
    {
        var result = await _service.UpdateLeaveRequestStatus(id, LeaveStatus.Rejected);

        return result.IsFailure
            ? View("Error", new ErrorViewModel { ErrorMessage = result.ErrorMessage })
            : RedirectToAction("index");
    }

    [Authorize]
    [HttpPost]
    public async Task<IActionResult> Delete(int id)
    {
        var result = await _service.DeleteLeaveRequest(id);

        return result.IsFailure
            ? View("Error", new ErrorViewModel { ErrorMessage = result.ErrorMessage })
            : RedirectToAction("index");
    }
}