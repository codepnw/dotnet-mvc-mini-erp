using System;
using System.Drawing;
using System.Runtime.CompilerServices;
using Microsoft.EntityFrameworkCore.Query.Internal;
using MiniERP.Mvc.DTOs;
using MiniERP.Mvc.Entities;
using MiniERP.Mvc.Exceptions;
using MiniERP.Mvc.Mappings;
using MiniERP.Mvc.Models;
using MiniERP.Mvc.Repositories;

namespace MiniERP.Mvc.Services;

public interface IDepartmentService
{
    Task<List<DepartmentViewModel>> GetDepartments();
    Task<DepartmentViewModel> GetDepartment(int id);
    Task<DepartmentViewModel> CreateDepartment(DepartmentDTO dto);
    Task<DepartmentViewModel> EditDepartment(int id, DepartmentDTO dto);
    Task<bool> DeleteDepartment(int id);
}

public class DepartmentService(IDepartmentRepository repository) : IDepartmentService
{
    private readonly IDepartmentRepository _repository = repository;

    public async Task<List<DepartmentViewModel>> GetDepartments()
    {
        var departments = await _repository.ListAsync();

        var response = departments.Select(d => d.ToViewModel()).ToList();

        return response;
    }

    public async Task<DepartmentViewModel> GetDepartment(int id)
    {
        var department = await _repository.FindByIdAsNoTrackingAsync(id)
            ?? throw new NotFoundException("department id not found");

        return department.ToViewModel();
    }

    public async Task<DepartmentViewModel> CreateDepartment(DepartmentDTO dto)
    {
        var department = dto.ToEntity();

        _repository.Add(department);
        await _repository.SaveChangesAsync();

        return department.ToViewModel();
    }

    public async Task<DepartmentViewModel> EditDepartment(int id, DepartmentDTO dto)
    {
        var department = await PrivateGetDepartment(id);

        if (department.Title == dto.Title)
            throw new BadRequestException("title no update");

        var exists = await _repository.CheckTitleAsync(dto.Title);

        if (exists)
            throw new BadRequestException("title already exists");

        department.Title = dto.Title;
        department.UpdatedAt = DateTime.UtcNow;

        await _repository.SaveChangesAsync();

        return department.ToViewModel();
    }

    public async Task<bool> DeleteDepartment(int id)
    {
        var exists = await _repository.CheckIdAsync(id);

        if (!exists)
            throw new NotFoundException("department id not found");

        var rowAffected = await _repository.Delete(id);

        if (rowAffected == 0)
            throw new Exception("delete department failed");

        return true;
    }

    private async Task<Department> PrivateGetDepartment(int id)
    {
        var department = await _repository.FindByIdAsync(id)
            ?? throw new NotFoundException("department id not found");
        return department;
    }
}
