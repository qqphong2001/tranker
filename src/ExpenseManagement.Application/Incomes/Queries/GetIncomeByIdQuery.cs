using ExpenseManagement.Application.Common.Interfaces;
using ExpenseManagement.Application.Common.Models;
using ExpenseManagement.Application.Incomes.DTOs;
using Microsoft.EntityFrameworkCore;

namespace ExpenseManagement.Application.Incomes.Queries;

public class GetIncomeByIdQueryHandler
{
    private readonly IApplicationDbContext _context;
    private readonly ICurrentUserService _currentUser;

    public GetIncomeByIdQueryHandler(IApplicationDbContext context, ICurrentUserService currentUser)
    {
        _context = context;
        _currentUser = currentUser;
    }

    public async Task<Result<IncomeDto>> Handle(Guid id, CancellationToken cancellationToken)
    {
        var income = await _context.Incomes
            .Include(i => i.Category)
            .Include(i => i.Currency)
            .Where(i => i.Id == id && i.UserId == _currentUser.UserId)
            .Select(i => new IncomeDto
            {
                Id = i.Id,
                Amount = i.Amount,
                Description = i.Description,
                Date = i.Date,
                Notes = i.Notes,
                Source = i.Source,
                CategoryId = i.CategoryId,
                CategoryName = i.Category.Name,
                CategoryColor = i.Category.Color,
                CategoryIcon = i.Category.Icon,
                CurrencyId = i.CurrencyId,
                CurrencyCode = i.Currency != null ? i.Currency.Code : null,
                CurrencySymbol = i.Currency != null ? i.Currency.Symbol : null,
                CreatedAt = i.CreatedAt
            })
            .FirstOrDefaultAsync(cancellationToken);

        if (income == null)
            return Result<IncomeDto>.Failure("Income not found");

        return Result<IncomeDto>.Success(income);
    }
}
