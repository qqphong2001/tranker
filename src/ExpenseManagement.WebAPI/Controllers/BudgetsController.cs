using ExpenseManagement.Application.Budgets.Commands;
using ExpenseManagement.Application.Budgets.DTOs;
using ExpenseManagement.Application.Budgets.Queries;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ExpenseManagement.WebAPI.Controllers;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class BudgetsController : ControllerBase
{
    private readonly CreateBudgetCommandHandler _createBudgetHandler;
    private readonly UpdateBudgetCommandHandler _updateBudgetHandler;
    private readonly DeleteBudgetCommandHandler _deleteBudgetHandler;
    private readonly GetBudgetsQueryHandler _getBudgetsHandler;
    private readonly GetBudgetByIdQueryHandler _getBudgetByIdHandler;

    public BudgetsController(
        CreateBudgetCommandHandler createBudgetHandler,
        UpdateBudgetCommandHandler updateBudgetHandler,
        DeleteBudgetCommandHandler deleteBudgetHandler,
        GetBudgetsQueryHandler getBudgetsHandler,
        GetBudgetByIdQueryHandler getBudgetByIdHandler)
    {
        _createBudgetHandler = createBudgetHandler;
        _updateBudgetHandler = updateBudgetHandler;
        _deleteBudgetHandler = deleteBudgetHandler;
        _getBudgetsHandler = getBudgetsHandler;
        _getBudgetByIdHandler = getBudgetByIdHandler;
    }

    [HttpGet]
    public async Task<IActionResult> GetBudgets([FromQuery] GetBudgetsQuery query, CancellationToken cancellationToken)
    {
        var result = await _getBudgetsHandler.Handle(query, cancellationToken);
        return Ok(result);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetBudgetById(Guid id, CancellationToken cancellationToken)
    {
        var result = await _getBudgetByIdHandler.Handle(id, cancellationToken);

        if (!result.Succeeded)
            return NotFound(new { message = string.Join(", ", result.Errors) });

        return Ok(result.Data);
    }

    [HttpPost]
    public async Task<IActionResult> CreateBudget([FromBody] CreateBudgetDto dto, CancellationToken cancellationToken)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var result = await _createBudgetHandler.Handle(dto, cancellationToken);

        if (!result.Succeeded)
            return BadRequest(new { message = string.Join(", ", result.Errors) });

        return CreatedAtAction(nameof(GetBudgetById), new { id = result.Data }, result.Data);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateBudget(Guid id, [FromBody] UpdateBudgetDto dto, CancellationToken cancellationToken)
    {
        if (id != dto.Id)
            return BadRequest(new { message = "ID mismatch" });

        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var result = await _updateBudgetHandler.Handle(dto, cancellationToken);

        if (!result.Succeeded)
            return BadRequest(new { message = string.Join(", ", result.Errors) });

        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteBudget(Guid id, CancellationToken cancellationToken)
    {
        var result = await _deleteBudgetHandler.Handle(id, cancellationToken);

        if (!result.Succeeded)
            return BadRequest(new { message = string.Join(", ", result.Errors) });

        return NoContent();
    }
}
