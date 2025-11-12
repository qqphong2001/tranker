using ExpenseManagement.Application.Dashboard.Queries;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ExpenseManagement.WebAPI.Controllers;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class DashboardController : ControllerBase
{
    private readonly GetDashboardDataQueryHandler _getDashboardDataHandler;

    public DashboardController(GetDashboardDataQueryHandler getDashboardDataHandler)
    {
        _getDashboardDataHandler = getDashboardDataHandler;
    }

    [HttpGet]
    public async Task<IActionResult> GetDashboardData(CancellationToken cancellationToken)
    {
        var result = await _getDashboardDataHandler.Handle(cancellationToken);
        return Ok(result);
    }
}
