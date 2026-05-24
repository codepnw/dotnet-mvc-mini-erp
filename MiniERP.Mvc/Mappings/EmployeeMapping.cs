using System;
using MiniERP.Mvc.DTOs.Requests;
using MiniERP.Mvc.DTOs.Responses;
using MiniERP.Mvc.Entities;
using MiniERP.Mvc.ViewModels;

namespace MiniERP.Mvc.Mappings;

public static class EmployeeMapping
{
    // ------------------ Start Mapping to Entity -----------------
    
    public static Employee ToCreateEntity(this EmployeeCreateRequest request) => new()
    {
        FirstName = request.FirstName,
        LastName = request.LastName,
        CitizenId = request.CitizenId,
        Salary = request.Salary,
        DepartmentId = request.DepartmentId
    };

    public static void ApplyEditEntity(this EmployeeEditRequest request, Employee emp)
    {
        emp.FirstName = request.FirstName;
        emp.LastName = request.LastName;
        emp.Salary = request.Salary;
        emp.DepartmentId = request.DepartmentId;
        emp.UpdatedAt = DateTime.UtcNow;
    }

    // ------------------ Start Mapping to DTO Response -----------------

    public static EmployeeDto ToResponseDto(this Employee emp) => new()
    {
        Id = emp.Id,
        FirstName = emp.FirstName,
        LastName = emp.LastName,
        Salary = emp.Salary,
        DepartmentId = emp.DepartmentId,
        DepartmentTitle = emp.Department != null ? emp.Department.Title : "N/A"
    };

    // ------------------ Start Mapping to DTO Request -----------------

    public static EmployeeEditRequest ToEditRequest(this EmployeeEditVm vm) => new()
    {
        FirstName = vm.FirstName,
        LastName = vm.LastName,
        Salary = vm.Salary,
        DepartmentId = vm.DepartmentId
    };

    public static EmployeeCreateRequest ToCreateRequest(this EmployeeCreateVm vm) => new()
    {
        FirstName = vm.FirstName,
        LastName = vm.LastName,
        Salary = vm.Salary,
        CitizenId = vm.CitizenId,
        DepartmentId = vm.DepartmentId
    };

    // ------------------ Start Mapping to View Model -----------------

    public static EmployeeEditVm ToEditViewModel(this EmployeeDto emp) => new()
    {
        Id = emp.Id,
        FirstName = emp.FirstName,
        LastName = emp.LastName,
        Salary = emp.Salary,
        DepartmentId = emp.DepartmentId,
    };
}