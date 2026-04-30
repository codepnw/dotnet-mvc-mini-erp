using System;
using Microsoft.EntityFrameworkCore;
using MiniERP.Mvc.Entities;

namespace MiniERP.Mvc.Data;

public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
{
    public DbSet<Department> Departments => Set<Department>();
    public DbSet<Employee> Employees => Set<Employee>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Department>()
            .Property(e => e.CreatedAt)
            .HasDefaultValueSql("GETUTCDATE()");

        modelBuilder.Entity<Employee>()
            .Property(e => e.CreatedAt)
            .HasDefaultValueSql("GETUTCDATE()");
    }
}
