using System.ComponentModel.DataAnnotations;
using MiniERP.Mvc.Entities;

namespace MiniERP.Mvc.DTOs;

public class StockMovementDto
{
    [Required] public required int ProductId { get; init; }

    [Required] [Range(1, int.MaxValue)] public required int Quantity { get; init; }

    public string? Remark { get; init; }
}