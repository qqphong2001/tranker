using ExpenseManagement.Application.Common.Interfaces;
using ExpenseManagement.Application.Common.Models;
using Microsoft.EntityFrameworkCore;

namespace ExpenseManagement.Application.Subscriptions.Commands;

public class DeleteSubscriptionCommandHandler
{
    private readonly IApplicationDbContext _context;
    private readonly ICurrentUserService _currentUser;

    public DeleteSubscriptionCommandHandler(IApplicationDbContext context, ICurrentUserService currentUser)
    {
        _context = context;
        _currentUser = currentUser;
    }

    public async Task<Result> Handle(Guid id, CancellationToken cancellationToken)
    {
        var subscription = await _context.Subscriptions
            .FirstOrDefaultAsync(s => s.Id == id && s.UserId == _currentUser.UserId, cancellationToken);

        if (subscription == null)
            return Result.Failure("Subscription not found");

        _context.Subscriptions.Remove(subscription);
        await _context.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}
