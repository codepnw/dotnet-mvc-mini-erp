using MiniERP.Mvc.Entities;

namespace MiniERP.Mvc.Common.Queries;

public class LeaveRequestQuery : PaginationRequest
{
    public string? Search { get; set; }
    public LeaveStatus? Status { get; set; }
    public DateTime? FromDate { get; set; }
    public DateTime? ToDate { get; set; }
}