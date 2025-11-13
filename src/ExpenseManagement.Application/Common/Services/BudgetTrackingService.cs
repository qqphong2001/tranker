using ExpenseManagement.Application.Common.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace ExpenseManagement.Application.Common.Services;

public interface IBudgetTrackingService
{
    Task UpdateBudgetSpentAmountsAsync(string userId, Guid categoryId, CancellationToken cancellationToken);
}

public class BudgetTrackingService : IBudgetTrackingService
{
    private readonly IApplicationDbContext _context;

    public BudgetTrackingService(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task UpdateBudgetSpentAmountsAsync(string userId, Guid categoryId, CancellationToken cancellationToken)
    {
        // Get all active budgets for this user and category
        var budgets = await _context.Budgets
            .Where(b => b.UserId == userId &&
                        b.CategoryId == categoryId &&
                        b.IsActive)
            .ToListAsync(cancellationToken);

        foreach (var budget in budgets)
        {
            // Calculate total expenses for this budget period
            var totalExpenses = await _context.Expenses
                .Where(e => e.UserId == userId &&
                           e.CategoryId == categoryId &&
                           e.Date >= budget.StartDate &&
                           e.Date <= budget.EndDate)
                .SumAsync(e => (decimal?)e.Amount, cancellationToken) ?? 0;

            budget.SpentAmount = totalExpenses;
        }

        await _context.SaveChangesAsync(cancellationToken);
    }
}
