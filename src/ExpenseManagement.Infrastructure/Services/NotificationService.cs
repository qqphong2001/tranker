using ExpenseManagement.Application.Common.Interfaces;
using ExpenseManagement.Application.Common.Models;
using ExpenseManagement.Domain.Entities;
using ExpenseManagement.Domain.Enums;

namespace ExpenseManagement.Infrastructure.Services;

public class NotificationService : INotificationService
{
    private readonly IApplicationDbContext _context;
    private readonly IEmailService _emailService;
    private readonly IDateTime _dateTime;

    public NotificationService(
        IApplicationDbContext context,
        IEmailService emailService,
        IDateTime dateTime)
    {
        _context = context;
        _emailService = emailService;
        _dateTime = dateTime;
    }

    public async Task CreateNotificationAsync(
        string userId,
        string title,
        string message,
        NotificationType type,
        string? relatedEntityId = null,
        CancellationToken cancellationToken = default)
    {
        var notification = new Notification
        {
            Id = Guid.NewGuid(),
            UserId = userId,
            Title = title,
            Message = message,
            Type = type,
            IsRead = false,
            RelatedEntityId = relatedEntityId,
            CreatedAt = _dateTime.UtcNow
        };

        _context.Notifications.Add(notification);
        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task SendEmailNotificationAsync(
        string email,
        string subject,
        string body,
        CancellationToken cancellationToken = default)
    {
        var emailRequest = new EmailRequest
        {
            To = email,
            Subject = subject,
            Body = body,
            IsHtml = true
        };

        await _emailService.SendEmailAsync(emailRequest, cancellationToken);
    }
}
