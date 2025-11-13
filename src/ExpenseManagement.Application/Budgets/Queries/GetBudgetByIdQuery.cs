using ExpenseManagement.Application.Budgets.DTOs;
using ExpenseManagement.Application.Common.Interfaces;
using ExpenseManagement.Application.Common.Models;
using Microsoft.EntityFrameworkCore;

namespace ExpenseManagement.Application.Budgets.Queries;

public class GetBudgetByIdQueryHandler
{
    private readonly IApplicationDbContext _context;
    private readonly ICurrentUserService _currentUser;

    public GetBudgetByIdQueryHandler(IApplicationDbContext context, ICurrentUserService currentUser)
    {
        _context = context;
        _currentUser = currentUser;
    }

    public async Task<Result<BudgetDto>> Handle(Guid id, CancellationToken cancellationToken)
    {
        var budget = await _context.Budgets
            .Include(b => b.Category)
            .Where(b => b.Id == id && b.UserId == _currentUser.UserId)
            .Select(b => new BudgetDto
            {
                Id = b.Id,
                Name = b.Name,
                Amount = b.Amount,
                SpentAmount = b.SpentAmount,
                RemainingAmount = b.Amount - b.SpentAmount,
                Percentage = b.Amount > 0 ? (b.SpentAmount / b.Amount) * 100 : 0,
                Period = b.Period,
                StartDate = b.StartDate,
                EndDate = b.EndDate,
                WarningThreshold = b.WarningThreshold,
                IsActive = b.IsActive,
                CategoryId = b.CategoryId,
                CategoryName = b.Category.Name,
                CategoryColor = b.Category.Color,
                IsExceeded = b.SpentAmount > b.Amount,
                IsWarning = b.Amount > 0 && (b.SpentAmount / b.Amount) * 100 >= b.WarningThreshold
            })
            .FirstOrDefaultAsync(cancellationToken);

        if (budget == null)
            return Result<BudgetDto>.Failure("Budget not found");

        return Result<BudgetDto>.Success(budget);
    }
}
