using ExpenseManagement.Application.Common.Interfaces;
using ExpenseManagement.Application.Common.Models;
using ExpenseManagement.Application.Incomes.DTOs;
using ExpenseManagement.Domain.Entities;

namespace ExpenseManagement.Application.Incomes.Commands;

public class CreateIncomeCommandHandler
{
    private readonly IApplicationDbContext _context;
    private readonly ICurrentUserService _currentUser;
    private readonly IDateTime _dateTime;

    public CreateIncomeCommandHandler(
        IApplicationDbContext context,
        ICurrentUserService currentUser,
        IDateTime dateTime)
    {
        _context = context;
        _currentUser = currentUser;
        _dateTime = dateTime;
    }

    public async Task<Result<Guid>> Handle(CreateIncomeDto request, CancellationToken cancellationToken)
    {
        var income = new Income
        {
            Id = Guid.NewGuid(),
            Amount = request.Amount,
            Description = request.Description,
            Date = request.Date,
            Notes = request.Notes,
            Source = request.Source,
            CategoryId = request.CategoryId,
            CurrencyId = request.CurrencyId,
            UserId = _currentUser.UserId!,
            CreatedAt = _dateTime.UtcNow,
            CreatedBy = _currentUser.Email
        };

        _context.Incomes.Add(income);
        await _context.SaveChangesAsync(cancellationToken);

        return Result<Guid>.Success(income.Id);
    }
}
