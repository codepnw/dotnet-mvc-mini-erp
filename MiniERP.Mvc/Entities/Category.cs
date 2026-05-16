namespace MiniERP.Mvc.Entities;

public class Category
{
    public int Id { get; set; }
    public required string Title { get; set; }
    public string? Description { get; set; }
    
    public bool IsDeleted { get; set; } = false;
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }

    public ICollection<Product> Products { get; set; } = [];
}