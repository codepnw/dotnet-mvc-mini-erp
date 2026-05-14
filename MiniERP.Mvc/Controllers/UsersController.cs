using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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

    [HttpGet]
    public IActionResult Create()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Create(UserCreateDto dto)
    {
        if (!ModelState.IsValid) return View(dto);

        var result = await _service.CreateUser(dto);

        if (!result.IsFailure) return RedirectToAction("Index");
        
        ModelState.AddModelError("", result.ErrorMessage!);
        return View(dto);
    }
    
    [HttpGet]
    public IActionResult Login()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Login(UserLoginDto dto)
    {
        if (!ModelState.IsValid) return View(dto);
        
        var result = await _service.Login(dto);

        // TODO: return view
        if (!result.IsFailure) return Json(result.Data);
            
        ModelState.AddModelError("", result.ErrorMessage!);
        return View(dto);
    }
}