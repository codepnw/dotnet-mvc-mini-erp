using System.ComponentModel.DataAnnotations;

namespace MiniERP.Mvc.DTOs.Requests;

public class EmployeeCreateRequest
{
    public string FirstName { get; init; } = string.Empty;
    public string LastName { get; init; } = string.Empty;
    public required string CitizenId { get; init; }
    public decimal Salary { get; init; }
    public required int DepartmentId { get; init; }
};

public class EmployeeEditRequest
{
    public string FirstName { get; init; } = string.Empty;
    public string LastName { get; init; } = string.Empty;
    public decimal Salary { get; init; }
    public int DepartmentId { get; init; }
}