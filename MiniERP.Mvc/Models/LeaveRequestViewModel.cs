using MiniERP.Mvc.Entities;

namespace MiniERP.Mvc.Models;

public class LeaveRequestViewModel
{
    public int Id { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string LeaveTypeTitle { get; set; }
    
    public DateTime FromDate { get; set; }
    public DateTime ToDate { get; set; }
    public int TotalDays { get; set; }
    
    public string? Reason { get; set; }
    public LeaveStatus Status { get; set; }
}