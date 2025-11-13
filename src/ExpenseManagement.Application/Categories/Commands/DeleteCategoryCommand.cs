using ExpenseManagement.Application.Common.Interfaces;
using ExpenseManagement.Application.Common.Models;
using Microsoft.EntityFrameworkCore;

namespace ExpenseManagement.Application.Categories.Commands;

public class DeleteCategoryCommandHandler
{
    private readonly IApplicationDbContext _context;
    private readonly ICurrentUserService _currentUser;

    public DeleteCategoryCommandHandler(IApplicationDbContext context, ICurrentUserService currentUser)
    {
        _context = context;
        _currentUser = currentUser;
    }

    public async Task<Result> Handle(Guid id, CancellationToken cancellationToken)
    {
        var category = await _context.Categories
            .FirstOrDefaultAsync(c => c.Id == id && c.UserId == _currentUser.UserId, cancellationToken);

        if (category == null)
            return Result.Failure("Category not found");

        if (category.IsDefault)
            return Result.Failure("Cannot delete default category");

        // Check if category is in use
        var hasExpenses = await _context.Expenses.AnyAsync(e => e.CategoryId == id, cancellationToken);
        var hasIncomes = await _context.Incomes.AnyAsync(i => i.CategoryId == id, cancellationToken);
        var hasBudgets = await _context.Budgets.AnyAsync(b => b.CategoryId == id, cancellationToken);

        if (hasExpenses || hasIncomes || hasBudgets)
            return Result.Failure("Category is in use and cannot be deleted");

        _context.Categories.Remove(category);
        await _context.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}
