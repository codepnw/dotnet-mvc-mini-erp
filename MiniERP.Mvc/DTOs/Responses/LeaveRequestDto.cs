using MiniERP.Mvc.Entities;

namespace MiniERP.Mvc.DTOs.Responses;

public class LeaveRequestDto
{
    public int Id { get; set; }
    public required string FirstName { get; set; }
    public required string LastName { get; set; }
    public required string LeaveTypeTitle { get; set; }
    
    public DateTime FromDate { get; set; }
    public DateTime ToDate { get; set; }
    public int TotalDays { get; set; }
    
    public string? Reason { get; set; }
    public LeaveStatus Status { get; set; }
}