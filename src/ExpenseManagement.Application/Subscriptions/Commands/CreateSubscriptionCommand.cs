using ExpenseManagement.Application.Common.Interfaces;
using ExpenseManagement.Application.Common.Models;
using ExpenseManagement.Application.Subscriptions.DTOs;
using ExpenseManagement.Domain.Entities;

namespace ExpenseManagement.Application.Subscriptions.Commands;

public class CreateSubscriptionCommandHandler
{
    private readonly IApplicationDbContext _context;
    private readonly ICurrentUserService _currentUser;
    private readonly IDateTime _dateTime;

    public CreateSubscriptionCommandHandler(
        IApplicationDbContext context,
        ICurrentUserService currentUser,
        IDateTime dateTime)
    {
        _context = context;
        _currentUser = currentUser;
        _dateTime = dateTime;
    }

    public async Task<Result<Guid>> Handle(CreateSubscriptionDto request, CancellationToken cancellationToken)
    {
        var subscription = new Subscription
        {
            Id = Guid.NewGuid(),
            Name = request.Name,
            Description = request.Description,
            Amount = request.Amount,
            StartDate = request.StartDate,
            EndDate = request.EndDate,
            NextBillingDate = request.StartDate,
            BillingCycle = request.BillingCycle,
            IsActive = true,
            ReminderDaysBefore = request.ReminderDaysBefore,
            CategoryId = request.CategoryId,
            CurrencyId = request.CurrencyId,
            UserId = _currentUser.UserId!,
            CreatedAt = _dateTime.UtcNow,
            CreatedBy = _currentUser.Email
        };

        _context.Subscriptions.Add(subscription);
        await _context.SaveChangesAsync(cancellationToken);

        return Result<Guid>.Success(subscription.Id);
    }
}
