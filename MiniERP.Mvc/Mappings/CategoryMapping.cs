using MiniERP.Mvc.DTOs.Requests;
using MiniERP.Mvc.DTOs.Responses;
using MiniERP.Mvc.Entities;
using MiniERP.Mvc.ViewModels;

namespace MiniERP.Mvc.Mappings;

public static class CategoryMapping
{
    public static Category ToEntity(this CategoryCreateRequest request) => new()
    {
        Title = request.Title,
        Description = request.Description ?? "",
    };

    public static CategoryDto ToDto(this Category category) => new()
    {
        Id = category.Id,
        Title = category.Title,
        Description = category.Description!,
    };

    public static CategoryCreateRequest ToCreateRequest(this CategoryFormVm vm) => new()
    {
        Title = vm.Title,
        Description = vm.Description
    };

    public static CategoryUpdateRequest ToUpdateRequest(this CategoryFormVm vm) => new()
    {
        Title = vm.Title,
        Description = vm.Description
    };

    public static void ApplyUpdate(this CategoryUpdateRequest request, Category category)
    {
        category.Title = request.Title ?? category.Title;
        category.Description = request.Description ?? category.Description;
        category.UpdatedAt = DateTime.UtcNow;
    }

    public static CategoryFormVm ToViewModel(this CategoryDto dto) => new()
    {
        Title = dto.Title,
        Description = dto.Description
    };
}