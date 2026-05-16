namespace MiniERP.Mvc.Models;

public class CategoryViewModel
{
    public int Id { get; set; }
    public required string Title { get; set; }
    public string? Description { get; set; }
}