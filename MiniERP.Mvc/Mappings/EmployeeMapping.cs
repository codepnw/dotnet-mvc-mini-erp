using System;
using MiniERP.Mvc.DTOs;
using MiniERP.Mvc.Entities;
using MiniERP.Mvc.Models;

namespace MiniERP.Mvc.Mappings;

public static class EmployeeMapping
{
    public static Employee ToCreateEntity(this EmployeeCreateDTO dto) => new()
    {
        FirstName = dto.FirstName,
        LastName = dto.LastName,
        CitizenId = dto.CitizenId,
        Salary = dto.Salary,
        DepartmentId = dto.DepartmentId
    };

    public static void ToUpdateEntity(this EmployeeUpdateDTO dto, Employee emp)
    {
        emp.FirstName = dto.FirstName ?? emp.FirstName;
        emp.LastName = dto.LastName ?? emp.LastName;
        emp.Salary = dto.Salary ?? emp.Salary;
        emp.DepartmentId = dto.DepartmentId ?? emp.DepartmentId;
        emp.UpdatedAt = DateTime.UtcNow;
    }

    public static EmployeeViewModel ToViewModel(this Employee emp) => new()
    {
        Id = emp.Id,
        FirstName = emp.FirstName,
        LastName = emp.LastName,
        SalaryText = emp.Salary.ToString("N2"),
        DepartmentTitle = emp.Department != null ? emp.Department.Title : "N/A"
    };
}
