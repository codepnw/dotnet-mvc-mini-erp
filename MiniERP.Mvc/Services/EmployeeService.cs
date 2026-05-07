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
    Task<Result<IReadOnlyList<EmployeeViewModel>>> ListEmployees();
    Task<Result<EmployeeViewModel>> GetEmployeeById(int id);
    Task<Result<EmployeeViewModel>> UpdateEmployee(int id, EmployeeUpdateDTO dto);
    Task<Result> DeleteEmployee(int id);
}

public class EmployeeService(AppDbContext context) : IEmployeeService
{
    private readonly AppDbContext _context = context;

    public async Task<Result<EmployeeViewModel>> CreateEmployee(EmployeeCreateDTO dto)
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

    public async Task<Result<IReadOnlyList<EmployeeViewModel>>> ListEmployees()
    {
        var listData = await _context.Employees
            .AsNoTracking()
            .Select(x => new EmployeeViewModel
            {
                FirstName = x.FirstName,
                LastName = x.LastName,
                SalaryText = x.Salary.ToString("N2"),
                DepartmentTitle = x.Department!.Title,
            })
            .ToListAsync();

        return Result<IReadOnlyList<EmployeeViewModel>>.Success(listData);
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

    public async Task<Result<EmployeeViewModel>> UpdateEmployee(int id, EmployeeUpdateDTO dto)
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