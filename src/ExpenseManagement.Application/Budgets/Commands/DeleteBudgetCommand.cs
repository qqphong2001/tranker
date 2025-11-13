using ExpenseManagement.Application.Common.Interfaces;
using ExpenseManagement.Application.Common.Models;
using Microsoft.EntityFrameworkCore;

namespace ExpenseManagement.Application.Budgets.Commands;

public class DeleteBudgetCommandHandler
{
    private readonly IApplicationDbContext _context;
    private readonly ICurrentUserService _currentUser;

    public DeleteBudgetCommandHandler(IApplicationDbContext context, ICurrentUserService currentUser)
    {
        _context = context;
        _currentUser = currentUser;
    }

    public async Task<Result> Handle(Guid id, CancellationToken cancellationToken)
    {
        var budget = await _context.Budgets
            .FirstOrDefaultAsync(b => b.Id == id && b.UserId == _currentUser.UserId, cancellationToken);

        if (budget == null)
            return Result.Failure("Budget not found");

        _context.Budgets.Remove(budget);
        await _context.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}
