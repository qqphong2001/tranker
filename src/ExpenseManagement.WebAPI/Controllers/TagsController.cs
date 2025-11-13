using ExpenseManagement.Application.Tags.Commands;
using ExpenseManagement.Application.Tags.DTOs;
using ExpenseManagement.Application.Tags.Queries;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ExpenseManagement.WebAPI.Controllers;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class TagsController : ControllerBase
{
    private readonly CreateTagCommandHandler _createTagHandler;
    private readonly DeleteTagCommandHandler _deleteTagHandler;
    private readonly GetTagsQueryHandler _getTagsHandler;

    public TagsController(
        CreateTagCommandHandler createTagHandler,
        DeleteTagCommandHandler deleteTagHandler,
        GetTagsQueryHandler getTagsHandler)
    {
        _createTagHandler = createTagHandler;
        _deleteTagHandler = deleteTagHandler;
        _getTagsHandler = getTagsHandler;
    }

    [HttpGet]
    public async Task<IActionResult> GetTags(CancellationToken cancellationToken)
    {
        var result = await _getTagsHandler.Handle(cancellationToken);
        return Ok(result);
    }

    [HttpPost]
    public async Task<IActionResult> CreateTag([FromBody] CreateTagDto dto, CancellationToken cancellationToken)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var result = await _createTagHandler.Handle(dto, cancellationToken);

        if (!result.Succeeded)
            return BadRequest(new { message = string.Join(", ", result.Errors) });

        return CreatedAtAction(nameof(GetTags), new { id = result.Data }, result.Data);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteTag(Guid id, CancellationToken cancellationToken)
    {
        var result = await _deleteTagHandler.Handle(id, cancellationToken);

        if (!result.Succeeded)
            return BadRequest(new { message = string.Join(", ", result.Errors) });

        return NoContent();
    }
}
