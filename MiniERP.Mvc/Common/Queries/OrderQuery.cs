using MiniERP.Mvc.Entities;

namespace MiniERP.Mvc.Common.Queries;

public class OrderQuery : PaginationRequest
{
    public string? Search { get; set; }
    public OrderStatus? Status { get; set; }
    public DateTime? FromDate { get; set; }
    public DateTime? ToDate { get; set; }
}