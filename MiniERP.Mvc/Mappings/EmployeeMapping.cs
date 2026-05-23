using System;
using MiniERP.Mvc.DTOs.Requests;
using MiniERP.Mvc.DTOs.Responses;
using MiniERP.Mvc.Entities;
using MiniERP.Mvc.Models;

namespace MiniERP.Mvc.Mappings;

public static class EmployeeMapping
{
    public static Employee ToCreateEntity(this EmployeeCreateRequest request) => new()
    {
        FirstName = request.FirstName,
        LastName = request.LastName,
        CitizenId = request.CitizenId,
        Salary = request.Salary,
        DepartmentId = request.DepartmentId
    };

    public static void ApplyUpdateEntity(this EmployeeUpdateRequest request, Employee emp)
    {
        emp.FirstName = request.FirstName ?? emp.FirstName;
        emp.LastName = request.LastName ?? emp.LastName;
        emp.Salary = request.Salary ?? emp.Salary;
        emp.DepartmentId = request.DepartmentId ?? emp.DepartmentId;
        emp.UpdatedAt = DateTime.UtcNow;
    }

    public static EmployeeDto ToEmployeeDto(this Employee emp) => new()
    {
        Id = emp.Id,
        FirstName = emp.FirstName,
        LastName = emp.LastName,
        Salary = emp.Salary,
        DepartmentId = emp.DepartmentId,
        DepartmentTitle = emp.Department != null ? emp.Department.Title : "N/A"
    };

    // public static EmployeeControllerEditVm ToViewController(this EmployeeViewModel emp) => new()
    // {
    //     Id = emp.Id,
    //     FirstName = emp.FirstName,
    //     LastName = emp.LastName,
    //
    //     // TODO: type mismatch
    //     // SalaryText = emp.SalaryText.ToString(),
    //     // DepartmentTitle = emp.DepartmentTitle
    // };

    public static EmployeeUpdateRequest ToEditDto(this EmployeeControllerEditVm vm) => new()
    {
        FirstName = vm.FirstName,
        LastName = vm.LastName,
        Salary = vm.Salary,

        // TODO: type mismatch
        // DepartmentId = vm.DepartmentTitle
    };
}