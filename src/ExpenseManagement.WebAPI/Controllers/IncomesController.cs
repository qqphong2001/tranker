using ExpenseManagement.Application.Incomes.Commands;
using ExpenseManagement.Application.Incomes.DTOs;
using ExpenseManagement.Application.Incomes.Queries;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ExpenseManagement.WebAPI.Controllers;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class IncomesController : ControllerBase
{
    private readonly CreateIncomeCommandHandler _createIncomeHandler;
    private readonly UpdateIncomeCommandHandler _updateIncomeHandler;
    private readonly DeleteIncomeCommandHandler _deleteIncomeHandler;
    private readonly GetIncomesQueryHandler _getIncomesHandler;
    private readonly GetIncomeByIdQueryHandler _getIncomeByIdHandler;

    public IncomesController(
        CreateIncomeCommandHandler createIncomeHandler,
        UpdateIncomeCommandHandler updateIncomeHandler,
        DeleteIncomeCommandHandler deleteIncomeHandler,
        GetIncomesQueryHandler getIncomesHandler,
        GetIncomeByIdQueryHandler getIncomeByIdHandler)
    {
        _createIncomeHandler = createIncomeHandler;
        _updateIncomeHandler = updateIncomeHandler;
        _deleteIncomeHandler = deleteIncomeHandler;
        _getIncomesHandler = getIncomesHandler;
        _getIncomeByIdHandler = getIncomeByIdHandler;
    }

    [HttpGet]
    public async Task<IActionResult> GetIncomes([FromQuery] GetIncomesQuery query, CancellationToken cancellationToken)
    {
        var result = await _getIncomesHandler.Handle(query, cancellationToken);
        return Ok(result);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetIncomeById(Guid id, CancellationToken cancellationToken)
    {
        var result = await _getIncomeByIdHandler.Handle(id, cancellationToken);

        if (!result.Succeeded)
            return NotFound(new { message = string.Join(", ", result.Errors) });

        return Ok(result.Data);
    }

    [HttpPost]
    public async Task<IActionResult> CreateIncome([FromBody] CreateIncomeDto dto, CancellationToken cancellationToken)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var result = await _createIncomeHandler.Handle(dto, cancellationToken);

        if (!result.Succeeded)
            return BadRequest(new { message = string.Join(", ", result.Errors) });

        return CreatedAtAction(nameof(GetIncomeById), new { id = result.Data }, result.Data);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateIncome(Guid id, [FromBody] UpdateIncomeDto dto, CancellationToken cancellationToken)
    {
        if (id != dto.Id)
            return BadRequest(new { message = "ID mismatch" });

        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var result = await _updateIncomeHandler.Handle(dto, cancellationToken);

        if (!result.Succeeded)
            return BadRequest(new { message = string.Join(", ", result.Errors) });

        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteIncome(Guid id, CancellationToken cancellationToken)
    {
        var result = await _deleteIncomeHandler.Handle(id, cancellationToken);

        if (!result.Succeeded)
            return BadRequest(new { message = string.Join(", ", result.Errors) });

        return NoContent();
    }
}
