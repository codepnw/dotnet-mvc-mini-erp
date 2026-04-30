using System;

namespace MiniERP.Mvc.Entities;

public class Department
{
    public int Id { get; set; }
    public required string Title { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }

    // Collection Navigation
    public ICollection<Employee> Employees { get; set; } = [];
}
