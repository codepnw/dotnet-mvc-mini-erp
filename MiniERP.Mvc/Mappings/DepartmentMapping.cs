using System;
using MiniERP.Mvc.DTOs.Requests;
using MiniERP.Mvc.DTOs.Responses;
using MiniERP.Mvc.Entities;
using MiniERP.Mvc.ViewModels;

namespace MiniERP.Mvc.Mappings;

public static class DepartmentMapping
{
    public static Department ToEntity(this DepartmentRequest request) => new()
    {
        Title = request.Title
    };

    public static DepartmentDto ToDto(this Department department) => new()
    {
        Id = department.Id,
        Title = department.Title
    };

    public static DepartmentRequest ToRequest(this DepartmentFormVm vm) => new()
    {
        Title = vm.Title,
    };

    public static DepartmentFormVm ToViewModel(this DepartmentDto dto) => new()
    {
        Title = dto.Title,
    };
}