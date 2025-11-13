using ExpenseManagement.Application.Common.Interfaces;
using ExpenseManagement.Application.Common.Models;
using ExpenseManagement.Application.Subscriptions.DTOs;
using Microsoft.EntityFrameworkCore;

namespace ExpenseManagement.Application.Subscriptions.Queries;

public class GetSubscriptionByIdQueryHandler
{
    private readonly IApplicationDbContext _context;
    private readonly ICurrentUserService _currentUser;

    public GetSubscriptionByIdQueryHandler(IApplicationDbContext context, ICurrentUserService currentUser)
    {
        _context = context;
        _currentUser = currentUser;
    }

    public async Task<Result<SubscriptionDto>> Handle(Guid id, CancellationToken cancellationToken)
    {
        var subscription = await _context.Subscriptions
            .Include(s => s.Category)
            .Include(s => s.Currency)
            .Where(s => s.Id == id && s.UserId == _currentUser.UserId)
            .Select(s => new SubscriptionDto
            {
                Id = s.Id,
                Name = s.Name,
                Description = s.Description,
                Amount = s.Amount,
                StartDate = s.StartDate,
                EndDate = s.EndDate,
                NextBillingDate = s.NextBillingDate,
                BillingCycle = s.BillingCycle,
                IsActive = s.IsActive,
                ReminderDaysBefore = s.ReminderDaysBefore,
                CategoryId = s.CategoryId,
                CategoryName = s.Category.Name,
                CategoryColor = s.Category.Color,
                CurrencyId = s.CurrencyId,
                CurrencyCode = s.Currency != null ? s.Currency.Code : null,
                CurrencySymbol = s.Currency != null ? s.Currency.Symbol : null
            })
            .FirstOrDefaultAsync(cancellationToken);

        if (subscription == null)
            return Result<SubscriptionDto>.Failure("Subscription not found");

        return Result<SubscriptionDto>.Success(subscription);
    }
}
