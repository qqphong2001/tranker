using ExpenseManagement.Application.Common.Interfaces;
using ExpenseManagement.Application.Common.Models;
using Microsoft.EntityFrameworkCore;

namespace ExpenseManagement.Application.Notifications.Commands;

public class DeleteNotificationCommandHandler
{
    private readonly IApplicationDbContext _context;
    private readonly ICurrentUserService _currentUser;

    public DeleteNotificationCommandHandler(IApplicationDbContext context, ICurrentUserService currentUser)
    {
        _context = context;
        _currentUser = currentUser;
    }

    public async Task<Result> Handle(Guid id, CancellationToken cancellationToken)
    {
        var notification = await _context.Notifications
            .FirstOrDefaultAsync(n => n.Id == id && n.UserId == _currentUser.UserId, cancellationToken);

        if (notification == null)
            return Result.Failure("Notification not found");

        _context.Notifications.Remove(notification);
        await _context.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}
