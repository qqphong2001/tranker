using ExpenseManagement.Application.Budgets.DTOs;
using ExpenseManagement.Application.Common.Interfaces;
using ExpenseManagement.Application.Common.Models;
using Microsoft.EntityFrameworkCore;

namespace ExpenseManagement.Application.Budgets.Commands;

public class UpdateBudgetCommandHandler
{
    private readonly IApplicationDbContext _context;
    private readonly ICurrentUserService _currentUser;
    private readonly IDateTime _dateTime;

    public UpdateBudgetCommandHandler(
        IApplicationDbContext context,
        ICurrentUserService currentUser,
        IDateTime dateTime)
    {
        _context = context;
        _currentUser = currentUser;
        _dateTime = dateTime;
    }

    public async Task<Result> Handle(UpdateBudgetDto request, CancellationToken cancellationToken)
    {
        var budget = await _context.Budgets
            .FirstOrDefaultAsync(b => b.Id == request.Id && b.UserId == _currentUser.UserId, cancellationToken);

        if (budget == null)
            return Result.Failure("Budget not found");

        budget.Name = request.Name;
        budget.Amount = request.Amount;
        budget.WarningThreshold = request.WarningThreshold;
        budget.IsActive = request.IsActive;
        budget.UpdatedAt = _dateTime.UtcNow;
        budget.UpdatedBy = _currentUser.Email;

        await _context.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}
