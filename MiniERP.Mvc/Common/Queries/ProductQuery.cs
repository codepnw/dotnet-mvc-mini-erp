namespace MiniERP.Mvc.Common.Queries;

public class ProductQuery : PaginationRequest
{
    public string? Name { get; set; }
    public int? CategoryId { get; set; }
}