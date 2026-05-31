using Microsoft.AspNetCore.Mvc;
using MiniERP.Mvc.Common.Queries;
using MiniERP.Mvc.DTOs.Requests;
using MiniERP.Mvc.Mappings;
using MiniERP.Mvc.Services;
using MiniERP.Mvc.ViewModels;

namespace MiniERP.Mvc.Controllers
{
    public class DepartmentsController(IDepartmentService service) : Controller
    {
        private readonly IDepartmentService _service = service;

        [HttpGet]
        public async Task<IActionResult> Index(DepartmentQuery query)
        {
            var result = await _service.ListDepartments(query);

            var vm = new DepartmentIndexVm()
            {
                Query = query
            };

            if (result.IsFailure)
            {
                vm.ErrorMessage = result.ErrorMessage;
                return View(vm);
            }

            vm.Departments = result.Data!;
            return View(vm);
        }

        [HttpGet]
        public async Task<IActionResult> Details(int id)
        {
            var result = await _service.GetDepartment(id);

            var vm = new DepartmentDetailsVm();

            if (result.IsFailure)
            {
                vm.ErrorMessage = result.ErrorMessage;
                return View(vm);
            }

            vm.Department = result.Data!;
            return View(vm);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View(new DepartmentFormVm());
        }

        [HttpPost]
        public async Task<IActionResult> Create(DepartmentFormVm vm)
        {
            if (!ModelState.IsValid) return View(vm);

            var result = await _service.CreateDepartment(vm.ToRequest());

            if (result.IsFailure)
            {
                ModelState.AddModelError("", result.ErrorMessage!);
                return View(vm);
            }

            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var result = await _service.GetDepartment(id);

            if (result.IsFailure)
            {
                return View(new DepartmentFormVm
                {
                    ErrorMessage = result.ErrorMessage
                });
            }

            return View(result.Data!.ToViewModel());
        }

        [HttpPost]
        public async Task<IActionResult> Edit(int id, DepartmentFormVm vm)
        {
            if (!ModelState.IsValid) return View(vm);

            var result = await _service.EditDepartment(id, vm.ToRequest());

            if (result.IsFailure)
            {
                ModelState.AddModelError("", result.ErrorMessage!);
                return View(vm);
            }

            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _service.DeleteDepartment(id);

            return result.IsFailure
                ? View("Error", new ErrorViewModel { ErrorMessage = result.ErrorMessage })
                : RedirectToAction("Index");
        }
    }
}