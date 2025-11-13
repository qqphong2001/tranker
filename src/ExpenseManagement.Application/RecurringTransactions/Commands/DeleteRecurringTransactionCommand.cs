using ExpenseManagement.Application.Common.Interfaces;
using ExpenseManagement.Application.Common.Models;
using Microsoft.EntityFrameworkCore;

namespace ExpenseManagement.Application.RecurringTransactions.Commands;

public class DeleteRecurringTransactionCommandHandler
{
    private readonly IApplicationDbContext _context;
    private readonly ICurrentUserService _currentUser;

    public DeleteRecurringTransactionCommandHandler(IApplicationDbContext context, ICurrentUserService currentUser)
    {
        _context = context;
        _currentUser = currentUser;
    }

    public async Task<Result> Handle(Guid id, CancellationToken cancellationToken)
    {
        var recurringTransaction = await _context.RecurringTransactions
            .FirstOrDefaultAsync(rt => rt.Id == id && rt.UserId == _currentUser.UserId, cancellationToken);

        if (recurringTransaction == null)
            return Result.Failure("Recurring transaction not found");

        _context.RecurringTransactions.Remove(recurringTransaction);
        await _context.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}
