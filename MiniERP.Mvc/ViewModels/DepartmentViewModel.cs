using System;
using System.ComponentModel.DataAnnotations;
using MiniERP.Mvc.Common;
using MiniERP.Mvc.Common.Queries;
using MiniERP.Mvc.DTOs.Responses;

namespace MiniERP.Mvc.ViewModels;

public class DepartmentIndexVm
{
    public DepartmentQuery Query { get; set; } = new();
    public PagedResult<DepartmentDto> Departments { get; set; } = new();
    public string? ErrorMessage { get; set; }
}

public class DepartmentDetailsVm
{
    public DepartmentDto Department { get; set; } = new();
    public string? ErrorMessage { get; set; }
}

public class DepartmentFormVm
{
    [Required(ErrorMessage = "Title is required"), StringLength(50, MinimumLength = 3)]
    public string Title { get; set; } = string.Empty;

    public string? ErrorMessage { get; set; }
}