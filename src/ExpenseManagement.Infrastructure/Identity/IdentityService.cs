using ExpenseManagement.Application.Authentication.DTOs;
using ExpenseManagement.Application.Common.Interfaces;
using ExpenseManagement.Application.Common.Models;
using Microsoft.AspNetCore.Identity;

namespace ExpenseManagement.Infrastructure.Identity;

public class IdentityService : IIdentityService
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly JwtTokenService _jwtTokenService;
    private readonly IDateTime _dateTime;

    public IdentityService(
        UserManager<ApplicationUser> userManager,
        JwtTokenService jwtTokenService,
        IDateTime dateTime)
    {
        _userManager = userManager;
        _jwtTokenService = jwtTokenService;
        _dateTime = dateTime;
    }

    public async Task<Result<AuthResponse>> RegisterAsync(string email, string password, string fullName, CancellationToken cancellationToken = default)
    {
        var existingUser = await _userManager.FindByEmailAsync(email);
        if (existingUser != null)
            return Result<AuthResponse>.Failure("Email is already registered");

        var user = new ApplicationUser
        {
            UserName = email,
            Email = email,
            FullName = fullName,
            CreatedAt = _dateTime.UtcNow
        };

        var result = await _userManager.CreateAsync(user, password);

        if (!result.Succeeded)
            return Result<AuthResponse>.Failure(result.Errors.Select(e => e.Description).ToArray());

        // Add default role
        await _userManager.AddToRoleAsync(user, "User");

        var authResponse = await _jwtTokenService.GenerateTokenAsync(user);
        return Result<AuthResponse>.Success(authResponse);
    }

    public async Task<Result<AuthResponse>> LoginAsync(string email, string password, CancellationToken cancellationToken = default)
    {
        var user = await _userManager.FindByEmailAsync(email);
        if (user == null)
            return Result<AuthResponse>.Failure("Invalid credentials");

        var isValidPassword = await _userManager.CheckPasswordAsync(user, password);
        if (!isValidPassword)
            return Result<AuthResponse>.Failure("Invalid credentials");

        var authResponse = await _jwtTokenService.GenerateTokenAsync(user);
        return Result<AuthResponse>.Success(authResponse);
    }

    public async Task<Result<AuthResponse>> RefreshTokenAsync(string refreshToken, CancellationToken cancellationToken = default)
    {
        var user = _userManager.Users.FirstOrDefault(u => u.RefreshToken == refreshToken);

        if (user == null || user.RefreshTokenExpiryTime <= _dateTime.UtcNow)
            return Result<AuthResponse>.Failure("Invalid or expired refresh token");

        var authResponse = await _jwtTokenService.GenerateTokenAsync(user);
        return Result<AuthResponse>.Success(authResponse);
    }

    public async Task<Result> RevokeTokenAsync(string userId, CancellationToken cancellationToken = default)
    {
        var user = await _userManager.FindByIdAsync(userId);
        if (user == null)
            return Result.Failure("User not found");

        user.RefreshToken = null;
        user.RefreshTokenExpiryTime = null;
        await _userManager.UpdateAsync(user);

        return Result.Success();
    }
}
