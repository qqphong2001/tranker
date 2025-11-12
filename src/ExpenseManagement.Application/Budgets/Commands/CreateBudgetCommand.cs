using ExpenseManagement.Application.Budgets.DTOs;
using ExpenseManagement.Application.Common.Interfaces;
using ExpenseManagement.Application.Common.Models;
using ExpenseManagement.Domain.Entities;
using ExpenseManagement.Domain.Enums;

namespace ExpenseManagement.Application.Budgets.Commands;

public class CreateBudgetCommandHandler
{
    private readonly IApplicationDbContext _context;
    private readonly ICurrentUserService _currentUser;
    private readonly IDateTime _dateTime;

    public CreateBudgetCommandHandler(
        IApplicationDbContext context,
        ICurrentUserService currentUser,
        IDateTime dateTime)
    {
        _context = context;
        _currentUser = currentUser;
        _dateTime = dateTime;
    }

    public async Task<Result<Guid>> Handle(CreateBudgetDto request, CancellationToken cancellationToken)
    {
        var endDate = CalculateEndDate(request.StartDate, request.Period);

        var budget = new Budget
        {
            Id = Guid.NewGuid(),
            Name = request.Name,
            Amount = request.Amount,
            SpentAmount = 0,
            Period = request.Period,
            StartDate = request.StartDate,
            EndDate = endDate,
            WarningThreshold = request.WarningThreshold,
            IsActive = true,
            CategoryId = request.CategoryId,
            UserId = _currentUser.UserId!,
            CreatedAt = _dateTime.UtcNow,
            CreatedBy = _currentUser.Email
        };

        _context.Budgets.Add(budget);
        await _context.SaveChangesAsync(cancellationToken);

        return Result<Guid>.Success(budget.Id);
    }

    private static DateTime CalculateEndDate(DateTime startDate, BudgetPeriod period)
    {
        return period switch
        {
            BudgetPeriod.Monthly => startDate.AddMonths(1).AddDays(-1),
            BudgetPeriod.Quarterly => startDate.AddMonths(3).AddDays(-1),
            BudgetPeriod.Yearly => startDate.AddYears(1).AddDays(-1),
            _ => startDate.AddMonths(1).AddDays(-1)
        };
    }
}
