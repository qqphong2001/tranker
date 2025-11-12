using ExpenseManagement.Domain.Enums;

namespace ExpenseManagement.Application.Common.Interfaces;

public interface INotificationService
{
    Task CreateNotificationAsync(string userId, string title, string message, NotificationType type, string? relatedEntityId = null, CancellationToken cancellationToken = default);
    Task SendEmailNotificationAsync(string email, string subject, string body, CancellationToken cancellationToken = default);
}
