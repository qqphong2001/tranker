using ExpenseManagement.Application.Common.Interfaces;
using ExpenseManagement.Application.Currencies.DTOs;
using Microsoft.EntityFrameworkCore;

namespace ExpenseManagement.Application.Currencies.Queries;

public class GetCurrenciesQueryHandler
{
    private readonly IApplicationDbContext _context;

    public GetCurrenciesQueryHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<List<CurrencyDto>> Handle(CancellationToken cancellationToken)
    {
        var currencies = await _context.Currencies
            .OrderByDescending(c => c.IsDefault)
            .ThenBy(c => c.Code)
            .Select(c => new CurrencyDto
            {
                Id = c.Id,
                Code = c.Code,
                Symbol = c.Symbol,
                Name = c.Name,
                ExchangeRate = c.ExchangeRate,
                IsDefault = c.IsDefault
            })
            .ToListAsync(cancellationToken);

        return currencies;
    }
}
