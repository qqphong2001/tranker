using ExpenseManagement.Application.Common.Interfaces;
using ExpenseManagement.Application.Common.Models;
using ExpenseManagement.Application.Incomes.DTOs;
using Microsoft.EntityFrameworkCore;

namespace ExpenseManagement.Application.Incomes.Commands;

public class UpdateIncomeCommandHandler
{
    private readonly IApplicationDbContext _context;
    private readonly ICurrentUserService _currentUser;
    private readonly IDateTime _dateTime;

    public UpdateIncomeCommandHandler(
        IApplicationDbContext context,
        ICurrentUserService currentUser,
        IDateTime dateTime)
    {
        _context = context;
        _currentUser = currentUser;
        _dateTime = dateTime;
    }

    public async Task<Result> Handle(UpdateIncomeDto request, CancellationToken cancellationToken)
    {
        var income = await _context.Incomes
            .FirstOrDefaultAsync(i => i.Id == request.Id && i.UserId == _currentUser.UserId, cancellationToken);

        if (income == null)
            return Result.Failure("Income not found");

        income.Amount = request.Amount;
        income.Description = request.Description;
        income.Date = request.Date;
        income.Notes = request.Notes;
        income.Source = request.Source;
        income.CategoryId = request.CategoryId;
        income.CurrencyId = request.CurrencyId;
        income.UpdatedAt = _dateTime.UtcNow;
        income.UpdatedBy = _currentUser.Email;

        await _context.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}
