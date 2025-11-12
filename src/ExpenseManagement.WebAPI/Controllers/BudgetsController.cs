using ExpenseManagement.Application.Budgets.Commands;
using ExpenseManagement.Application.Budgets.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ExpenseManagement.WebAPI.Controllers;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class BudgetsController : ControllerBase
{
    private readonly CreateBudgetCommandHandler _createBudgetHandler;

    public BudgetsController(CreateBudgetCommandHandler createBudgetHandler)
    {
        _createBudgetHandler = createBudgetHandler;
    }

    [HttpPost]
    public async Task<IActionResult> CreateBudget([FromBody] CreateBudgetDto dto, CancellationToken cancellationToken)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var result = await _createBudgetHandler.Handle(dto, cancellationToken);

        if (!result.Succeeded)
            return BadRequest(new { message = string.Join(", ", result.Errors) });

        return CreatedAtAction(nameof(CreateBudget), new { id = result.Data }, result.Data);
    }
}
