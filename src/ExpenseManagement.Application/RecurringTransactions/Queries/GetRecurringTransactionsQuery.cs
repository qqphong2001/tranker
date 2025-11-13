using ExpenseManagement.Application.Common.Interfaces;
using ExpenseManagement.Application.Common.Models;
using ExpenseManagement.Application.RecurringTransactions.DTOs;
using ExpenseManagement.Domain.Enums;
using Microsoft.EntityFrameworkCore;

namespace ExpenseManagement.Application.RecurringTransactions.Queries;

public class GetRecurringTransactionsQuery
{
    public int PageNumber { get; set; } = 1;
    public int PageSize { get; set; } = 10;
    public TransactionType? TransactionType { get; set; }
    public bool? IsActive { get; set; }
}

public class GetRecurringTransactionsQueryHandler
{
    private readonly IApplicationDbContext _context;
    private readonly ICurrentUserService _currentUser;

    public GetRecurringTransactionsQueryHandler(IApplicationDbContext context, ICurrentUserService currentUser)
    {
        _context = context;
        _currentUser = currentUser;
    }

    public async Task<PaginatedList<RecurringTransactionDto>> Handle(GetRecurringTransactionsQuery request, CancellationToken cancellationToken)
    {
        var query = _context.RecurringTransactions
            .Include(rt => rt.Category)
            .Include(rt => rt.Currency)
            .Where(rt => rt.UserId == _currentUser.UserId)
            .AsQueryable();

        // Apply filters
        if (request.TransactionType.HasValue)
            query = query.Where(rt => rt.TransactionType == request.TransactionType.Value);

        if (request.IsActive.HasValue)
            query = query.Where(rt => rt.IsActive == request.IsActive.Value);

        // Order by next occurrence
        query = query.OrderBy(rt => rt.NextOccurrence);

        // Project to DTO
        var dtoQuery = query.Select(rt => new RecurringTransactionDto
        {
            Id = rt.Id,
            Name = rt.Name,
            Amount = rt.Amount,
            Description = rt.Description,
            RecurrenceType = rt.RecurrenceType,
            TransactionType = rt.TransactionType,
            StartDate = rt.StartDate,
            EndDate = rt.EndDate,
            NextOccurrence = rt.NextOccurrence,
            IsActive = rt.IsActive,
            CategoryId = rt.CategoryId,
            CategoryName = rt.Category.Name,
            CategoryColor = rt.Category.Color,
            CurrencyId = rt.CurrencyId,
            CurrencyCode = rt.Currency != null ? rt.Currency.Code : null,
            CurrencySymbol = rt.Currency != null ? rt.Currency.Symbol : null
        });

        return await PaginatedList<RecurringTransactionDto>.CreateAsync(dtoQuery, request.PageNumber, request.PageSize);
    }
}
