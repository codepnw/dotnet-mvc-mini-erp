using System.ComponentModel.DataAnnotations;
using Microsoft.Identity.Client;
using Microsoft.Net.Http.Headers;

namespace MiniERP.Mvc.ViewModels;

// public class UserViewModel
// {
//     public int Id { get; set; }
//     public required string Email { get; set; }
//     public required string FirstName { get; set; }
//     public required string LastName { get; set; }
//     public required string Role { get; set; }
// }

public class UserLoginVm
{
    [Required(ErrorMessage = "Email is required")]
    [EmailAddress(ErrorMessage = "Invalid Email Address")]
    public string Email { get; set; } = "";

    [Required(ErrorMessage = "Password is required")]
    public string Password { get; set; } = "";
}