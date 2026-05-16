using MiniERP.Mvc.DTOs;
using MiniERP.Mvc.Entities;
using MiniERP.Mvc.Models;

namespace MiniERP.Mvc.Mappings;

public static class CategoryMapping
{
    public static Category ToEntity(this CategoryCreateDto dto) => new()
    {
        Title = dto.Title,
        Description = dto.Description ?? "",
    };

    public static CategoryViewModel ToViewModel(this Category category) => new()
    {
        Id = category.Id,
        Title = category.Title,
        Description = category.Description,
    };

    public static void ApplyUpdate(this CategoryUpdateDto dto, Category category)
    {
        category.Title = dto.Title ?? category.Title;
        category.Description = dto.Description ?? category.Description;
        category.UpdatedAt = DateTime.UtcNow;
    }
}