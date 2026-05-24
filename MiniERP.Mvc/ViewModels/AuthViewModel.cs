namespace MiniERP.Mvc.Models;

public class AuthViewModel
{
    public required string AccessToken { get; set; }
    public required string RefreshToken { get; set; }
}