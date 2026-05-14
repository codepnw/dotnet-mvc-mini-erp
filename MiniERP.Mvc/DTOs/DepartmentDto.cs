using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;

namespace MiniERP.Mvc.DTOs;

public class DepartmentDto
{
    [Required(ErrorMessage = "Title is required")]
    public required string Title { get; init; }
}