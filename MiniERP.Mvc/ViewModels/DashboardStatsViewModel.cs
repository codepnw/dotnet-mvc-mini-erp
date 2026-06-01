namespace MiniERP.Mvc.ViewModels;

public class DashboardStatsVm
{
    public int TotalEmployees { get; set; }
    public int TotalProducts { get; set; }
    public int LowStockProducts { get; set; }
    public int TotalOrders { get; set; }
    public decimal TotalRevenue { get; set; }
    public int PendingLeaves { get; set; }
}