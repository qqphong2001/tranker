using ExpenseManagement.Application.Common.Interfaces;
using ExpenseManagement.Application.Common.Models;
using ExpenseManagement.Application.RecurringTransactions.DTOs;
using Microsoft.EntityFrameworkCore;

namespace ExpenseManagement.Application.RecurringTransactions.Commands;

public class UpdateRecurringTransactionCommandHandler
{
    private readonly IApplicationDbContext _context;
    private readonly ICurrentUserService _currentUser;
    private readonly IDateTime _dateTime;

    public UpdateRecurringTransactionCommandHandler(
        IApplicationDbContext context,
        ICurrentUserService currentUser,
        IDateTime dateTime)
    {
        _context = context;
        _currentUser = currentUser;
        _dateTime = dateTime;
    }

    public async Task<Result> Handle(UpdateRecurringTransactionDto request, CancellationToken cancellationToken)
    {
        var recurringTransaction = await _context.RecurringTransactions
            .FirstOrDefaultAsync(rt => rt.Id == request.Id && rt.UserId == _currentUser.UserId, cancellationToken);

        if (recurringTransaction == null)
            return Result.Failure("Recurring transaction not found");

        recurringTransaction.Name = request.Name;
        recurringTransaction.Amount = request.Amount;
        recurringTransaction.Description = request.Description;
        recurringTransaction.RecurrenceType = request.RecurrenceType;
        recurringTransaction.IsActive = request.IsActive;
        recurringTransaction.CategoryId = request.CategoryId;
        recurringTransaction.UpdatedAt = _dateTime.UtcNow;
        recurringTransaction.UpdatedBy = _currentUser.Email;

        await _context.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}
