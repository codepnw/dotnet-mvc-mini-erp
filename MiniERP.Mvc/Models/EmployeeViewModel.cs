using System;

namespace MiniERP.Mvc.Models;

public class EmployeeViewModel
{
    public int Id { get; set; }
    public required string FullName { get; set; }
    public required string SalaryText { get; set; }
    public required string DepartmentTitle { get; set; }
}
