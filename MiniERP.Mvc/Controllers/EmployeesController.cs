using System.Runtime.CompilerServices;
using System.Security.Cryptography;
using Microsoft.AspNetCore.Mvc;
using MiniERP.Mvc.Common.Queries;
using MiniERP.Mvc.DTOs;
using MiniERP.Mvc.Models;
using MiniERP.Mvc.Services;

namespace MiniERP.Mvc.Controllers
{
    public class EmployeesController(IEmployeeService service) : Controller
    {
        private readonly IEmployeeService _service = service;

        [HttpGet]
        public async Task<IActionResult> Index(EmployeeQuery req)
        {
            var result = await _service.ListEmployees(req);

            if (!result.IsFailure) return View(result.Data);

            ModelState.AddModelError("", result.ErrorMessage!);
            return View(req);

            // return result.IsFailure
            //     ? View("Error", new ErrorViewModel { ErrorMessage = result.ErrorMessage })
            //     : View(result.Data);
        }

        [HttpGet]
        public async Task<IActionResult> Details(int id)
        {
            var result = await _service.GetEmployeeById(id);

            return result.IsFailure
                ? View("Error", new ErrorViewModel { ErrorMessage = result.ErrorMessage })
                : View(result.Data);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(EmployeeCreateDto dto)
        {
            if (!ModelState.IsValid) return View(dto);
            
            var result = await _service.CreateEmployee(dto);

            if (!result.IsFailure) return RedirectToAction("Index");

            ModelState.AddModelError("", result.ErrorMessage!);
            return View(dto);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var result = await _service.GetEmployeeById(id);

            return result.IsFailure
                ? View("Error", new ErrorViewModel { ErrorMessage = result.ErrorMessage })
                : View(result.Data);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(int id, EmployeeUpdateDto dto)
        {
            var result = await _service.UpdateEmployee(id, dto);

            return result.IsFailure
                ? View("Error", new ErrorViewModel { ErrorMessage = result.ErrorMessage })
                : RedirectToAction("Index");
        }

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