using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MiniERP.Mvc.DTOs.Requests;
using MiniERP.Mvc.Services;
using MiniERP.Mvc.ViewModels;

namespace MiniERP.Mvc.Controllers;

public class UsersController(IUserService service) : Controller
{
    private readonly IUserService _service = service;

    // GET
    public IActionResult Index()
    {
        return View();
    }

    [HttpGet]
    public IActionResult Create()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Create(UserCreateRequest request)
    {
        if (!ModelState.IsValid) return View(request);

        var result = await _service.CreateUser(request);

        if (!result.IsFailure) return RedirectToAction("Index");

        ModelState.AddModelError("", result.ErrorMessage!);
        return View(request);
    }

    [HttpGet]
    public IActionResult Login()
    {
        return View(new UserLoginVm());
    }

    [AllowAnonymous]
    [HttpPost]
    public async Task<IActionResult> Login(UserLoginVm vm)
    {
        if (!ModelState.IsValid) return View(vm);

        var result = await _service.Login(new UserLoginRequest
        {
            Email = vm.Email,
            Password = vm.Password
        });

        if (result.IsFailure)
        {
            ModelState.AddModelError("", result.ErrorMessage!);
            return View(vm);
        }

        var data = result.Data;

        if (data is null)
        {
            ModelState.AddModelError("", "Login data is null");
            return View(vm);
        }

        // Save JWT Token
        Response.Cookies.Append("access_token", data.AccessToken);
        Response.Cookies.Append("refresh_token", data.RefreshToken);

        // Create Cookies Authentication
        var claims = new List<Claim> { new(ClaimTypes.Name, vm.Email) };
        var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
        var principal = new ClaimsPrincipal(identity);

        await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);

        return RedirectToAction("Index", "Dashboard");
    }

    [Authorize]
    [HttpPost]
    public async Task<IActionResult> Logout()
    {
        Response.Cookies.Delete("access_token");
        Response.Cookies.Delete("refresh_token");

        await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

        return RedirectToAction("Login");
    }
}