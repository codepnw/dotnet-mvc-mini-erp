using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;

namespace MiniERP.Mvc.DTOs;

public record class DepartmentDTO(
    [Required(ErrorMessage = "title is required"), StringLength(30)] string Title
);
