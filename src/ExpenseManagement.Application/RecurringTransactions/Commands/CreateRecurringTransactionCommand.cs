using ExpenseManagement.Application.Common.Interfaces;
using ExpenseManagement.Application.Common.Models;
using ExpenseManagement.Application.RecurringTransactions.DTOs;
using ExpenseManagement.Domain.Entities;
using ExpenseManagement.Domain.Enums;

namespace ExpenseManagement.Application.RecurringTransactions.Commands;

public class CreateRecurringTransactionCommandHandler
{
    private readonly IApplicationDbContext _context;
    private readonly ICurrentUserService _currentUser;
    private readonly IDateTime _dateTime;

    public CreateRecurringTransactionCommandHandler(
        IApplicationDbContext context,
        ICurrentUserService currentUser,
        IDateTime dateTime)
    {
        _context = context;
        _currentUser = currentUser;
        _dateTime = dateTime;
    }

    public async Task<Result<Guid>> Handle(CreateRecurringTransactionDto request, CancellationToken cancellationToken)
    {
        var nextOccurrence = CalculateNextOccurrence(request.StartDate, request.RecurrenceType);

        var recurringTransaction = new RecurringTransaction
        {
            Id = Guid.NewGuid(),
            Name = request.Name,
            Amount = request.Amount,
            Description = request.Description,
            RecurrenceType = request.RecurrenceType,
            TransactionType = request.TransactionType,
            StartDate = request.StartDate,
            EndDate = request.EndDate,
            NextOccurrence = nextOccurrence,
            IsActive = true,
            CategoryId = request.CategoryId,
            CurrencyId = request.CurrencyId,
            UserId = _currentUser.UserId!,
            CreatedAt = _dateTime.UtcNow,
            CreatedBy = _currentUser.Email
        };

        _context.RecurringTransactions.Add(recurringTransaction);
        await _context.SaveChangesAsync(cancellationToken);

        return Result<Guid>.Success(recurringTransaction.Id);
    }

    private static DateTime CalculateNextOccurrence(DateTime startDate, RecurrenceType recurrenceType)
    {
        return recurrenceType switch
        {
            RecurrenceType.Daily => startDate.AddDays(1),
            RecurrenceType.Weekly => startDate.AddDays(7),
            RecurrenceType.Monthly => startDate.AddMonths(1),
            RecurrenceType.Yearly => startDate.AddYears(1),
            _ => startDate
        };
    }
}
