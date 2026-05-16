using Microsoft.EntityFrameworkCore;
using MiniERP.Mvc.Common;
using MiniERP.Mvc.Common.Queries;
using MiniERP.Mvc.Data;
using MiniERP.Mvc.DTOs;
using MiniERP.Mvc.Extensions;
using MiniERP.Mvc.Mappings;
using MiniERP.Mvc.Models;

namespace MiniERP.Mvc.Services;

public interface IEmployeeService
{
    Task<Result<EmployeeViewModel>> CreateEmployee(EmployeeCreateDto dto);
    Task<Result<PagedResult<EmployeeViewModel>>> ListEmployees(EmployeeQuery req);
    Task<Result<EmployeeViewModel>> GetEmployeeById(int id);
    Task<Result<EmployeeViewModel>> UpdateEmployee(int id, EmployeeUpdateDto dto);
    Task<Result> DeleteEmployee(int id);
}

public class EmployeeService(AppDbContext context) : IEmployeeService
{
    private readonly AppDbContext _context = context;

    public async Task<Result<EmployeeViewModel>> CreateEmployee(EmployeeCreateDto dto)
    {
        var deptExists = await _context.Departments
            .AsNoTracking()
            .AnyAsync(x => x.Id == dto.DepartmentId);

        if (!deptExists)
            return Result<EmployeeViewModel>.Failure("Department id not found", ErrorCode.NotFound);

        var employee = dto.ToCreateEntity();

        _context.Employees.Add(employee);

        var rowAffected = await _context.SaveChangesAsync();

        return rowAffected == 0
            ? Result<EmployeeViewModel>.Failure("Insert employee failed", ErrorCode.InternalServerError)
            : Result<EmployeeViewModel>.Success(employee.ToViewModel());
    }

    public async Task<Result<PagedResult<EmployeeViewModel>>> ListEmployees(EmployeeQuery req)
    {
        var query = _context.Employees.AsNoTracking().AsQueryable();

        // Search name
        if (!string.IsNullOrEmpty(req.Search))
        {
            query = query.Where(x =>
                x.FirstName.Contains(req.Search) ||
                x.LastName.Contains(req.Search));
        }

        // Filter departmentId 
        if (req.DepartmentId is not null)
        {
            query = query.Where(x => x.DepartmentId == req.DepartmentId);
        }

        // Total Count
        var totalCount = await query.CountAsync();

        // List Items
        var items = await query
            .OrderBy(x => x.Id)
            .Paginate(req.Page, req.PageSize)
            .Select(x => new EmployeeViewModel
            {
                Id = x.Id,
                FirstName = x.FirstName,
                LastName = x.LastName,
                SalaryText = x.Salary.ToString("N2"),
                DepartmentTitle = x.Department!.Title,
            })
            .ToListAsync();

        // Response Result
        var result = new PagedResult<EmployeeViewModel>()
        {
            Items = items,
            Page = req.Page,
            PageSize = req.PageSize,
            TotalCount = totalCount
        };

        return Result<PagedResult<EmployeeViewModel>>.Success(result);
    }

    public async Task<Result<EmployeeViewModel>> GetEmployeeById(int id)
    {
        var data = await _context.Employees
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.Id == id);

        return data is null
            ? Result<EmployeeViewModel>.Failure("Employee not found", ErrorCode.NotFound)
            : Result<EmployeeViewModel>.Success(data.ToViewModel());
    }

    public async Task<Result<EmployeeViewModel>> UpdateEmployee(int id, EmployeeUpdateDto dto)
    {
        var employee = await _context.Employees.FirstOrDefaultAsync(x => x.Id == id);

        if (employee is null)
            return Result<EmployeeViewModel>.Failure("Employee not found", ErrorCode.NotFound);

        dto.ToUpdateEntity(employee);

        var rowAffected = await _context.SaveChangesAsync();

        return rowAffected == 0
            ? Result<EmployeeViewModel>.Failure("Update employee failed", ErrorCode.InternalServerError)
            : Result<EmployeeViewModel>.Success(employee.ToViewModel());
    }

    public async Task<Result> DeleteEmployee(int id)
    {
        var data = await _context.Employees.FirstOrDefaultAsync(x => x.Id == id);

        if (data is null)
            return Result.Failure("Employee not found", ErrorCode.NotFound);

        data.IsDeleted = true;
        var rowAffected = await _context.SaveChangesAsync();

        return rowAffected == 0
            ? Result.Failure("Delete employee failed", ErrorCode.InternalServerError)
            : Result.Success();
    }
}