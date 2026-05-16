using System;
using Microsoft.EntityFrameworkCore;
using MiniERP.Mvc.Entities;

namespace MiniERP.Mvc.Data;

public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
{
    public DbSet<User> Users => Set<User>();
    public DbSet<Department> Departments => Set<Department>();
    public DbSet<Employee> Employees => Set<Employee>();
    public DbSet<LeaveType> LeaveTypes => Set<LeaveType>();
    public DbSet<LeaveRequest> LeaveRequests => Set<LeaveRequest>();
    public DbSet<Category> Categories => Set<Category>();
    public DbSet<Product> Products => Set<Product>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        const string getUtcDate = "GETUTCDATE()";

        var user = modelBuilder.Entity<User>();
        var employee = modelBuilder.Entity<Employee>();
        var department = modelBuilder.Entity<Department>();
        var leaveType = modelBuilder.Entity<LeaveType>();
        var leaveReq = modelBuilder.Entity<LeaveRequest>();
        var category = modelBuilder.Entity<Category>();
        var product = modelBuilder.Entity<Product>();

        // ------------- User ----------------

        user.HasIndex(x => x.Email).IsUnique();

        user.HasQueryFilter(x => !x.IsDeleted);

        user.Property(x => x.Role).HasConversion<string>();

        user.Property(x => x.CreatedAt).HasDefaultValueSql(getUtcDate);

        user.Property(x => x.Email).HasMaxLength(100);
        user.Property(x => x.PasswordHash).HasMaxLength(255);
        user.Property(x => x.FirstName).HasMaxLength(100);
        user.Property(x => x.LastName).HasMaxLength(100);

        // ------------- Department ----------------

        department.HasIndex(x => x.Title).IsUnique();

        department.HasQueryFilter(x => !x.IsDeleted);

        department.Property(x => x.CreatedAt).HasDefaultValueSql(getUtcDate);

        // ------------- Employee ----------------

        employee.HasIndex(x => x.CitizenId).IsUnique();

        employee.HasQueryFilter(x => !x.IsDeleted);

        employee.Property(x => x.Salary).HasPrecision(10, 2);

        employee.Property(x => x.CreatedAt).HasDefaultValueSql(getUtcDate);

        // ------------- LeaveType ----------------

        leaveType.HasIndex(x => x.Title).IsUnique();

        leaveType.Property(x => x.CreatedAt).HasDefaultValueSql(getUtcDate);

        leaveType.HasData(
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

        leaveReq.Property(x => x.Status).HasConversion<string>();

        leaveReq.HasQueryFilter(x => !x.IsDeleted);

        leaveReq.Property(x => x.Reason).HasMaxLength(255);

        leaveReq.Property(x => x.CreatedAt).HasDefaultValueSql(getUtcDate);

        // ------------- Category ----------------
        
        category.HasIndex(x => x.Title).IsUnique();
        
        category.Property(x => x.Title).HasMaxLength(100);
        category.Property(x => x.Description).HasMaxLength(255);
        
        category.HasQueryFilter(x => !x.IsDeleted);
        
        category.Property(x => x.CreatedAt).HasDefaultValueSql(getUtcDate);

        // ------------- Product ----------------
        
        product.HasIndex(x => x.Name).IsUnique();
        product.HasIndex(x => x.Sku).IsUnique();
        
        product.Property(x => x.Price).HasPrecision(10, 2);
        
        product.Property(x => x.Name).HasMaxLength(100);
        product.Property(x => x.Sku).HasMaxLength(100);
        
        product.HasQueryFilter(x => !x.IsDeleted);
        
        product.Property(x => x.CreatedAt).HasDefaultValueSql(getUtcDate);
    }
}