using ExpenseManagement.Application.Categories.Commands;
using ExpenseManagement.Application.Categories.DTOs;
using ExpenseManagement.Application.Categories.Queries;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ExpenseManagement.WebAPI.Controllers;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class CategoriesController : ControllerBase
{
    private readonly CreateCategoryCommandHandler _createCategoryHandler;
    private readonly UpdateCategoryCommandHandler _updateCategoryHandler;
    private readonly DeleteCategoryCommandHandler _deleteCategoryHandler;
    private readonly GetCategoriesQueryHandler _getCategoriesHandler;

    public CategoriesController(
        CreateCategoryCommandHandler createCategoryHandler,
        UpdateCategoryCommandHandler updateCategoryHandler,
        DeleteCategoryCommandHandler deleteCategoryHandler,
        GetCategoriesQueryHandler getCategoriesHandler)
    {
        _createCategoryHandler = createCategoryHandler;
        _updateCategoryHandler = updateCategoryHandler;
        _deleteCategoryHandler = deleteCategoryHandler;
        _getCategoriesHandler = getCategoriesHandler;
    }

    [HttpGet]
    public async Task<IActionResult> GetCategories([FromQuery] GetCategoriesQuery query, CancellationToken cancellationToken)
    {
        var result = await _getCategoriesHandler.Handle(query, cancellationToken);
        return Ok(result);
    }

    [HttpPost]
    public async Task<IActionResult> CreateCategory([FromBody] CreateCategoryDto dto, CancellationToken cancellationToken)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var result = await _createCategoryHandler.Handle(dto, cancellationToken);

        if (!result.Succeeded)
            return BadRequest(new { message = string.Join(", ", result.Errors) });

        return CreatedAtAction(nameof(GetCategories), new { id = result.Data }, result.Data);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateCategory(Guid id, [FromBody] UpdateCategoryDto dto, CancellationToken cancellationToken)
    {
        if (id != dto.Id)
            return BadRequest(new { message = "ID mismatch" });

        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var result = await _updateCategoryHandler.Handle(dto, cancellationToken);

        if (!result.Succeeded)
            return BadRequest(new { message = string.Join(", ", result.Errors) });

        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteCategory(Guid id, CancellationToken cancellationToken)
    {
        var result = await _deleteCategoryHandler.Handle(id, cancellationToken);

        if (!result.Succeeded)
            return BadRequest(new { message = string.Join(", ", result.Errors) });

        return NoContent();
    }
}
