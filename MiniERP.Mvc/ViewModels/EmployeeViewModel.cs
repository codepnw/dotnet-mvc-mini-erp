using System;
using System.ComponentModel.DataAnnotations;
using MiniERP.Mvc.Common;
using MiniERP.Mvc.Common.Queries;
using MiniERP.Mvc.DTOs.Responses;

namespace MiniERP.Mvc.ViewModels;

public class EmployeeIndexVm
{
    public EmployeeQuery Query { get; set; } = new();
    public PagedResult<EmployeeDto> Employees { get; set; } = new();
    public string? ErrorMessage { get; set; }
}

public class EmployeeDetailsVm
{
    public EmployeeDto Employee { get; set; } = new();
    public string? ErrorMessage { get; set; }
}

public class EmployeeCreateVm
{
    [Required(ErrorMessage = "Firstname is required"), StringLength(100)]
    public string FirstName { get; init; } = string.Empty;

    [Required(ErrorMessage = "Lastname is required"), StringLength(100)]
    public string LastName { get; init; } = string.Empty;

    [RegularExpression(@"^\d{13}$", ErrorMessage = "Citizen id required 13 digits")]
    public required string CitizenId { get; init; }

    [Range(0, 200000, ErrorMessage = "Salary range 0 - 200,000")]
    public decimal Salary { get; init; }

    [Required(ErrorMessage = "Department id is required")]
    public required int DepartmentId { get; init; }
}

public class EmployeeEditVm
{
    public int Id { get; set; }

    [Required(ErrorMessage = "Firstname is required"), StringLength(100)]
    public string FirstName { get; set; } = string.Empty;

    [Required(ErrorMessage = "Lastname is required"), StringLength(100)]
    public string LastName { get; set; } = string.Empty;

    [Range(0, 200000, ErrorMessage = "Salary range 0 - 200,000")]
    public decimal Salary { get; set; }

    [Required] public int DepartmentId { get; set; }

    public string? ErrorMessage { get; set; }
}