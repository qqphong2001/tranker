using ExpenseManagement.Application.Common.Interfaces;
using ExpenseManagement.Application.Common.Models;
using ExpenseManagement.Application.Currencies.DTOs;
using Microsoft.EntityFrameworkCore;

namespace ExpenseManagement.Application.Currencies.Commands;

public class UpdateCurrencyExchangeRateCommandHandler
{
    private readonly IApplicationDbContext _context;
    private readonly IDateTime _dateTime;

    public UpdateCurrencyExchangeRateCommandHandler(
        IApplicationDbContext context,
        IDateTime dateTime)
    {
        _context = context;
        _dateTime = dateTime;
    }

    public async Task<Result> Handle(UpdateCurrencyExchangeRateDto request, CancellationToken cancellationToken)
    {
        var currency = await _context.Currencies
            .FirstOrDefaultAsync(c => c.Id == request.Id, cancellationToken);

        if (currency == null)
            return Result.Failure("Currency not found");

        if (request.ExchangeRate <= 0)
            return Result.Failure("Exchange rate must be greater than zero");

        currency.ExchangeRate = request.ExchangeRate;

        await _context.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}
