using ExpenseManagement.Application.Subscriptions.Commands;
using ExpenseManagement.Application.Subscriptions.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ExpenseManagement.WebAPI.Controllers;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class SubscriptionsController : ControllerBase
{
    private readonly CreateSubscriptionCommandHandler _createSubscriptionHandler;

    public SubscriptionsController(CreateSubscriptionCommandHandler createSubscriptionHandler)
    {
        _createSubscriptionHandler = createSubscriptionHandler;
    }

    [HttpPost]
    public async Task<IActionResult> CreateSubscription([FromBody] CreateSubscriptionDto dto, CancellationToken cancellationToken)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var result = await _createSubscriptionHandler.Handle(dto, cancellationToken);

        if (!result.Succeeded)
            return BadRequest(new { message = string.Join(", ", result.Errors) });

        return CreatedAtAction(nameof(CreateSubscription), new { id = result.Data }, result.Data);
    }
}
