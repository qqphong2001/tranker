using ExpenseManagement.Application.Common.Interfaces;
using ExpenseManagement.Application.Common.Models;
using ExpenseManagement.Application.Subscriptions.DTOs;
using Microsoft.EntityFrameworkCore;

namespace ExpenseManagement.Application.Subscriptions.Commands;

public class UpdateSubscriptionCommandHandler
{
    private readonly IApplicationDbContext _context;
    private readonly ICurrentUserService _currentUser;
    private readonly IDateTime _dateTime;

    public UpdateSubscriptionCommandHandler(
        IApplicationDbContext context,
        ICurrentUserService currentUser,
        IDateTime dateTime)
    {
        _context = context;
        _currentUser = currentUser;
        _dateTime = dateTime;
    }

    public async Task<Result> Handle(UpdateSubscriptionDto request, CancellationToken cancellationToken)
    {
        var subscription = await _context.Subscriptions
            .FirstOrDefaultAsync(s => s.Id == request.Id && s.UserId == _currentUser.UserId, cancellationToken);

        if (subscription == null)
            return Result.Failure("Subscription not found");

        subscription.Name = request.Name;
        subscription.Description = request.Description;
        subscription.Amount = request.Amount;
        subscription.BillingCycle = request.BillingCycle;
        subscription.ReminderDaysBefore = request.ReminderDaysBefore;
        subscription.IsActive = request.IsActive;
        subscription.CategoryId = request.CategoryId;
        subscription.UpdatedAt = _dateTime.UtcNow;
        subscription.UpdatedBy = _currentUser.Email;

        await _context.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}
