using Microsoft.EntityFrameworkCore;
using MiniERP.Mvc.Common;
using MiniERP.Mvc.Common.Queries;
using MiniERP.Mvc.Data;
using MiniERP.Mvc.DTOs.Requests;
using MiniERP.Mvc.DTOs.Responses;
using MiniERP.Mvc.Extensions;
using MiniERP.Mvc.Mappings;

namespace MiniERP.Mvc.Services;

public interface IEmployeeService
{
    Task<Result<EmployeeDto>> CreateEmployee(EmployeeCreateRequest request);
    Task<Result<PagedResult<EmployeeDto>>> ListEmployees(EmployeeQuery request);
    Task<Result<EmployeeDto>> GetEmployeeById(int id);
    Task<Result> UpdateEmployee(int id, EmployeeEditRequest request);
    Task<Result> DeleteEmployee(int id);
}

public class EmployeeService(AppDbContext context) : IEmployeeService
{
    private readonly AppDbContext _context = context;

    public async Task<Result<EmployeeDto>> CreateEmployee(EmployeeCreateRequest request)
    {
        var deptExists = await _context.Departments
            .AsNoTracking()
            .AnyAsync(x => x.Id == request.DepartmentId);

        if (!deptExists)
            return Result<EmployeeDto>.Failure("Department id not found", ErrorCode.NotFound);

        var employee = request.ToCreateEntity();

        _context.Employees.Add(employee);
        await _context.SaveChangesAsync();

        return Result<EmployeeDto>.Success(employee.ToResponseDto());
    }

    public async Task<Result<PagedResult<EmployeeDto>>> ListEmployees(EmployeeQuery request)
    {
        var query = _context.Employees.AsNoTracking().AsQueryable();

        // Global Search: Id, Firstname, Lastname, DepartmentTitle
        if (!string.IsNullOrEmpty(request.Search))
        {
            var isId = int.TryParse(request.Search, out var id);

            query = query.Where(x =>
                x.FirstName.Contains(request.Search) ||
                x.LastName.Contains(request.Search) ||
                (x.Department != null && x.Department!.Title.Contains(request.Search)) ||
                (isId && x.Id == id));
        }

        // Total Count
        var totalCount = await query.CountAsync();

        // List Items
        var items = await query
            .OrderBy(x => x.Id)
            .Paginate(request.Page, request.PageSize)
            .Select(x => new EmployeeDto
            {
                Id = x.Id,
                FirstName = x.FirstName,
                LastName = x.LastName,
                Salary = x.Salary,
                DepartmentId = x.DepartmentId,
                DepartmentTitle = x.Department!.Title,
            })
            .ToListAsync();

        // Response Result
        var result = new PagedResult<EmployeeDto>()
        {
            Items = items,
            Page = request.Page,
            PageSize = request.PageSize,
            TotalCount = totalCount
        };

        return Result<PagedResult<EmployeeDto>>.Success(result);
    }

    public async Task<Result<EmployeeDto>> GetEmployeeById(int id)
    {
        var data = await _context.Employees
            .AsNoTracking()
            .Include(x => x.Department)
            .FirstOrDefaultAsync(x => x.Id == id);

        return data is null
            ? Result<EmployeeDto>.Failure("Employee not found", ErrorCode.NotFound)
            : Result<EmployeeDto>.Success(data.ToResponseDto());
    }

    public async Task<Result> UpdateEmployee(int id, EmployeeEditRequest request)
    {
        var employee = await _context.Employees.FirstOrDefaultAsync(x => x.Id == id);

        if (employee is null)
            return Result.Failure("Employee not found", ErrorCode.NotFound);

        request.ApplyEditEntity(employee);
        await _context.SaveChangesAsync();

        return Result.Success();
    }

    public async Task<Result> DeleteEmployee(int id)
    {
        var data = await _context.Employees.FirstOrDefaultAsync(x => x.Id == id);

        if (data is null)
            return Result.Failure("Employee not found", ErrorCode.NotFound);

        data.IsDeleted = true;
        await _context.SaveChangesAsync();

        return Result.Success();
    }
}