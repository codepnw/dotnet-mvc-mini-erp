using Microsoft.EntityFrameworkCore;
using MiniERP.Mvc.Common;
using MiniERP.Mvc.Data;
using MiniERP.Mvc.DTOs;
using MiniERP.Mvc.Entities;
using MiniERP.Mvc.Mappings;
using MiniERP.Mvc.Models;

namespace MiniERP.Mvc.Services;

public interface ILeaveRequestService
{
    Task<Result> CreateLeaveRequest(LeaveRequestCreateDTO dto);
    Task<Result<LeaveRequestViewModel>> GetLeaveRequest(int id);
    Task<Result<IReadOnlyList<LeaveRequestViewModel>>> GetAllLeaveRequests();
    Task<Result> UpdateLeaveRequest(int id, LeaveRequestUpdateDTO dto);
    Task<Result> UpdateLeaveRequestStatus(int id, LeaveStatus status);
    Task<Result> DeleteLeaveRequest(int id);
}

public class LeaveRequestService(AppDbContext context) : ILeaveRequestService
{
    private readonly AppDbContext _context = context;

    public async Task<Result> CreateLeaveRequest(LeaveRequestCreateDTO dto)
    {
        if (dto.FromDate > dto.ToDate)
            return Result.Failure("Invalid date range", ErrorCode.BadRequest);

        var empExists = await _context.Employees
            .AsNoTracking()
            .AnyAsync(x => x.Id == dto.EmployeeId);

        if (!empExists)
            return Result.Failure("Employee not found", ErrorCode.NotFound);

        var typeExists = await _context.LeaveTypes
            .AsNoTracking()
            .AnyAsync(x => x.Id == dto.LeaveTypeId);

        if (!typeExists)
            return Result.Failure("Leave type not found", ErrorCode.NotFound);

        var overlap = await _context.LeaveRequests
            .AnyAsync(x =>
                x.EmployeeId == dto.EmployeeId &&
                dto.FromDate <= x.ToDate &&
                dto.ToDate >= x.FromDate
            );

        if (overlap)
            return Result.Failure("Leve request overlaps", ErrorCode.BadRequest);

        var reqEntity = dto.ToLeaveRequestEntity(LeaveStatus.Pending);

        _context.LeaveRequests.Add(reqEntity);
        var rowAffected = await _context.SaveChangesAsync();

        return rowAffected == 0
            ? Result.Failure("Insert leave request failed", ErrorCode.InternalServerError)
            : Result.Success();
    }

    public async Task<Result<LeaveRequestViewModel>> GetLeaveRequest(int id)
    {
        var data = await _context.LeaveRequests
            .AsNoTracking()
            .Include(x => x.Employee)
            .Include(x => x.LeaveType)
            .FirstOrDefaultAsync(x => x.Id == id);

        return data is null
            ? Result<LeaveRequestViewModel>.Failure("Leave request not found", ErrorCode.NotFound)
            : Result<LeaveRequestViewModel>.Success(data.ToLeaveRequestViewModel());
    }

    public async Task<Result<IReadOnlyList<LeaveRequestViewModel>>> GetAllLeaveRequests()
    {
        var listData = await _context.LeaveRequests
            .AsNoTracking()
            .Where(x => !x.IsDeleted)
            .Select(x => new LeaveRequestViewModel
            {
                Id = x.Id,
                FirstName = x.Employee!.FirstName,
                LastName = x.Employee.LastName,
                LeaveTypeTitle = x.LeaveType!.Title,
                FromDate = x.FromDate,
                ToDate = x.ToDate,
                TotalDays = x.TotalDays,
                Reason = x.Reason,
                Status = x.Status
            })
            .ToListAsync();

        return Result<IReadOnlyList<LeaveRequestViewModel>>.Success(listData);
    }

    public async Task<Result> UpdateLeaveRequest(int id, LeaveRequestUpdateDTO dto)
    {
        var result = await FindLeaveRequestById(id);
        var data = result.Data;

        if (data is null)
            return Result.Failure(result.ErrorMessage!, result.ErrorCode);

        var overlap = await _context.LeaveRequests
            .AnyAsync(x =>
                x.Id != id &&
                x.EmployeeId == data.EmployeeId &&
                dto.FromDate <= x.ToDate &&
                dto.ToDate >= x.FromDate
            );
        if (overlap)
            return Result.Failure("Leave request overlaps", ErrorCode.BadRequest);

        dto.ApplyUpdateLeaveRequest(data);

        data.TotalDays = (data.ToDate.Date - data.FromDate.Date).Days + 1;
        data.UpdatedAt = DateTime.UtcNow;

        var rowAffected = await _context.SaveChangesAsync();

        return rowAffected == 0
            ? Result.Failure("Update leave request failed", ErrorCode.InternalServerError)
            : Result.Success();
    }

    public async Task<Result> UpdateLeaveRequestStatus(int id, LeaveStatus status)
    {
        var result = await FindLeaveRequestById(id);
        var data = result.Data;

        if (data is null)
            return Result.Failure(result.ErrorMessage!, result.ErrorCode);

        if (data.Status != LeaveStatus.Pending)
            return Result.Failure("Only status pending can update", result.ErrorCode);

        data.Status = status;
        data.UpdatedAt = DateTime.UtcNow;
        var rowAffected = await _context.SaveChangesAsync();

        return rowAffected == 0
            ? Result.Failure("Update leave request failed", ErrorCode.InternalServerError)
            : Result.Success();
    }

    public async Task<Result> DeleteLeaveRequest(int id)
    {
        var result = await FindLeaveRequestById(id);
        var data = result.Data;

        if (data is null)
            return Result.Failure(result.ErrorMessage!, result.ErrorCode);

        data.IsDeleted = true;
        var rowAffected = await _context.SaveChangesAsync();

        return rowAffected == 0
            ? Result.Failure("Delete leave request failed", ErrorCode.InternalServerError)
            : Result.Success();
    }

    private async Task<Result<LeaveRequest>> FindLeaveRequestById(int id)
    {
        var data = await _context.LeaveRequests
            .FirstOrDefaultAsync(x => x.Id == id);

        return data is null
            ? Result<LeaveRequest>.Failure("Leave request not found", ErrorCode.NotFound)
            : Result<LeaveRequest>.Success(data);
    }
}