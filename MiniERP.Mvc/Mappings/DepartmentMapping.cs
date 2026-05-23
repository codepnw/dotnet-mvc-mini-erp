using System;
using MiniERP.Mvc.DTOs.Requests;
using MiniERP.Mvc.DTOs.Responses;
using MiniERP.Mvc.Entities;
using MiniERP.Mvc.Models;

namespace MiniERP.Mvc.Mappings;

public static class DepartmentMapping
{
    public static Department ToEntity(this DepartmentRequest request) => new()
    {
        Title = request.Title
    };

    public static DepartmentDto ToDepartmentDto(this Department department) => new()
    {
        Id = department.Id,
        Title = department.Title
    };
}
