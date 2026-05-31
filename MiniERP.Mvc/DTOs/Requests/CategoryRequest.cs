using System.ComponentModel.DataAnnotations;

namespace MiniERP.Mvc.DTOs.Requests;

public class CategoryCreateRequest
{
    public string Title { get; init; } = "";
    public string? Description { get; init; }
}

public class CategoryUpdateRequest
{
    public string? Title { get; init; }
    public string? Description { get; init; }
}