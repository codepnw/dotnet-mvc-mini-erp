namespace MiniERP.Mvc.Entities;

public class Product
{
    public int Id { get; set; }
    public required string Name { get; set; }
    public required string Sku { get; set; }
    public required decimal Price { get; set; }
    public int Stock { get; set; }

    public bool IsDeleted { get; set; } = false;
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }

    // FK
    public required int CategoryId { get; set; }
    public Category? Category { get; set; }
}