using System;
using Microsoft.EntityFrameworkCore;
using MiniERP.Mvc.Data;
using MiniERP.Mvc.Entities;

namespace MiniERP.Mvc.Repositories;

public interface IDepartmentRepository
{
    Task<Department?> FindByIdAsNoTrackingAsync(int id);
    Task<Department?> FindByIdAsync(int id);
    Task<List<Department>> ListAsync();
    Task<bool> CheckTitleAsync(string title);
    Task<bool> CheckIdAsync(int id);
    void Add(Department d);
    void Update(Department d);
    Task<int> Delete(int id);
    Task<int> SaveChangesAsync();
}

public class DepartmentRepository(AppDbContext context) : IDepartmentRepository
{
    private readonly AppDbContext _context = context;

    public Task<Department?> FindByIdAsNoTrackingAsync(int id) => _context.Departments
        .AsNoTracking()
        .FirstOrDefaultAsync(d => d.Id == id);

    public Task<Department?> FindByIdAsync(int id) => _context.Departments
        .FirstOrDefaultAsync(d => d.Id == id);

    public Task<List<Department>> ListAsync() => _context.Departments
        .AsNoTracking()
        .ToListAsync();

    public Task<bool> CheckTitleAsync(string title) => _context.Departments
        .AnyAsync(d => d.Title == title);

    public Task<bool> CheckIdAsync(int id) => _context.Departments
        .AnyAsync(d => d.Id == id);

    public void Add(Department d) => _context.Departments.Add(d);

    public void Update(Department d) => _context.Departments.Add(d);

    public async Task<int> Delete(int id) => _context.Departments
        .Where(e => e.Id == id)
        .ExecuteDelete();

    public Task<int> SaveChangesAsync() => _context.SaveChangesAsync();
}
