using ExpenseManagement.Application.Common.Interfaces;
using ExpenseManagement.Application.Common.Models;
using Microsoft.EntityFrameworkCore;

namespace ExpenseManagement.Application.Notifications.Commands;

public class MarkAllNotificationsAsReadCommandHandler
{
    private readonly IApplicationDbContext _context;
    private readonly ICurrentUserService _currentUser;
    private readonly IDateTime _dateTime;

    public MarkAllNotificationsAsReadCommandHandler(
        IApplicationDbContext context,
        ICurrentUserService currentUser,
        IDateTime dateTime)
    {
        _context = context;
        _currentUser = currentUser;
        _dateTime = dateTime;
    }

    public async Task<Result> Handle(CancellationToken cancellationToken)
    {
        var unreadNotifications = await _context.Notifications
            .Where(n => n.UserId == _currentUser.UserId && !n.IsRead)
            .ToListAsync(cancellationToken);

        foreach (var notification in unreadNotifications)
        {
            notification.IsRead = true;
            notification.ReadAt = _dateTime.UtcNow;
            notification.UpdatedAt = _dateTime.UtcNow;
            notification.UpdatedBy = _currentUser.Email;
        }

        await _context.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}
