using System.ComponentModel.DataAnnotations;

namespace MiniERP.Mvc.DTOs;

public class CategoryCreateDto
{
    [Required(ErrorMessage = "Category title is required")]
    [StringLength(100)]
    public required string Title { get; init; }

    [StringLength(255)] public string? Description { get; init; }
}

public class CategoryUpdateDto
{
    [StringLength(100)] public string? Title { get; init; }
    
    [StringLength(255)] public string? Description { get; init; }
}