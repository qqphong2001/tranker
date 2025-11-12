using ExpenseManagement.Application.Common.Interfaces;
using ExpenseManagement.Application.Common.Models;
using Microsoft.EntityFrameworkCore;

namespace ExpenseManagement.Application.Expenses.Commands;

public class DeleteExpenseCommandHandler
{
    private readonly IApplicationDbContext _context;
    private readonly ICurrentUserService _currentUser;

    public DeleteExpenseCommandHandler(IApplicationDbContext context, ICurrentUserService currentUser)
    {
        _context = context;
        _currentUser = currentUser;
    }

    public async Task<Result> Handle(Guid id, CancellationToken cancellationToken)
    {
        var expense = await _context.Expenses
            .FirstOrDefaultAsync(e => e.Id == id && e.UserId == _currentUser.UserId, cancellationToken);

        if (expense == null)
            return Result.Failure("Expense not found");

        _context.Expenses.Remove(expense);
        await _context.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}
