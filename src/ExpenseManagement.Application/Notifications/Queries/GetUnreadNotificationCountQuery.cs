using ExpenseManagement.Application.Common.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace ExpenseManagement.Application.Notifications.Queries;

public class GetUnreadNotificationCountQueryHandler
{
    private readonly IApplicationDbContext _context;
    private readonly ICurrentUserService _currentUser;

    public GetUnreadNotificationCountQueryHandler(IApplicationDbContext context, ICurrentUserService currentUser)
    {
        _context = context;
        _currentUser = currentUser;
    }

    public async Task<int> Handle(CancellationToken cancellationToken)
    {
        return await _context.Notifications
            .Where(n => n.UserId == _currentUser.UserId && !n.IsRead)
            .CountAsync(cancellationToken);
    }
}
