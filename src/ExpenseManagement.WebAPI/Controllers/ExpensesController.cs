using ExpenseManagement.Application.Expenses.Commands;
using ExpenseManagement.Application.Expenses.DTOs;
using ExpenseManagement.Application.Expenses.Queries;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ExpenseManagement.WebAPI.Controllers;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class ExpensesController : ControllerBase
{
    private readonly CreateExpenseCommandHandler _createExpenseHandler;
    private readonly UpdateExpenseCommandHandler _updateExpenseHandler;
    private readonly DeleteExpenseCommandHandler _deleteExpenseHandler;
    private readonly GetExpensesQueryHandler _getExpensesHandler;

    public ExpensesController(
        CreateExpenseCommandHandler createExpenseHandler,
        UpdateExpenseCommandHandler updateExpenseHandler,
        DeleteExpenseCommandHandler deleteExpenseHandler,
        GetExpensesQueryHandler getExpensesHandler)
    {
        _createExpenseHandler = createExpenseHandler;
        _updateExpenseHandler = updateExpenseHandler;
        _deleteExpenseHandler = deleteExpenseHandler;
        _getExpensesHandler = getExpensesHandler;
    }

    [HttpGet]
    public async Task<IActionResult> GetExpenses([FromQuery] GetExpensesQuery query, CancellationToken cancellationToken)
    {
        var result = await _getExpensesHandler.Handle(query, cancellationToken);
        return Ok(result);
    }

    [HttpPost]
    public async Task<IActionResult> CreateExpense([FromBody] CreateExpenseDto dto, CancellationToken cancellationToken)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var result = await _createExpenseHandler.Handle(dto, cancellationToken);

        if (!result.Succeeded)
            return BadRequest(new { message = string.Join(", ", result.Errors) });

        return CreatedAtAction(nameof(GetExpenses), new { id = result.Data }, result.Data);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateExpense(Guid id, [FromBody] UpdateExpenseDto dto, CancellationToken cancellationToken)
    {
        if (id != dto.Id)
            return BadRequest(new { message = "ID mismatch" });

        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var result = await _updateExpenseHandler.Handle(dto, cancellationToken);

        if (!result.Succeeded)
            return BadRequest(new { message = string.Join(", ", result.Errors) });

        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteExpense(Guid id, CancellationToken cancellationToken)
    {
        var result = await _deleteExpenseHandler.Handle(id, cancellationToken);

        if (!result.Succeeded)
            return BadRequest(new { message = string.Join(", ", result.Errors) });

        return NoContent();
    }
}
