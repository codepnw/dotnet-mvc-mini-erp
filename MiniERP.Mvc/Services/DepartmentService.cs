using System;
using System.Drawing;
using System.Reflection.Metadata.Ecma335;
using System.Runtime.CompilerServices;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using Microsoft.EntityFrameworkCore.Query.Internal;
using MiniERP.Mvc.Common;
using MiniERP.Mvc.Common.Queries;
using MiniERP.Mvc.Data;
using MiniERP.Mvc.DTOs.Requests;
using MiniERP.Mvc.DTOs.Responses;
using MiniERP.Mvc.Entities;
using MiniERP.Mvc.Exceptions;
using MiniERP.Mvc.Extensions;
using MiniERP.Mvc.Mappings;
using MiniERP.Mvc.Models;

namespace MiniERP.Mvc.Services;

public interface IDepartmentService
{
    Task<Result<PagedResult<DepartmentDto>>> ListDepartments(DepartmentQuery request);
    Task<Result<DepartmentDto>> GetDepartment(int id);
    Task<Result<DepartmentDto>> CreateDepartment(DepartmentRequest request);
    Task<Result> EditDepartment(int id, DepartmentRequest request);
    Task<Result> DeleteDepartment(int id);
}

public class DepartmentService(AppDbContext context) : IDepartmentService
{
    private readonly AppDbContext _context = context;

    public async Task<Result<PagedResult<DepartmentDto>>> ListDepartments(DepartmentQuery request)
    {
        var query = _context.Departments.AsNoTracking().AsQueryable();

        // Search name
        if (!string.IsNullOrEmpty(request.Search))
        {
            query = query.Where(x => x.Title.Contains(request.Search));
        }

        // Total Count
        var totalCount = await query.CountAsync();

        // List Items
        var items = await query
            .Paginate(request.Page, request.PageSize)
            .Select(x => new DepartmentDto
            {
                Id = x.Id,
                Title = x.Title,
            })
            .ToListAsync();

        // Response Result
        var result = new PagedResult<DepartmentDto>()
        {
            Items = items,
            TotalCount = totalCount,
            Page = request.Page,
            PageSize = request.PageSize
        };

        return Result<PagedResult<DepartmentDto>>.Success(result);
    }

    public async Task<Result<DepartmentDto>> GetDepartment(int id)
    {
        var department = await _context.Departments
            .AsNoTracking()
            .FirstOrDefaultAsync(d => d.Id == id);

        return department is null
            ? Result<DepartmentDto>.Failure("Department not found", ErrorCode.NotFound)
            : Result<DepartmentDto>.Success(department.ToDepartmentDto());
    }

    public async Task<Result<DepartmentDto>> CreateDepartment(DepartmentRequest request)
    {
        var exists = await _context.Departments.AnyAsync(d => d.Title == request.Title);

        if (exists)
            return Result<DepartmentDto>.Failure("Title already exists", ErrorCode.Conflict);

        var department = request.ToEntity();

        _context.Departments.Add(department);
        await _context.SaveChangesAsync();
        
        return Result<DepartmentDto>.Success(department.ToDepartmentDto());
    }

    public async Task<Result> EditDepartment(int id, DepartmentRequest request)
    {
        var department = await _context.Departments.FirstOrDefaultAsync(d => d.Id == id);

        if (department is null)
            return Result.Failure("Department not found", ErrorCode.NotFound);

        var exists = await _context.Departments.AnyAsync(d =>
            d.Id != id &&
            d.Title == request.Title
        );

        if (exists)
            return Result.Failure("Title already exists", ErrorCode.Conflict);

        department.Title = request.Title;
        department.UpdatedAt = DateTime.UtcNow;
        await _context.SaveChangesAsync();
        
        return Result.Success();
    }

    public async Task<Result> DeleteDepartment(int id)
    {
        var department = await _context.Departments.FirstOrDefaultAsync(d => d.Id == id);

        if (department is null)
            return Result.Failure("Department not found", ErrorCode.NotFound);

        department.IsDeleted = true;
        department.UpdatedAt = DateTime.UtcNow;
        await _context.SaveChangesAsync();
        
        return Result.Success();
    }
}