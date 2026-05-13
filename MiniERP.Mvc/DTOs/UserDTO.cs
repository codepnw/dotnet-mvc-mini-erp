using System.ComponentModel.DataAnnotations;

namespace MiniERP.Mvc.DTOs;

public record UserRegisterDTO(
    [Required] string Email,
    [Required] string Password,
    [Required] string ConfirmPassword,
    [Required] string FirstName,
    [Required] string LastName
);

public record UserLoginDTO(
    [Required] string Email,
    [Required] string Password
);