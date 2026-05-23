namespace MiniERP.Mvc.DTOs.Responses;

public class EmployeeDto
{
    public int Id { get; set; }
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public decimal Salary { get; set; }
    public int DepartmentId { get; set; }
    public string DepartmentTitle { get; set; } = string.Empty;
}