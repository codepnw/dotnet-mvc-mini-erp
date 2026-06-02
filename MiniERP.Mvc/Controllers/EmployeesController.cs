using System.Runtime.CompilerServices;
using System.Security.Cryptography;
using Microsoft.AspNetCore.Mvc;
using MiniERP.Mvc.Common.Queries;
using MiniERP.Mvc.DTOs.Requests;
using MiniERP.Mvc.Mappings;
using MiniERP.Mvc.ViewModels;
using MiniERP.Mvc.Services;
using Microsoft.AspNetCore.Authorization;

namespace MiniERP.Mvc.Controllers
{
    public class EmployeesController(IEmployeeService service) : Controller
    {
        private readonly IEmployeeService _service = service;

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> Index(EmployeeQuery req)
        {
            var result = await _service.ListEmployees(req);

            var vm = new EmployeeIndexVm
            {
                Query = req,
            };

            if (result.IsFailure)
            {
                vm.ErrorMessage = result.ErrorMessage;
                return View(vm);
            }

            vm.Employees = result.Data!;
            return View(vm);
        }

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> Details(int id)
        {
            var result = await _service.GetEmployeeById(id);

            var vm = new EmployeeDetailsVm();

            if (result.IsFailure)
            {
                vm.ErrorMessage = result.ErrorMessage;
                return View(vm);
            }

            vm.Employee = result.Data!;
            return View(vm);
        }

        [Authorize]
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> Create(EmployeeCreateVm vm)
        {
            if (!ModelState.IsValid) return View(vm);

            var result = await _service.CreateEmployee(vm.ToCreateRequest());

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
            var result = await _service.GetEmployeeById(id);

            if (result.IsFailure)
            {
                return View(new EmployeeEditVm
                {
                    ErrorMessage = result.ErrorMessage,
                });
            }

            return View(result.Data!.ToEditViewModel());
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> Edit(int id, EmployeeEditVm vm)
        {
            if (!ModelState.IsValid) return View(vm);

            var result = await _service.UpdateEmployee(id, vm.ToEditRequest());

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
            var result = await _service.DeleteEmployee(id);

            return result.IsFailure
                ? View("Error", new ErrorViewModel { ErrorMessage = result.ErrorMessage })
                : RedirectToAction("Index");
        }
    }
}