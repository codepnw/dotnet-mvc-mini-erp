using System;
using System.Drawing;
using System.Reflection.Metadata.Ecma335;
using System.Runtime.CompilerServices;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using Microsoft.EntityFrameworkCore.Query.Internal;
using MiniERP.Mvc.Common;
using MiniERP.Mvc.Data;
using MiniERP.Mvc.DTOs;
using MiniERP.Mvc.Entities;
using MiniERP.Mvc.Exceptions;
using MiniERP.Mvc.Mappings;
using MiniERP.Mvc.Models;
namespace MiniERP.Mvc.Services;

public interface IDepartmentService
{
    Task<Result<IReadOnlyList<DepartmentViewModel>>> GetDepartments();
    Task<Result<DepartmentViewModel>> GetDepartment(int id);
    Task<Result<DepartmentViewModel>> CreateDepartment(DepartmentDTO dto);
    Task<Result<DepartmentViewModel>> EditDepartment(int id, DepartmentDTO dto);
    Task<Result> DeleteDepartment(int id);
}

public class DepartmentService(AppDbContext context) : IDepartmentService
{
    private readonly AppDbContext _context = context;

    public async Task<Result<IReadOnlyList<DepartmentViewModel>>> GetDepartments()
    {
        var departments = await _context.Departments
            .AsNoTracking()
            .Select(d => new DepartmentViewModel
            {
                Id = d.Id,
                Title = d.Title
            })
            .ToListAsync();

        return Result<IReadOnlyList<DepartmentViewModel>>.Success(departments);
    }

    public async Task<Result<DepartmentViewModel>> GetDepartment(int id)
    {
        var department = await _context.Departments
            .AsNoTracking()
            .FirstOrDefaultAsync(d => d.Id == id);

        return department is null
            ? Result<DepartmentViewModel>.Failure("Department not found", ErrorCode.NotFound)
            : Result<DepartmentViewModel>.Success(department.ToViewModel());
    }

    public async Task<Result<DepartmentViewModel>> CreateDepartment(DepartmentDTO dto)
    {
        var exists = await _context.Departments.AnyAsync(d => d.Title == dto.Title);

        if (exists)
            return Result<DepartmentViewModel>.Failure("Title already exists", ErrorCode.Conflict);

        var department = dto.ToEntity();

        _context.Departments.Add(department);
        var rowAffected = await _context.SaveChangesAsync();

        return rowAffected == 0
            ? Result<DepartmentViewModel>.Failure("Insert department failed", ErrorCode.InternalServerError)
            : Result<DepartmentViewModel>.Success(department.ToViewModel());
    }

    public async Task<Result<DepartmentViewModel>> EditDepartment(int id, DepartmentDTO dto)
    {
        var department = await _context.Departments.FirstOrDefaultAsync(d => d.Id == id);

        if (department is null)
            return Result<DepartmentViewModel>.Failure("Department not found", ErrorCode.NotFound);

        var exists = await _context.Departments.AnyAsync(d =>
            d.Id != id &&
            d.Title == dto.Title
        );

        if (exists)
            return Result<DepartmentViewModel>.Failure("Title already exists", ErrorCode.Conflict);

        department.Title = dto.Title;
        department.UpdatedAt = DateTime.UtcNow;

        var rowAffected = await _context.SaveChangesAsync();

        return rowAffected == 0
            ? Result<DepartmentViewModel>.Failure("Edit department failed", ErrorCode.InternalServerError)
            : Result<DepartmentViewModel>.Success(department.ToViewModel());
    }

    public async Task<Result> DeleteDepartment(int id)
    {
        var department = await _context.Departments.FirstOrDefaultAsync(d => d.Id == id);

        if (department is null)
            return Result.Failure("Department not found", ErrorCode.NotFound);

        department.IsDeleted = true;
        department.UpdatedAt = DateTime.UtcNow;
        
        var rowAffected = await _context.SaveChangesAsync();

        return rowAffected == 0
            ? Result.Failure("Delete department failed", ErrorCode.InternalServerError)
            : Result.Success();
    }
}