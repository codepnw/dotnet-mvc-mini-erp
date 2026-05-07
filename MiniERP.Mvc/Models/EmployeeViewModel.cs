using System;

namespace MiniERP.Mvc.Models;

public class EmployeeViewModel
{
    public int Id { get; set; }
    public required string FirstName { get; set; }
    public required string LastName { get; set; }
    public required string SalaryText { get; set; }
    public required string DepartmentTitle { get; set; }
}
