using System.ComponentModel.DataAnnotations;

namespace MiniERP.Mvc.DTOs;

public class UserCreateDto
{
    [Required(ErrorMessage = "Email is required")]
    [EmailAddress(ErrorMessage = "Invalid Email Address")]
    public required string Email { get; init; }

    [Required(ErrorMessage = "Password is required")]
    public required string Password { get; init; }

    [Compare("Password", ErrorMessage = "Password and confirm password do not match")]
    public required string ConfirmPassword { get; init; }

    [Required(ErrorMessage = "First Name is required")]
    public required string FirstName { get; init; }

    [Required(ErrorMessage = "Last Name is required")]
    public required string LastName { get; init; }
}

public record UserLoginDto
{
    [Required(ErrorMessage = "Email is required")]
    [EmailAddress(ErrorMessage = "Invalid Email Address")]
    public required string Email { get; init; }

    [Required(ErrorMessage = "Password is required")]
    public required string Password { get; init; }
}