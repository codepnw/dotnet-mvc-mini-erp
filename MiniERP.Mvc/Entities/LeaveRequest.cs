namespace MiniERP.Mvc.Entities;

public enum LeaveStatus
{
    Pending,
    Approved,
    Rejected
}

public class LeaveRequest
{
    public int Id { get; set; }

    // FK
    public required int EmployeeId { get; set; }
    public Employee? Employee { get; set; }

    // FK
    public required int LeaveTypeId { get; set; }
    public LeaveType? LeaveType { get; set; }

    public required DateTime FromDate { get; set; }
    public required DateTime ToDate { get; set; }
    public int TotalDays { get; set; }

    public string? Reason { get; set; }
    public LeaveStatus Status { get; set; } = LeaveStatus.Pending;

    public bool IsDeleted { get; set; } = false;
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
}