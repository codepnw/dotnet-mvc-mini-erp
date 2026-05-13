using Microsoft.AspNetCore.Mvc;
using MiniERP.Mvc.DTOs;
using MiniERP.Mvc.Services;

namespace MiniERP.Mvc.Controllers;

public class UsersController(IUserService service) : Controller
{
    private readonly IUserService _service = service;

    // GET
    public IActionResult Index()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Register([FromBody] UserRegisterDTO dto)
    {
        var result = await _service.CreateUser(dto);

        return result.IsFailure
            ? Json(new { message = result.ErrorMessage })
            : Json(result.Data);
    }

    [HttpPost]
    public async Task<IActionResult> Login([FromBody] UserLoginDTO dto)
    {
        var result = await _service.Login(dto);

        return result.IsFailure
            ? Json(new { message = result.ErrorMessage })
            : Json(result.Data);
    }
}