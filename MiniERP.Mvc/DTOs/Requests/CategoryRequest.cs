using System.ComponentModel.DataAnnotations;

namespace MiniERP.Mvc.DTOs.Requests;

public class CategoryCreateRequest
{
    [Required(ErrorMessage = "Category title is required")]
    [StringLength(100)]
    public required string Title { get; init; }

    [StringLength(255)] public string? Description { get; init; }
}

public class CategoryUpdateRequest
{
    [StringLength(100)] public string? Title { get; init; }
    
    [StringLength(255)] public string? Description { get; init; }
}