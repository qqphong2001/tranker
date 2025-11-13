using ExpenseManagement.Application.Common.Interfaces;
using ExpenseManagement.Application.Common.Models;
using ExpenseManagement.Application.Common.Services;
using Microsoft.EntityFrameworkCore;

namespace ExpenseManagement.Application.Expenses.Commands;

public class DeleteExpenseCommandHandler
{
    private readonly IApplicationDbContext _context;
    private readonly ICurrentUserService _currentUser;
    private readonly IBudgetTrackingService _budgetTrackingService;

    public DeleteExpenseCommandHandler(
        IApplicationDbContext context,
        ICurrentUserService currentUser,
        IBudgetTrackingService budgetTrackingService)
    {
        _context = context;
        _currentUser = currentUser;
        _budgetTrackingService = budgetTrackingService;
    }

    public async Task<Result> Handle(Guid id, CancellationToken cancellationToken)
    {
        var expense = await _context.Expenses
            .FirstOrDefaultAsync(e => e.Id == id && e.UserId == _currentUser.UserId, cancellationToken);

        if (expense == null)
            return Result.Failure("Expense not found");

        var categoryId = expense.CategoryId;

        _context.Expenses.Remove(expense);
        await _context.SaveChangesAsync(cancellationToken);

        // Update budget spent amounts
        await _budgetTrackingService.UpdateBudgetSpentAmountsAsync(
            _currentUser.UserId!,
            categoryId,
            cancellationToken);

        return Result.Success();
    }
}
