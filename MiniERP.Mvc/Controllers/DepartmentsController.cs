using Microsoft.AspNetCore.Mvc;
using MiniERP.Mvc.DTOs;
using MiniERP.Mvc.Services;

namespace MiniERP.Mvc.Controllers
{
    public class DepartmentsController(IDepartmentService service) : Controller
    {
        private readonly IDepartmentService _service = service;

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var result = await _service.GetDepartments();

            return Json(result);
        }

        [HttpGet]
        public async Task<IActionResult> Details(int id)
        {
            var result = await _service.GetDepartment(id);

            return Json(result);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] DepartmentDTO dto)
        {
            if (!ModelState.IsValid) return Json(ModelState);

            var result = await _service.CreateDepartment(dto);

            return Json(result);
        }

        [HttpPatch]
        public async Task<IActionResult> Edit(int id, [FromBody] DepartmentDTO dto)
        {
            if (!ModelState.IsValid) return Json(ModelState);

            var result = await _service.EditDepartment(id, dto);

            return Json(result);
        }

        [HttpDelete]
        public async Task<IActionResult> Delete(int id)
        {
            await _service.DeleteDepartment(id);

            return Json(new { message = "department deleted."});
        }
    }
}
