using System;
using Microsoft.EntityFrameworkCore;
using MiniERP.Mvc.Data;
using MiniERP.Mvc.Entities;

namespace MiniERP.Mvc.Repositories;

public interface IEmployeeRepository
{
    Task<Employee?> FindByIdAsync(int id);
    Task<List<Employee>> ListAsync();

    void Add(Employee e);
    void Update(Employee e);
    Task<int> Delete(int id);
    Task<int> SaveChangesAsync();
}

public class EmployeeRepository(AppDbContext context) : IEmployeeRepository
{
    private readonly AppDbContext _context = context;

    public Task<Employee?> FindByIdAsync(int id) => _context.Employees
        .AsNoTracking()
        .Include(e => e.Department)
        .FirstOrDefaultAsync(e => e.Id == id);

    public Task<List<Employee>> ListAsync() => _context.Employees
        .AsNoTracking()
        .Include(e => e.Department)
        .ToListAsync();

    public void Add(Employee e) => _context.Employees.Add(e);

    public void Update(Employee e) => _context.Employees.Update(e);

    public async Task<int> Delete(int id) => _context.Employees
        .Where(e => e.Id == id)
        .ExecuteDelete();

    public Task<int> SaveChangesAsync() => _context.SaveChangesAsync();
}
