using System;
using System.Drawing;
using Microsoft.EntityFrameworkCore;
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
    Task<EmployeeViewModel> CreateEmployee(EmployeeCreateDTO dto);
    Task<List<EmployeeViewModel>> ListEmployees();
    Task<EmployeeViewModel> GetEmployeeById(int id);
    Task<EmployeeViewModel> UpdateEmployee(int id, EmployeeUpdateDTO dto);
    Task<bool> DeleteEmployee(int id);
}

public class EmployeeService(IEmployeeRepository repository) : IEmployeeService
{
    private readonly IEmployeeRepository _repository = repository;

    public async Task<EmployeeViewModel> CreateEmployee(EmployeeCreateDTO dto)
    {
        var emp = dto.ToCreateEntity();

        _repository.Add(emp);
        await _repository.SaveChangesAsync();

        return emp.ToViewModel();
    }

    public async Task<List<EmployeeViewModel>> ListEmployees()
    {
        var employees = await _repository.ListAsync();

        var response = employees.Select(e => e.ToViewModel()).ToList();

        return response;
    }

    public async Task<EmployeeViewModel> GetEmployeeById(int id)
    {
        return await PrivateGetEmployeeDetails(id);
    }

    public async Task<EmployeeViewModel> UpdateEmployee(int id, EmployeeUpdateDTO dto)
    {
        var emp = await PrivateGetEmployeeId(id);

        dto.ToUpdateEntity(emp);

        _repository.Update(emp);
        await _repository.SaveChangesAsync();

        return emp.ToViewModel();
    }

    public async Task<bool> DeleteEmployee(int id)
    {
        var rowAffected = await _repository.Delete(id);

        if (rowAffected == 0) throw new NotFoundException("employee id not found");

        return true;
    }

    private async Task<Employee> PrivateGetEmployeeId(int id)
    {
        var emp = await _repository.FindByIdAsync(id)
            ?? throw new NotFoundException("employee id not found");

        return emp;
    }

    private async Task<EmployeeViewModel> PrivateGetEmployeeDetails(int id)
    {
        var emp = await PrivateGetEmployeeId(id);

        return emp.ToViewModel();
    }
}
