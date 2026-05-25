using System.ComponentModel.DataAnnotations;

namespace MiniERP.Mvc.DTOs.Requests;

public class LeaveRequestCreateRequest
{
    public int EmployeeId { get; init; }
    public int LeaveTypeId { get; init; }
    public DateTime FromDate { get; init; }
    public DateTime ToDate { get; init; }
    public string? Reason { get; init; }
};

public class LeaveRequestUpdateRequest
{
    public DateTime? FromDate { get; init; }
    public DateTime? ToDate { get; init; }
    public string? Reason { get; init; }
};