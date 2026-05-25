namespace MiniERP.Mvc.DTOs.Responses;

public class EmployeeDto
{
    public int Id { get; set; }
    public string FirstName { get; set; } = "";
    public string LastName { get; set; } = "";
    public decimal Salary { get; set; }
    public int DepartmentId { get; set; }
    public string DepartmentTitle { get; set; } = "";
}