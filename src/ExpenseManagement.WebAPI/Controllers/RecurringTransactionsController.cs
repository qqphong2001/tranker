using ExpenseManagement.Application.RecurringTransactions.Commands;
using ExpenseManagement.Application.RecurringTransactions.DTOs;
using ExpenseManagement.Application.RecurringTransactions.Queries;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ExpenseManagement.WebAPI.Controllers;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class RecurringTransactionsController : ControllerBase
{
    private readonly CreateRecurringTransactionCommandHandler _createHandler;
    private readonly UpdateRecurringTransactionCommandHandler _updateHandler;
    private readonly DeleteRecurringTransactionCommandHandler _deleteHandler;
    private readonly GetRecurringTransactionsQueryHandler _getHandler;

    public RecurringTransactionsController(
        CreateRecurringTransactionCommandHandler createHandler,
        UpdateRecurringTransactionCommandHandler updateHandler,
        DeleteRecurringTransactionCommandHandler deleteHandler,
        GetRecurringTransactionsQueryHandler getHandler)
    {
        _createHandler = createHandler;
        _updateHandler = updateHandler;
        _deleteHandler = deleteHandler;
        _getHandler = getHandler;
    }

    [HttpGet]
    public async Task<IActionResult> GetRecurringTransactions([FromQuery] GetRecurringTransactionsQuery query, CancellationToken cancellationToken)
    {
        var result = await _getHandler.Handle(query, cancellationToken);
        return Ok(result);
    }

    [HttpPost]
    public async Task<IActionResult> CreateRecurringTransaction([FromBody] CreateRecurringTransactionDto dto, CancellationToken cancellationToken)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var result = await _createHandler.Handle(dto, cancellationToken);

        if (!result.Succeeded)
            return BadRequest(new { message = string.Join(", ", result.Errors) });

        return CreatedAtAction(nameof(GetRecurringTransactions), new { id = result.Data }, result.Data);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateRecurringTransaction(Guid id, [FromBody] UpdateRecurringTransactionDto dto, CancellationToken cancellationToken)
    {
        if (id != dto.Id)
            return BadRequest(new { message = "ID mismatch" });

        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var result = await _updateHandler.Handle(dto, cancellationToken);

        if (!result.Succeeded)
            return BadRequest(new { message = string.Join(", ", result.Errors) });

        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteRecurringTransaction(Guid id, CancellationToken cancellationToken)
    {
        var result = await _deleteHandler.Handle(id, cancellationToken);

        if (!result.Succeeded)
            return BadRequest(new { message = string.Join(", ", result.Errors) });

        return NoContent();
    }
}
