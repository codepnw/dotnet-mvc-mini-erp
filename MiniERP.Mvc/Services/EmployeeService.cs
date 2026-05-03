using System;
using System.Drawing;
using Microsoft.EntityFrameworkCore;
using MiniERP.Mvc.Common;
using MiniERP.Mvc.Data;
using MiniERP.Mvc.DTOs;
using MiniERP.Mvc.Entities;
using MiniERP.Mvc.Exceptions;
using MiniERP.Mvc.Mappings;
using MiniERP.Mvc.Models;
using MiniERP.Mvc.Repositories;

namespace MiniERP.Mvc.Services;

public interface IEmployeeService
{
    Task<Result<EmployeeViewModel>> CreateEmployee(EmployeeCreateDTO dto);
    Task<Result<List<EmployeeViewModel>>> ListEmployees();
    Task<Result<EmployeeViewModel>> GetEmployeeById(int id);
    Task<Result<EmployeeViewModel>> UpdateEmployee(int id, EmployeeUpdateDTO dto);
    Task<Result> DeleteEmployee(int id);
}

public class EmployeeService(IEmployeeRepository repository) : IEmployeeService
{
    private readonly IEmployeeRepository _repository = repository;

    public async Task<Result<EmployeeViewModel>> CreateEmployee(EmployeeCreateDTO dto)
    {
        var employee = dto.ToCreateEntity();

        _repository.Add(employee);
        var rowAffected = await _repository.SaveChangesAsync();

        if (rowAffected == 0)
            return Result<EmployeeViewModel>.Failure("db insert employee failed", ErrorCode.InternalServerError);

        return Result<EmployeeViewModel>.Success(employee.ToViewModel());
    }

    public async Task<Result<List<EmployeeViewModel>>> ListEmployees()
    {
        var employees = await _repository.ListAsync();

        var response = employees.Select(e => e.ToViewModel()).ToList();

        return Result<List<EmployeeViewModel>>.Success(response);
    }

    public async Task<Result<EmployeeViewModel>> GetEmployeeById(int id)
    {
        var employee = await _repository.FindByIdNoTrackingAsync(id);

        if (employee is null)
            return Result<EmployeeViewModel>.Failure("employee not found", ErrorCode.NotFound);

        return Result<EmployeeViewModel>.Success(employee.ToViewModel());
    }

    public async Task<Result<EmployeeViewModel>> UpdateEmployee(int id, EmployeeUpdateDTO dto)
    {
        var employee = await _repository.FindByIdAsync(id);

        if (employee is null)
            return Result<EmployeeViewModel>.Failure("employee not found", ErrorCode.NotFound);

        dto.ToUpdateEntity(employee);

        var rowAffected = await _repository.SaveChangesAsync();

        if (rowAffected == 0)
            return Result<EmployeeViewModel>.Failure("db update employee failed", ErrorCode.InternalServerError);

        return Result<EmployeeViewModel>.Success(employee.ToViewModel());
    }

    public async Task<Result> DeleteEmployee(int id)
    {
        var exists = await _repository.CheckIdAsync(id);

        if (!exists)
            return Result.Failure("employee not found", ErrorCode.NotFound);

        var rowAffected = await _repository.Delete(id);

        if (rowAffected == 0)
            return Result.Failure("db delete employee failed", ErrorCode.InternalServerError);

        return Result.Success();
    }
}
