using ExpenseManagement.Application.Common.Interfaces;
using ExpenseManagement.Application.Common.Models;
using ExpenseManagement.Application.Expenses.DTOs;
using Microsoft.EntityFrameworkCore;

namespace ExpenseManagement.Application.Expenses.Commands;

public class UpdateExpenseCommandHandler
{
    private readonly IApplicationDbContext _context;
    private readonly ICurrentUserService _currentUser;
    private readonly IDateTime _dateTime;

    public UpdateExpenseCommandHandler(
        IApplicationDbContext context,
        ICurrentUserService currentUser,
        IDateTime dateTime)
    {
        _context = context;
        _currentUser = currentUser;
        _dateTime = dateTime;
    }

    public async Task<Result> Handle(UpdateExpenseDto request, CancellationToken cancellationToken)
    {
        var expense = await _context.Expenses
            .Include(e => e.Tags)
            .FirstOrDefaultAsync(e => e.Id == request.Id && e.UserId == _currentUser.UserId, cancellationToken);

        if (expense == null)
            return Result.Failure("Expense not found");

        expense.Amount = request.Amount;
        expense.Description = request.Description;
        expense.Date = request.Date;
        expense.Notes = request.Notes;
        expense.ImageUrl = request.ImageUrl;
        expense.CategoryId = request.CategoryId;
        expense.CurrencyId = request.CurrencyId;
        expense.UpdatedAt = _dateTime.UtcNow;
        expense.UpdatedBy = _currentUser.Email;

        // Update tags
        expense.Tags.Clear();
        if (request.TagIds?.Any() == true)
        {
            var tags = await _context.Tags
                .Where(t => request.TagIds.Contains(t.Id) && t.UserId == _currentUser.UserId)
                .ToListAsync(cancellationToken);

            expense.Tags = tags;
        }

        await _context.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}
