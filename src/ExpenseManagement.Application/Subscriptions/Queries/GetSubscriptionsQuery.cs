using ExpenseManagement.Application.Common.Interfaces;
using ExpenseManagement.Application.Common.Models;
using ExpenseManagement.Application.Subscriptions.DTOs;
using Microsoft.EntityFrameworkCore;

namespace ExpenseManagement.Application.Subscriptions.Queries;

public class GetSubscriptionsQuery
{
    public int PageNumber { get; set; } = 1;
    public int PageSize { get; set; } = 10;
    public Guid? CategoryId { get; set; }
    public bool? IsActive { get; set; }
}

public class GetSubscriptionsQueryHandler
{
    private readonly IApplicationDbContext _context;
    private readonly ICurrentUserService _currentUser;

    public GetSubscriptionsQueryHandler(IApplicationDbContext context, ICurrentUserService currentUser)
    {
        _context = context;
        _currentUser = currentUser;
    }

    public async Task<PaginatedList<SubscriptionDto>> Handle(GetSubscriptionsQuery request, CancellationToken cancellationToken)
    {
        var query = _context.Subscriptions
            .Include(s => s.Category)
            .Include(s => s.Currency)
            .Where(s => s.UserId == _currentUser.UserId)
            .AsQueryable();

        // Apply filters
        if (request.CategoryId.HasValue)
            query = query.Where(s => s.CategoryId == request.CategoryId.Value);

        if (request.IsActive.HasValue)
            query = query.Where(s => s.IsActive == request.IsActive.Value);

        // Order by next billing date
        query = query.OrderBy(s => s.NextBillingDate);

        // Project to DTO
        var dtoQuery = query.Select(s => new SubscriptionDto
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
        });

        return await PaginatedList<SubscriptionDto>.CreateAsync(dtoQuery, request.PageNumber, request.PageSize);
    }
}
