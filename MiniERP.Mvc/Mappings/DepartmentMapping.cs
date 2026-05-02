using System;
using MiniERP.Mvc.DTOs;
using MiniERP.Mvc.Entities;
using MiniERP.Mvc.Models;

namespace MiniERP.Mvc.Mappings;

public static class DepartmentMapping
{
    public static Department ToEntity(this DepartmentDTO dto) => new()
    {
        Title = dto.Title
    };

    public static DepartmentViewModel ToViewModel(this Department department) => new()
    {
        Id = department.Id,
        Title = department.Title
    };
}
