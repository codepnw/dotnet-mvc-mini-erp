using Microsoft.JSInterop.Infrastructure;
using MiniERP.Mvc.DTOs.Requests;
using MiniERP.Mvc.DTOs.Responses;
using MiniERP.Mvc.Entities;
using MiniERP.Mvc.ViewModels;

namespace MiniERP.Mvc.Mappings;

public static class LeaveRequestMappings
{
    public static LeaveRequest ToEntity(this LeaveRequestCreateRequest request, LeaveStatus status) => new()
    {
        EmployeeId = request.EmployeeId,
        LeaveTypeId = request.LeaveTypeId,
        FromDate = request.FromDate,
        ToDate = request.ToDate,
        Reason = request.Reason,

        TotalDays = (request.ToDate.Date - request.FromDate.Date).Days + 1,
        Status = status
    };

    public static LeaveRequestDto ToDto(this LeaveRequest data) => new()
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

    public static void ApplyEdit(this LeaveRequestUpdateRequest request, LeaveRequest data)
    {
        data.FromDate = request.FromDate ?? data.FromDate;
        data.ToDate = request.ToDate ?? data.ToDate;
        data.Reason = request.Reason ?? data.Reason;
    }

    // ----------------- Start to DTO Request -----------------
    
    extension(LeaveRequestFormVm vm)
    {
        public LeaveRequestCreateRequest ToCreateRequest() => new()
        {
            EmployeeId = vm.EmployeeId,
            LeaveTypeId = vm.LeaveTypeId,
            FromDate = vm.FromDate,
            ToDate = vm.ToDate,
            Reason = vm.Reason,
        };

        public LeaveRequestUpdateRequest ToEditRequest() => new()
        {
            FromDate = vm.FromDate,
            ToDate = vm.ToDate,
            Reason = vm.Reason,
        };
    }

    // ----------------- Start to View Model -----------------
    
    public static LeaveRequestFormVm ToViewModel(this LeaveRequestDto dto) => new()
    {
        EmployeeName = $"{dto.FirstName} {dto.LastName}",
        LeaveTypeTitle = dto.LeaveTypeTitle,
        FromDate = dto.FromDate,
        ToDate = dto.ToDate,
        Reason = dto.Reason,
    };
}