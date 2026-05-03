using System;
using System.Drawing;
using System.Reflection.Metadata.Ecma335;
using System.Runtime.CompilerServices;
using Microsoft.EntityFrameworkCore.Query;
using Microsoft.EntityFrameworkCore.Query.Internal;
using MiniERP.Mvc.Common;
using MiniERP.Mvc.DTOs;
using MiniERP.Mvc.Entities;
using MiniERP.Mvc.Exceptions;
using MiniERP.Mvc.Mappings;
using MiniERP.Mvc.Models;
using MiniERP.Mvc.Repositories;

namespace MiniERP.Mvc.Services;

public interface IDepartmentService
{
    Task<Result<List<DepartmentViewModel>>> GetDepartments();
    Task<Result<DepartmentViewModel>> GetDepartment(int id);
    Task<Result<DepartmentViewModel>> CreateDepartment(DepartmentDTO dto);
    Task<Result<DepartmentViewModel>> EditDepartment(int id, DepartmentDTO dto);
    Task<Result> DeleteDepartment(int id);
}

public class DepartmentService(IDepartmentRepository repository) : IDepartmentService
{
    private readonly IDepartmentRepository _repository = repository;

    public async Task<Result<List<DepartmentViewModel>>> GetDepartments()
    {
        var departments = await _repository.ListAsync();

        var response = departments.Select(d => d.ToViewModel()).ToList();

        return Result<List<DepartmentViewModel>>.Success(response);
    }

    public async Task<Result<DepartmentViewModel>> GetDepartment(int id)
    {
        var department = await _repository.FindByIdAsNoTrackingAsync(id);

        if (department is null)
            return Result<DepartmentViewModel>.Failure("department not found", ErrorCode.NotFound);

        return Result<DepartmentViewModel>.Success(department.ToViewModel());
    }

    public async Task<Result<DepartmentViewModel>> CreateDepartment(DepartmentDTO dto)
    {
        var exists = await _repository.CheckTitleAsync(dto.Title);

        if (exists)
            return Result<DepartmentViewModel>.Failure("title already exists", ErrorCode.Conflict);

        var department = dto.ToEntity();

        _repository.Add(department);
        var rowAffected = await _repository.SaveChangesAsync();

        if (rowAffected == 0)
            return Result<DepartmentViewModel>.Failure("db insert department failed", ErrorCode.InternalServerError);

        return Result<DepartmentViewModel>.Success(department.ToViewModel());
    }

    public async Task<Result<DepartmentViewModel>> EditDepartment(int id, DepartmentDTO dto)
    {
        var department = await _repository.FindByIdAsync(id);

        if (department is null)
            return Result<DepartmentViewModel>.Failure("department not found", ErrorCode.NotFound);

        var exists = await _repository.CheckTitleAsync(dto.Title);

        if (exists)
            return Result<DepartmentViewModel>.Failure("title already exists", ErrorCode.Conflict);

        department.Title = dto.Title;
        department.UpdatedAt = DateTime.UtcNow;

        var rowAffected = await _repository.SaveChangesAsync();

        if (rowAffected == 0)
            return Result<DepartmentViewModel>.Failure("db update department failed", ErrorCode.InternalServerError);

        return Result<DepartmentViewModel>.Success(department.ToViewModel());
    }

    public async Task<Result> DeleteDepartment(int id)
    {
        var exists = await _repository.CheckIdAsync(id);

        if (!exists)
            return Result.Failure("department id not found", ErrorCode.NotFound);

        var rowAffected = await _repository.Delete(id);

        if (rowAffected == 0)
            return Result.Failure("db delete department failed", ErrorCode.InternalServerError);

        return Result.Success();
    }
}
