using MiniERP.Mvc.DTOs.Requests;
using MiniERP.Mvc.DTOs.Responses;
using MiniERP.Mvc.Entities;
using MiniERP.Mvc.Models;

namespace MiniERP.Mvc.Mappings;

public static class CategoryMapping
{
    public static Category ToEntity(this CategoryCreateRequest request) => new()
    {
        Title = request.Title,
        Description = request.Description ?? "",
    };

    public static CategoryDto ToCategoryDto(this Category category) => new()
    {
        Id = category.Id,
        Title = category.Title,
        Description = category.Description,
    };

    public static void ApplyUpdate(this CategoryUpdateRequest request, Category category)
    {
        category.Title = request.Title ?? category.Title;
        category.Description = request.Description ?? category.Description;
        category.UpdatedAt = DateTime.UtcNow;
    }
}