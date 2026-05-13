using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using MiniERP.Mvc.Common;
using MiniERP.Mvc.Data;
using MiniERP.Mvc.DTOs;
using MiniERP.Mvc.Entities;
using MiniERP.Mvc.Mappings;
using MiniERP.Mvc.Models;
using JwtRegisteredClaimNames = Microsoft.IdentityModel.JsonWebTokens.JwtRegisteredClaimNames;

namespace MiniERP.Mvc.Services;

public interface IUserService
{
    Task<Result<AuthViewModel>> CreateUser(UserRegisterDTO dto);
    Task<Result<AuthViewModel>> Login(UserLoginDTO dto);
}

public class UserService(AppDbContext context, IConfiguration config) : IUserService
{
    private readonly AppDbContext _context = context;
    
    // JWT Config
    private readonly string _secretKey = config["Jwt:SecretKey"]!;
    private readonly string _refreshKey = config["Jwt:RefreshSecretKey"]!;
    private readonly double _accessTokenExpireHours = double.Parse(config["Jwt:AccessExpireHours"]!);
    private readonly double _refreshTokenExpireHours = double.Parse(config["Jwt:RefreshExpireHours"]!);

    public async Task<Result<AuthViewModel>> CreateUser(UserRegisterDTO dto)
    {
        var normalizedEmail = EmailLowerCase(dto.Email);

        // Check Email
        var emailExists = await _context.Users.AnyAsync(x => x.Email == normalizedEmail);

        if (emailExists)
            return Result<AuthViewModel>.Failure("Email already exists", ErrorCode.BadRequest);

        // Check Matching Password
        if (dto.Password != dto.ConfirmPassword)
            return Result<AuthViewModel>.Failure("Password not match", ErrorCode.BadRequest);

        var newUser = dto.ToUserEntity("");

        // Hash Password
        var hasher = new PasswordHasher<User>();
        var hashedPassword = hasher.HashPassword(newUser, dto.Password);

        newUser.PasswordHash = hashedPassword;

        // Insert User to DB
        _context.Users.Add(newUser);
        var rowAffected = await _context.SaveChangesAsync();

        if (rowAffected == 0)
            return Result<AuthViewModel>.Failure("Insert user failed", ErrorCode.InternalServerError);

        // Generate Token
        var accessToken = GenerateJwtToken(
            _secretKey,
            newUser,
            _accessTokenExpireHours
        );
        var refreshToken = GenerateJwtToken(
            _refreshKey,
            newUser,
            _refreshTokenExpireHours
        );

        var response = new AuthViewModel
        {
            AccessToken = accessToken,
            RefreshToken = refreshToken,
        };

        return Result<AuthViewModel>.Success(response);
    }

    public async Task<Result<AuthViewModel>> Login(UserLoginDTO dto)
    {
        var normalizedEmail = EmailLowerCase(dto.Email);
        
        // Find User by Email
        var userData = await _context.Users
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.Email == normalizedEmail);

        if (userData is null)
            return Result<AuthViewModel>.Failure("Invalid email or password", ErrorCode.BadRequest);

        // Verify Password
        var hasher = new PasswordHasher<User>();
        var verifyPassword = hasher.VerifyHashedPassword(userData, userData.PasswordHash, dto.Password);

        if (verifyPassword != PasswordVerificationResult.Success)
            return Result<AuthViewModel>.Failure("Invalid email or password", ErrorCode.BadRequest);

        // Generate Token
        var accessToken = GenerateJwtToken(
            _secretKey,
            userData,
            _accessTokenExpireHours
        );
        var refreshToken = GenerateJwtToken(
            _refreshKey,
            userData,
            _refreshTokenExpireHours
        );

        var response = new AuthViewModel
        {
            AccessToken = accessToken,
            RefreshToken = refreshToken,
        };

        return Result<AuthViewModel>.Success(response);
    }

    private static string EmailLowerCase(string email)
    {
        return email.Trim().ToLower();
    }

    private static string GenerateJwtToken(string key, User u, double hoursDuration)
    {
        var secretKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key));
        var credentials = new SigningCredentials(secretKey, SecurityAlgorithms.HmacSha256);

        var claims = new[]
        {
            new Claim(ClaimTypes.NameIdentifier, u.Id.ToString()),
            new Claim(JwtRegisteredClaimNames.Sub, u.Email),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            new Claim(ClaimTypes.Role, u.Role.ToString()),
        };

        var token = new JwtSecurityToken(
            issuer: "MiniERP",
            audience: "MiniERP",
            claims: claims,
            expires: DateTime.UtcNow.AddHours(hoursDuration),
            signingCredentials: credentials
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}