using ExpenseManagement.Application.Notifications.Commands;
using ExpenseManagement.Application.Notifications.Queries;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ExpenseManagement.WebAPI.Controllers;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class NotificationsController : ControllerBase
{
    private readonly GetNotificationsQueryHandler _getNotificationsHandler;
    private readonly GetUnreadNotificationCountQueryHandler _getUnreadCountHandler;
    private readonly MarkNotificationAsReadCommandHandler _markAsReadHandler;
    private readonly MarkAllNotificationsAsReadCommandHandler _markAllAsReadHandler;
    private readonly DeleteNotificationCommandHandler _deleteHandler;

    public NotificationsController(
        GetNotificationsQueryHandler getNotificationsHandler,
        GetUnreadNotificationCountQueryHandler getUnreadCountHandler,
        MarkNotificationAsReadCommandHandler markAsReadHandler,
        MarkAllNotificationsAsReadCommandHandler markAllAsReadHandler,
        DeleteNotificationCommandHandler deleteHandler)
    {
        _getNotificationsHandler = getNotificationsHandler;
        _getUnreadCountHandler = getUnreadCountHandler;
        _markAsReadHandler = markAsReadHandler;
        _markAllAsReadHandler = markAllAsReadHandler;
        _deleteHandler = deleteHandler;
    }

    [HttpGet]
    public async Task<IActionResult> GetNotifications([FromQuery] GetNotificationsQuery query, CancellationToken cancellationToken)
    {
        var result = await _getNotificationsHandler.Handle(query, cancellationToken);
        return Ok(result);
    }

    [HttpGet("unread-count")]
    public async Task<IActionResult> GetUnreadCount(CancellationToken cancellationToken)
    {
        var count = await _getUnreadCountHandler.Handle(cancellationToken);
        return Ok(new { count });
    }

    [HttpPut("{id}/mark-as-read")]
    public async Task<IActionResult> MarkAsRead(Guid id, CancellationToken cancellationToken)
    {
        var result = await _markAsReadHandler.Handle(id, cancellationToken);

        if (!result.Succeeded)
            return BadRequest(new { message = string.Join(", ", result.Errors) });

        return NoContent();
    }

    [HttpPut("mark-all-as-read")]
    public async Task<IActionResult> MarkAllAsRead(CancellationToken cancellationToken)
    {
        var result = await _markAllAsReadHandler.Handle(cancellationToken);

        if (!result.Succeeded)
            return BadRequest(new { message = string.Join(", ", result.Errors) });

        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteNotification(Guid id, CancellationToken cancellationToken)
    {
        var result = await _deleteHandler.Handle(id, cancellationToken);

        if (!result.Succeeded)
            return BadRequest(new { message = string.Join(", ", result.Errors) });

        return NoContent();
    }
}
