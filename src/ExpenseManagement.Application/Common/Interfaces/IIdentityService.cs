using ExpenseManagement.Application.Authentication.DTOs;
using ExpenseManagement.Application.Common.Models;

namespace ExpenseManagement.Application.Common.Interfaces;

public interface IIdentityService
{
    Task<Result<AuthResponse>> RegisterAsync(string email, string password, string fullName, CancellationToken cancellationToken = default);
    Task<Result<AuthResponse>> LoginAsync(string email, string password, CancellationToken cancellationToken = default);
    Task<Result<AuthResponse>> RefreshTokenAsync(string refreshToken, CancellationToken cancellationToken = default);
    Task<Result> RevokeTokenAsync(string userId, CancellationToken cancellationToken = default);
}
