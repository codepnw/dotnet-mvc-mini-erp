namespace MiniERP.Mvc.Common.Queries;

public class EmployeeQuery : PaginationRequest
{
    public string? Search { get; set; }
    public int? DepartmentId { get; set; }
}