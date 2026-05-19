using Microsoft.EntityFrameworkCore;
using MiniERP.Mvc.Common;
using MiniERP.Mvc.Data;
using MiniERP.Mvc.Entities;
using MiniERP.Mvc.Models;

namespace MiniERP.Mvc.Services;

public interface IDashboardService
{
    Task<Result<DashboardStatsViewModel>> GetStats();
}

public class DashboardService(AppDbContext context) : IDashboardService
{
    private readonly AppDbContext _context = context;

    public async Task<Result<DashboardStatsViewModel>> GetStats()
    {
        // Count Dashboard Stats
        var totalEmployees = await _context.Employees.CountAsync();
        var totalProducts = await _context.Products.CountAsync();
        var totalLowStock = await _context.Products.CountAsync(x => x.Stock <= x.MinimumStock);
        var totalOrders = await _context.Orders.CountAsync();
        var totalRevenue = await _context.Orders
            .Where(x => x.Status == OrderStatus.Completed)
            .SumAsync(x => x.TotalAmount);
        var totalPendingLeave = await _context.LeaveRequests.CountAsync(x => x.Status == LeaveStatus.Pending);

        return Result<DashboardStatsViewModel>.Success(new DashboardStatsViewModel
        {
            TotalEmployees = totalEmployees,
            TotalProducts = totalProducts,
            LowStockProducts = totalLowStock,
            TotalOrders = totalOrders,
            TotalRevenue = totalRevenue,
            PendingLeaves = totalPendingLeave,
        });
    }
}