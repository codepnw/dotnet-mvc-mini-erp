using MiniERP.Mvc.Entities;

namespace MiniERP.Mvc.DTOs.Responses;

public class LeaveRequestDto
{
    public int Id { get; set; }
    public string FirstName { get; set; } = "";
    public string LastName { get; set; } = "";
    public string LeaveTypeTitle { get; set; } = "";

    public DateTime FromDate { get; set; }
    public DateTime ToDate { get; set; }
    public int TotalDays { get; set; }

    public string? Reason { get; set; }
    public LeaveStatus Status { get; set; }
}

public class LeaveRequestEmployeeDto
{
    public int Id { get; set; }
    public string FirstName { get; set; } = "";
    public string LastName { get; set; } = "";
}

public class LeaveRequestTypeDto
{
    public int Id { get; set; }
    public string Title { get; set; } = "";
}