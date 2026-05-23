namespace MiniERP.Mvc.DTOs.Responses;

public class CategoryDto
{
    public int Id { get; set; }
    public required string Title { get; set; }
    public string? Description { get; set; }
}