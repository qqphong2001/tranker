using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using ExpenseManagement.Application.Authentication.DTOs;
using ExpenseManagement.Application.Common.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace ExpenseManagement.Infrastructure.Identity;

public class JwtTokenService
{
    private readonly IConfiguration _configuration;
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly IDateTime _dateTime;

    public JwtTokenService(
        IConfiguration configuration,
        UserManager<ApplicationUser> userManager,
        IDateTime dateTime)
    {
        _configuration = configuration;
        _userManager = userManager;
        _dateTime = dateTime;
    }

    public async Task<AuthResponse> GenerateTokenAsync(ApplicationUser user)
    {
        var claims = new List<Claim>
        {
            new(ClaimTypes.NameIdentifier, user.Id),
            new(ClaimTypes.Email, user.Email!),
            new(ClaimTypes.Name, user.FullName),
            new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
        };

        var roles = await _userManager.GetRolesAsync(user);
        claims.AddRange(roles.Select(role => new Claim(ClaimTypes.Role, role)));

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Secret"]!));
        var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var expiryMinutes = int.Parse(_configuration["Jwt:ExpiryMinutes"] ?? "60");
        var expiresAt = _dateTime.UtcNow.AddMinutes(expiryMinutes);

        var token = new JwtSecurityToken(
            issuer: _configuration["Jwt:Issuer"],
            audience: _configuration["Jwt:Audience"],
            claims: claims,
            expires: expiresAt,
            signingCredentials: credentials
        );

        var accessToken = new JwtSecurityTokenHandler().WriteToken(token);
        var refreshToken = GenerateRefreshToken();

        // Update user with refresh token
        user.RefreshToken = refreshToken;
        user.RefreshTokenExpiryTime = _dateTime.UtcNow.AddDays(7);
        user.UpdatedAt = _dateTime.UtcNow;
        await _userManager.UpdateAsync(user);

        return new AuthResponse
        {
            UserId = user.Id,
            Email = user.Email!,
            FullName = user.FullName,
            AccessToken = accessToken,
            RefreshToken = refreshToken,
            ExpiresAt = expiresAt
        };
    }

    private static string GenerateRefreshToken()
    {
        var randomNumber = new byte[64];
        using var rng = RandomNumberGenerator.Create();
        rng.GetBytes(randomNumber);
        return Convert.ToBase64String(randomNumber);
    }
}
