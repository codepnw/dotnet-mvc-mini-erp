using System.ComponentModel.DataAnnotations;

namespace MiniERP.Mvc.DTOs;

public record class EmployeeCreateDTO(
    [Required(ErrorMessage = "firstname is required"), StringLength(50)] string FirstName,
    [Required(ErrorMessage = "lastname is required"), StringLength(50)] string LastName,
    [Range(0, 200000, ErrorMessage = "salary range 0 - 200,000")] decimal Salary,
    [Required(ErrorMessage = "department id is required")] int DepartmentId
);

public record class EmployeeUpdateDTO(
    string? FirstName,
    string? LastName,
    decimal? Salary,
    int? DepartmentId
);
