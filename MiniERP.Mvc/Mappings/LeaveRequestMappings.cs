using Microsoft.JSInterop.Infrastructure;
using MiniERP.Mvc.DTOs.Requests;
using MiniERP.Mvc.DTOs.Responses;
using MiniERP.Mvc.Entities;
using MiniERP.Mvc.Models;

namespace MiniERP.Mvc.Mappings;

public static class LeaveRequestMappings
{
    public static LeaveRequest ToLeaveRequestEntity(this LeaveRequestCreateRequest request, LeaveStatus status) => new()
    {
        EmployeeId = request.EmployeeId,
        LeaveTypeId = request.LeaveTypeId,
        FromDate = request.FromDate,
        ToDate = request.ToDate,
        Reason = request.Reason,

        TotalDays = (request.ToDate.Date - request.FromDate.Date).Days + 1,
        Status = status
    };

    public static LeaveRequestDto ToLeaveRequestDto(this LeaveRequest data) => new()
    {
        Id = data.Id,
        FirstName = data.Employee?.FirstName ?? "N/A",
        LastName = data.Employee?.LastName ?? "N/A",
        LeaveTypeTitle = data.LeaveType?.Title ?? "N/A",
        FromDate = data.FromDate,
        ToDate = data.ToDate,
        TotalDays = data.TotalDays,
        Reason = data.Reason,
        Status = data.Status
    };

    public static void ApplyUpdateLeaveRequest(this LeaveRequestUpdateRequest request, LeaveRequest data)
    {
        data.FromDate = request.FromDate ?? data.FromDate;
        data.ToDate = request.ToDate ?? data.ToDate;
        data.Reason = request.Reason ?? data.Reason;
    }
}