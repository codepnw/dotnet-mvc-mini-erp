using Microsoft.JSInterop.Infrastructure;
using MiniERP.Mvc.DTOs;
using MiniERP.Mvc.Entities;
using MiniERP.Mvc.Models;

namespace MiniERP.Mvc.Mappings;

public static class LeaveRequestMappings
{
    public static LeaveRequest ToLeaveRequestEntity(this LeaveRequestCreateDto dto, LeaveStatus status) => new()
    {
        EmployeeId = dto.EmployeeId,
        LeaveTypeId = dto.LeaveTypeId,
        FromDate = dto.FromDate,
        ToDate = dto.ToDate,
        Reason = dto.Reason,

        TotalDays = (dto.ToDate.Date - dto.FromDate.Date).Days + 1,
        Status = status
    };

    public static LeaveRequestViewModel ToLeaveRequestViewModel(this LeaveRequest data) => new()
    {
        Id = data.Id,
        FirstName = data.Employee?.FirstName ?? "N/A",
        LastName = data.Employee?.LastName ?? "N/A",
        LeaveTypeTitle =  data.LeaveType?.Title ?? "N/A",
        FromDate = data.FromDate,
        ToDate = data.ToDate,
        TotalDays = data.TotalDays,
        Reason = data.Reason,
        Status = data.Status
    };

    public static void ApplyUpdateLeaveRequest(this LeaveRequestUpdateDto dto, LeaveRequest data)
    {
        data.FromDate = dto.FromDate ?? data.FromDate;
        data.ToDate = dto.ToDate ?? data.ToDate;
        data.Reason = dto.Reason ?? data.Reason;
    }
}