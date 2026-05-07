using System;
using Microsoft.EntityFrameworkCore;
using MiniERP.Mvc.Entities;

namespace MiniERP.Mvc.Data;

public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
{
    public DbSet<Department> Departments => Set<Department>();
    public DbSet<Employee> Employees => Set<Employee>();
    public DbSet<LeaveType> LeaveTypes => Set<LeaveType>();
    public DbSet<LeaveRequest> LeaveRequests => Set<LeaveRequest>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        const string getUtcDate = "GETUTCDATE()";
        var employee = modelBuilder.Entity<Employee>();
        var department = modelBuilder.Entity<Department>();
        var leaveType = modelBuilder.Entity<LeaveType>();
        var leaveReq = modelBuilder.Entity<LeaveRequest>();
    
        // ------------- Department ----------------
        department
            .HasIndex(x => x.Title)
            .IsUnique();

        department
            .Property(x => x.CreatedAt)
            .HasDefaultValueSql(getUtcDate);

        // ------------- Employee ----------------
        employee
            .HasIndex(x => x.CitizenId)
            .IsUnique();
        
        employee
            .Property(x => x.Salary)
            .HasPrecision(10, 2);

        employee
            .Property(x => x.CreatedAt)
            .HasDefaultValueSql(getUtcDate);

        // ------------- LeaveType ----------------
        leaveType
            .HasIndex(x => x.Title)
            .IsUnique();
        
        leaveType
            .Property(x => x.CreatedAt)
            .HasDefaultValueSql(getUtcDate);

        leaveType
            .HasData(
                new LeaveType
                {
                    Id = 1,
                    Title = "Sick Leave",
                    IsDeleted = false
                },
                new LeaveType
                {
                    Id = 2,
                    Title = "Vacation Leave",
                    IsDeleted = false
                },
                new LeaveType
                {
                    Id = 3,
                    Title = "Personal Leave",
                    IsDeleted = false
                }
            );

        // ------------- LeaveRequest ----------------
        leaveReq
            .Property(x => x.Status)
            .HasConversion<string>();
        
        leaveReq
            .Property(x => x.Reason)
            .HasMaxLength(255);
        
        leaveReq
            .Property(x => x.CreatedAt)
            .HasDefaultValueSql(getUtcDate);
    }
}