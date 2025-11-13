using ExpenseManagement.Application.Subscriptions.Commands;
using ExpenseManagement.Application.Subscriptions.DTOs;
using ExpenseManagement.Application.Subscriptions.Queries;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ExpenseManagement.WebAPI.Controllers;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class SubscriptionsController : ControllerBase
{
    private readonly CreateSubscriptionCommandHandler _createSubscriptionHandler;
    private readonly UpdateSubscriptionCommandHandler _updateSubscriptionHandler;
    private readonly DeleteSubscriptionCommandHandler _deleteSubscriptionHandler;
    private readonly GetSubscriptionsQueryHandler _getSubscriptionsHandler;
    private readonly GetSubscriptionByIdQueryHandler _getSubscriptionByIdHandler;

    public SubscriptionsController(
        CreateSubscriptionCommandHandler createSubscriptionHandler,
        UpdateSubscriptionCommandHandler updateSubscriptionHandler,
        DeleteSubscriptionCommandHandler deleteSubscriptionHandler,
        GetSubscriptionsQueryHandler getSubscriptionsHandler,
        GetSubscriptionByIdQueryHandler getSubscriptionByIdHandler)
    {
        _createSubscriptionHandler = createSubscriptionHandler;
        _updateSubscriptionHandler = updateSubscriptionHandler;
        _deleteSubscriptionHandler = deleteSubscriptionHandler;
        _getSubscriptionsHandler = getSubscriptionsHandler;
        _getSubscriptionByIdHandler = getSubscriptionByIdHandler;
    }

    [HttpGet]
    public async Task<IActionResult> GetSubscriptions([FromQuery] GetSubscriptionsQuery query, CancellationToken cancellationToken)
    {
        var result = await _getSubscriptionsHandler.Handle(query, cancellationToken);
        return Ok(result);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetSubscriptionById(Guid id, CancellationToken cancellationToken)
    {
        var result = await _getSubscriptionByIdHandler.Handle(id, cancellationToken);

        if (!result.Succeeded)
            return NotFound(new { message = string.Join(", ", result.Errors) });

        return Ok(result.Data);
    }

    [HttpPost]
    public async Task<IActionResult> CreateSubscription([FromBody] CreateSubscriptionDto dto, CancellationToken cancellationToken)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var result = await _createSubscriptionHandler.Handle(dto, cancellationToken);

        if (!result.Succeeded)
            return BadRequest(new { message = string.Join(", ", result.Errors) });

        return CreatedAtAction(nameof(GetSubscriptionById), new { id = result.Data }, result.Data);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateSubscription(Guid id, [FromBody] UpdateSubscriptionDto dto, CancellationToken cancellationToken)
    {
        if (id != dto.Id)
            return BadRequest(new { message = "ID mismatch" });

        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var result = await _updateSubscriptionHandler.Handle(dto, cancellationToken);

        if (!result.Succeeded)
            return BadRequest(new { message = string.Join(", ", result.Errors) });

        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteSubscription(Guid id, CancellationToken cancellationToken)
    {
        var result = await _deleteSubscriptionHandler.Handle(id, cancellationToken);

        if (!result.Succeeded)
            return BadRequest(new { message = string.Join(", ", result.Errors) });

        return NoContent();
    }
}
