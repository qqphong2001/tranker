using ExpenseManagement.Application.Authentication.DTOs;
using ExpenseManagement.Application.Common.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ExpenseManagement.WebAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly IIdentityService _identityService;

    public AuthController(IIdentityService identityService)
    {
        _identityService = identityService;
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterDto model)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        if (model.Password != model.ConfirmPassword)
            return BadRequest(new { message = "Passwords do not match" });

        var result = await _identityService.RegisterAsync(model.Email, model.Password, model.FullName);

        if (!result.Succeeded)
            return BadRequest(new { message = string.Join(", ", result.Errors) });

        return Ok(result.Data);
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginDto model)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var result = await _identityService.LoginAsync(model.Email, model.Password);

        if (!result.Succeeded)
            return Unauthorized(new { message = string.Join(", ", result.Errors) });

        return Ok(result.Data);
    }

    [HttpPost("refresh-token")]
    public async Task<IActionResult> RefreshToken([FromBody] RefreshTokenDto model)
    {
        if (string.IsNullOrWhiteSpace(model.RefreshToken))
            return BadRequest(new { message = "Refresh token is required" });

        var result = await _identityService.RefreshTokenAsync(model.RefreshToken);

        if (!result.Succeeded)
            return Unauthorized(new { message = string.Join(", ", result.Errors) });

        return Ok(result.Data);
    }

    [Authorize]
    [HttpPost("logout")]
    public async Task<IActionResult> Logout()
    {
        var userId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;

        if (string.IsNullOrEmpty(userId))
            return Unauthorized();

        var result = await _identityService.RevokeTokenAsync(userId);

        if (!result.Succeeded)
            return BadRequest(new { message = string.Join(", ", result.Errors) });

        return Ok(new { message = "Logged out successfully" });
    }
}
