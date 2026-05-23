using System.ComponentModel.DataAnnotations;

namespace MiniERP.Mvc.DTOs.Requests;

public class EmployeeCreateRequest
{
    [Required(ErrorMessage = "Firstname is required"), StringLength(100)]
    public required string FirstName { get; init; }

    [Required(ErrorMessage = "Lastname is required"), StringLength(100)]
    public required string LastName { get; init; }

    [RegularExpression(@"^\d{13}$", ErrorMessage = "Citizen id required 13 digits")]
    public required string CitizenId { get; init; }

    [Range(0, 200000, ErrorMessage = "Salary range 0 - 200,000")]
    public decimal Salary { get; init; }

    [Required(ErrorMessage = "Department id is required")]
    public required int DepartmentId { get; init; }
};

public class EmployeeUpdateRequest
{
    [StringLength(100)] public string? FirstName { get; init; }

    [StringLength(100)] public string? LastName { get; init; }

    [Range(0, 200000, ErrorMessage = "Salary range 0 - 200,000")]
    public decimal? Salary { get; init; }

    public int? DepartmentId { get; init; }
}