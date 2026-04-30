using System;

namespace MiniERP.Mvc.Entities;

public class Employee
{
    public int Id { get; set; }
    public required string FirstName { get; set; }
    public required string LastName { get; set; }
    public decimal Salary { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }

    // FK
    public required int DepartmentId { get; set; }
    // Reference Navigation
    public Department? Department { get; set; }
}
