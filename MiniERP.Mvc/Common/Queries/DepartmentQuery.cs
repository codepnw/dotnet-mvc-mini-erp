using MiniERP.Mvc.Entities;

namespace MiniERP.Mvc.Common.Queries;

public class DepartmentQuery : PaginationRequest
{
    public string? Search { get; set; }
}