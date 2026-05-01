using System.Runtime.CompilerServices;
using System.Security.Cryptography;
using Microsoft.AspNetCore.Mvc;
using MiniERP.Mvc.DTOs;
using MiniERP.Mvc.Services;

namespace MiniERP.Mvc.Controllers
{
    public class EmployeesController(IEmployeeService service) : Controller
    {
        private readonly IEmployeeService _service = service;

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var employees = await _service.ListEmployees();

            // return View(employees);
            return Json(employees);
        }

        [HttpGet]
        public async Task<IActionResult> Details(int id)
        {
            var result = await _service.GetEmployeeById(id);

            return Json(result);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] EmployeeCreateDTO dto)
        {
            if (!ModelState.IsValid) return Json(ModelState);

            var result = await _service.CreateEmployee(dto);

            return Json(result);
        }

        [HttpPatch]
        public async Task<IActionResult> Edit(int id, [FromBody] EmployeeUpdateDTO dto)
        {
            var result = await _service.UpdateEmployee(id, dto);

            return Json(result);
        }

        [HttpDelete]
        public async Task<IActionResult> Delete(int id)
        {
            await _service.DeleteEmployee(id);

            return Json(new { message = "employee deleted"});
        }
    }
}
