using ExpenseManagement.Application.Currencies.Commands;
using ExpenseManagement.Application.Currencies.DTOs;
using ExpenseManagement.Application.Currencies.Queries;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ExpenseManagement.WebAPI.Controllers;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class CurrenciesController : ControllerBase
{
    private readonly GetCurrenciesQueryHandler _getCurrenciesHandler;
    private readonly UpdateCurrencyExchangeRateCommandHandler _updateExchangeRateHandler;

    public CurrenciesController(
        GetCurrenciesQueryHandler getCurrenciesHandler,
        UpdateCurrencyExchangeRateCommandHandler updateExchangeRateHandler)
    {
        _getCurrenciesHandler = getCurrenciesHandler;
        _updateExchangeRateHandler = updateExchangeRateHandler;
    }

    [HttpGet]
    public async Task<IActionResult> GetCurrencies(CancellationToken cancellationToken)
    {
        var result = await _getCurrenciesHandler.Handle(cancellationToken);
        return Ok(result);
    }

    [HttpPut("{id}/exchange-rate")]
    public async Task<IActionResult> UpdateExchangeRate(Guid id, [FromBody] UpdateCurrencyExchangeRateDto dto, CancellationToken cancellationToken)
    {
        if (id != dto.Id)
            return BadRequest(new { message = "ID mismatch" });

        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var result = await _updateExchangeRateHandler.Handle(dto, cancellationToken);

        if (!result.Succeeded)
            return BadRequest(new { message = string.Join(", ", result.Errors) });

        return NoContent();
    }
}
