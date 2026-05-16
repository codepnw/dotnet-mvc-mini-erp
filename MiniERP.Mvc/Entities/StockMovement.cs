namespace MiniERP.Mvc.Entities;

public enum StockMovementType
{
    In,
    Out,
    Adjust
}

public class StockMovement
{
    public int Id { get; set; }

    // FK
    public required int ProductId { get; set; }
    public Product? Product { get; set; }

    public int Quantity { get; set; }
    public StockMovementType MovementType { get; set; }
    public string? Remark { get; set; }

    public DateTime CreatedAt { get; set; }
}