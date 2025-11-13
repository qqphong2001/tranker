using ExpenseManagement.Application.Common.Interfaces;
using ExpenseManagement.Application.Common.Models;
using ExpenseManagement.Application.Common.Services;
using ExpenseManagement.Application.Expenses.DTOs;
using ExpenseManagement.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace ExpenseManagement.Application.Expenses.Commands;

public class CreateExpenseCommandHandler
{
    private readonly IApplicationDbContext _context;
    private readonly ICurrentUserService _currentUser;
    private readonly IDateTime _dateTime;
    private readonly IBudgetTrackingService _budgetTrackingService;

    public CreateExpenseCommandHandler(
        IApplicationDbContext context,
        ICurrentUserService currentUser,
        IDateTime dateTime,
        IBudgetTrackingService budgetTrackingService)
    {
        _context = context;
        _currentUser = currentUser;
        _dateTime = dateTime;
        _budgetTrackingService = budgetTrackingService;
    }

    public async Task<Result<Guid>> Handle(CreateExpenseDto request, CancellationToken cancellationToken)
    {
        var expense = new Expense
        {
            Id = Guid.NewGuid(),
            Amount = request.Amount,
            Description = request.Description,
            Date = request.Date,
            Notes = request.Notes,
            ImageUrl = request.ImageUrl,
            CategoryId = request.CategoryId,
            CurrencyId = request.CurrencyId,
            UserId = _currentUser.UserId!,
            CreatedAt = _dateTime.UtcNow,
            CreatedBy = _currentUser.Email
        };

        // Add tags if provided
        if (request.TagIds?.Any() == true)
        {
            var tags = await _context.Tags
                .Where(t => request.TagIds.Contains(t.Id) && t.UserId == _currentUser.UserId)
                .ToListAsync(cancellationToken);

            expense.Tags = tags;
        }

        _context.Expenses.Add(expense);
        await _context.SaveChangesAsync(cancellationToken);

        // Update budget spent amounts
        await _budgetTrackingService.UpdateBudgetSpentAmountsAsync(
            _currentUser.UserId!,
            request.CategoryId,
            cancellationToken);

        return Result<Guid>.Success(expense.Id);
    }
}
