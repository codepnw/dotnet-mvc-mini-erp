using Microsoft.EntityFrameworkCore;
using MiniERP.Mvc.Common;
using MiniERP.Mvc.Data;
using MiniERP.Mvc.Entities;
using MiniERP.Mvc.Models;

namespace MiniERP.Mvc.Services;

public interface IReportService
{
    Task<Result<ReportRevenueViewModel>> GetRevenueSummary();
    Task<Result<List<ReportProductTopSellViewModel>>> GetProductTopSell();
}

public class ReportService(AppDbContext context) : IReportService
{
    private readonly AppDbContext _context = context;

    public async Task<Result<ReportRevenueViewModel>> GetRevenueSummary()
    {
        var completedOrders = _context.Orders.Where(x => x.Status == OrderStatus.Completed);
        var totalRevenue = await completedOrders.SumAsync(x => x.TotalAmount);
        var totalOrders = await completedOrders.CountAsync();
        var avgOrderValue = totalOrders == 0
            ? 0
            : totalRevenue / totalOrders;

        return Result<ReportRevenueViewModel>.Success(new ReportRevenueViewModel
        {
            TotalRevenue = totalRevenue,
            TotalOrders = totalOrders,
            AverageOrderValue = avgOrderValue
        });
    }

    public async Task<Result<List<ReportProductTopSellViewModel>>> GetProductTopSell()
    {
        var products = await _context.OrderItems
            .GroupBy(x => new
            {
                x.ProductId,
                x.Product!.Name
            })
            .Select(x => new ReportProductTopSellViewModel
            {
                ProductId = x.Key.ProductId,
                ProductName = x.Key.Name,
                TotalQuantitySold = x.Sum(s => s.Quantity)
            })
            .OrderByDescending(x => x.TotalQuantitySold)
            .Take(5)
            .ToListAsync();
        
        return Result<List<ReportProductTopSellViewModel>>.Success(products);
    }
}