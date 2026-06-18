namespace MiniERP.Mvc.Common.Queries;

public class ProductQuery : PaginationRequest
{
    public string? Search { get; set; }
}