namespace MiniERP.Mvc.ViewModels;

public class AuthViewModel
{
    public required string AccessToken { get; set; }
    public required string RefreshToken { get; set; }
}