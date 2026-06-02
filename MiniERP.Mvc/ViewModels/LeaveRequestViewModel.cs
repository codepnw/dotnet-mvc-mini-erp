using System.ComponentModel.DataAnnotations;
using MiniERP.Mvc.Common;
using MiniERP.Mvc.Common.Queries;
using MiniERP.Mvc.DTOs.Responses;

namespace MiniERP.Mvc.ViewModels;

public class LeaveRequestIndexVm
{
    public LeaveRequestQuery Query { get; set; } = new();
    public PagedResult<LeaveRequestDto> LeaveRequests { get; set; } = new();
    public string? ErrorMessage { get; set; }
}

public class LeaveRequestDetailsVm
{
    public LeaveRequestDto LeaveRequest { get; set; } = new();
    public string? ErrorMessage { get; set; }
}

public class LeaveRequestFormVm
{
    public int Id { get; set; }

    [Required(ErrorMessage = "Employee id is required")]
    public int EmployeeId { get; set; }

    public string EmployeeName { get; set; } = "";

    [Required(ErrorMessage = "Leave type is required")]
    public int LeaveTypeId { get; set; }

    public string LeaveTypeTitle { get; set; } = "";

    [Required(ErrorMessage = "From date is required")]
    public DateTime FromDate { get; set; } = DateTime.Today;

    [Required(ErrorMessage = "To date is required")]
    public DateTime ToDate { get; set; } = DateTime.Today;

    public List<LeaveRequestEmployeeDto> EmployeesOptions { get; set; } = [];
    public List<LeaveRequestTypeDto> LeaveTypeOptions { get; set; } = [];

    public string? Reason { get; set; }
    public string? ErrorMessage { get; set; }
}