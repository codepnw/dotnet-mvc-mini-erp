using System.ComponentModel.DataAnnotations;
using MiniERP.Mvc.Common;
using MiniERP.Mvc.Common.Queries;
using MiniERP.Mvc.DTOs.Responses;

namespace MiniERP.Mvc.ViewModels;

public class CategoryIndexVm
{
    public CategoryQuery Query { get; set; } = new();
    public PagedResult<CategoryDto> Categories { get; set; } = new();
    public string? ErrorMessage { get; set; }
}

public class CategoryDetailsVm
{
    public CategoryDto Category { get; set; } = new();
    public string? ErrorMessage { get; set; }
}

public class CategoryFormVm
{
    [Required(ErrorMessage = "Title is required"), StringLength(50, MinimumLength = 3)]
    public string Title { get; set; } = "";

    public string? Description { get; set; }
    public string? ErrorMessage { get; set; }
}
