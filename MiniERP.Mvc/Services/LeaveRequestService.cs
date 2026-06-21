using Microsoft.EntityFrameworkCore;
using MiniERP.Mvc.Common;
using MiniERP.Mvc.Common.Constants;
using MiniERP.Mvc.Common.CurrentUser;
using MiniERP.Mvc.Common.Queries;
using MiniERP.Mvc.Data;
using MiniERP.Mvc.DTOs.Requests;
using MiniERP.Mvc.DTOs.Responses;
using MiniERP.Mvc.Entities;
using MiniERP.Mvc.Extensions;
using MiniERP.Mvc.Mappings;
using MiniERP.Mvc.ViewModels;

namespace MiniERP.Mvc.Services;

public interface ILeaveRequestService
{
    Task<Result> CreateLeaveRequest(LeaveRequestCreateRequest request);
    Task<Result<LeaveRequestDto>> GetLeaveRequest(int id);
    Task<Result<PagedResult<LeaveRequestDto>>> ListLeaveRequests(LeaveRequestQuery req);
    Task<Result> UpdateLeaveRequest(int id, LeaveRequestUpdateRequest request);
    Task<Result> UpdateLeaveRequestStatus(int id, LeaveStatus status);
    Task<Result> DeleteLeaveRequest(int id);

    // For View Model Select Options
    Task<List<LeaveRequestEmployeeDto>> ListEmployees();
    Task<List<LeaveRequestTypeDto>> ListLeaveTypes();
}

public class LeaveRequestService(AppDbContext context, ICurrentUser currentUser) : ILeaveRequestService
{
    private readonly AppDbContext _context = context;
    private readonly ICurrentUser _currentUser = currentUser;

    public async Task<Result> CreateLeaveRequest(LeaveRequestCreateRequest request)
    {
        if (request.FromDate > request.ToDate)
            return Result.Failure("Invalid date range", ErrorCode.BadRequest);

        var empExists = await _context.Employees
            .AsNoTracking()
            .AnyAsync(x => x.Id == request.EmployeeId);

        if (!empExists)
            return Result.Failure("Employee not found", ErrorCode.NotFound);

        var typeExists = await _context.LeaveTypes
            .AsNoTracking()
            .AnyAsync(x => x.Id == request.LeaveTypeId);

        if (!typeExists)
            return Result.Failure("Leave type not found", ErrorCode.NotFound);

        var overlap = await _context.LeaveRequests
            .AnyAsync(x =>
                x.EmployeeId == request.EmployeeId &&
                request.FromDate <= x.ToDate &&
                request.ToDate >= x.FromDate
            );

        if (overlap)
            return Result.Failure("Leve request overlaps", ErrorCode.BadRequest);

        var reqEntity = request.ToEntity(LeaveStatus.Pending);

        _context.LeaveRequests.Add(reqEntity);
        await _context.SaveChangesAsync();

        return Result.Success();
    }

    public async Task<Result<LeaveRequestDto>> GetLeaveRequest(int id)
    {
        var data = await _context.LeaveRequests
            .AsNoTracking()
            .Include(x => x.Employee)
            .Include(x => x.LeaveType)
            .FirstOrDefaultAsync(x => x.Id == id);

        return data is null
            ? Result<LeaveRequestDto>.Failure("Leave request not found", ErrorCode.NotFound)
            : Result<LeaveRequestDto>.Success(data.ToDto());
    }

    public async Task<Result<PagedResult<LeaveRequestDto>>> ListLeaveRequests(LeaveRequestQuery req)
    {
        if (!_currentUser.IsAdmin) return Result<PagedResult<LeaveRequestDto>>.Failure(Errors.ErrNoPermission, ErrorCode.Forbiden);

        var query = _context.LeaveRequests.AsNoTracking().AsQueryable();

        // Global Search: Id, FirstName, LastName
        if (!string.IsNullOrWhiteSpace(req.Search))
        {
            var isId = int.TryParse(req.Search, out var id);

            query = query.Where(x => 
                x.Employee!.FirstName.Contains(req.Search) || 
                x.Employee.LastName.Contains(req.Search) || 
                (isId && x.Id == id));
        }

        // Search Status
        if (req.Status.HasValue)
        {
            query = query.Where(x => x.Status == req.Status);
        }

        // Filter From Date
        if (req.FromDate.HasValue)
        {
            query = query.Where(x => x.FromDate.Date >= req.FromDate.Value.Date);
        }

        // Filter To Date
        if (req.ToDate.HasValue)
        {
            query = query.Where(x => x.ToDate.Date <= req.ToDate.Value.Date);
        }

        var totalCount = await query.CountAsync();

        // List Items
        var items = await query
            .OrderBy(x => x.Id)
            .Paginate(req.Page, req.PageSize)
            .Select(x => new LeaveRequestDto
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

        // Response Result
        var result = new PagedResult<LeaveRequestDto>
        {
            Items = items,
            Page = req.Page,
            PageSize = req.PageSize,
            TotalCount = totalCount
        };

        return Result<PagedResult<LeaveRequestDto>>.Success(result);
    }

    public async Task<Result> UpdateLeaveRequest(int id, LeaveRequestUpdateRequest request)
    {
        var result = await FindLeaveRequestById(id);
        var data = result.Data;

        if (data is null)
            return Result.Failure(result.ErrorMessage!, result.ErrorCode);

        var overlap = await _context.LeaveRequests
            .AnyAsync(x =>
                x.Id != id &&
                x.EmployeeId == data.EmployeeId &&
                request.FromDate <= x.ToDate &&
                request.ToDate >= x.FromDate
            );
        if (overlap)
            return Result.Failure("Leave request overlaps", ErrorCode.BadRequest);

        request.ApplyEdit(data);

        data.TotalDays = (data.ToDate.Date - data.FromDate.Date).Days + 1;
        data.UpdatedAt = DateTime.UtcNow;
        await _context.SaveChangesAsync();

        return Result.Success();
    }

    public async Task<Result> UpdateLeaveRequestStatus(int id, LeaveStatus status)
    {
        if (!_currentUser.IsAdmin) return Result.Failure(Errors.ErrNoPermission, ErrorCode.Forbiden);

        var result = await FindLeaveRequestById(id);
        var data = result.Data;

        if (data is null)
            return Result.Failure(result.ErrorMessage!, result.ErrorCode);

        if (data.Status != LeaveStatus.Pending)
            return Result.Failure("Only status pending can update", result.ErrorCode);

        data.Status = status;
        data.UpdatedAt = DateTime.UtcNow;
        await _context.SaveChangesAsync();

        return Result.Success();
    }

    public async Task<Result> DeleteLeaveRequest(int id)
    {
        if (!_currentUser.IsAdmin) return Result.Failure(Errors.ErrNoPermission, ErrorCode.Forbiden);

        var result = await FindLeaveRequestById(id);
        var data = result.Data;

        if (data is null)
            return Result.Failure(result.ErrorMessage!, result.ErrorCode);

        data.IsDeleted = true;
        await _context.SaveChangesAsync();

        return Result.Success();
    }

    public async Task<List<LeaveRequestEmployeeDto>> ListEmployees()
    {
        return await _context.Employees
            .AsNoTracking()
            .Select(x => new LeaveRequestEmployeeDto
            {
                Id = x.Id,
                FirstName = x.FirstName,
                LastName = x.LastName,
            })
            .ToListAsync();
    }

    public async Task<List<LeaveRequestTypeDto>> ListLeaveTypes()
    {
        return await _context.LeaveTypes
            .AsNoTracking()
            .Select(x => new LeaveRequestTypeDto
            {
                Id = x.Id,
                Title = x.Title,
            })
            .ToListAsync();
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