using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;

namespace MiniERP.Mvc.DTOs.Requests;

public class DepartmentRequest
{
    [Required(ErrorMessage = "Title is required")]
    public required string Title { get; init; }
}