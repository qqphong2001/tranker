using ExpenseManagement.Application.Categories.DTOs;
using ExpenseManagement.Application.Common.Interfaces;
using ExpenseManagement.Application.Common.Models;
using Microsoft.EntityFrameworkCore;

namespace ExpenseManagement.Application.Categories.Commands;

public class UpdateCategoryCommandHandler
{
    private readonly IApplicationDbContext _context;
    private readonly ICurrentUserService _currentUser;
    private readonly IDateTime _dateTime;

    public UpdateCategoryCommandHandler(
        IApplicationDbContext context,
        ICurrentUserService currentUser,
        IDateTime dateTime)
    {
        _context = context;
        _currentUser = currentUser;
        _dateTime = dateTime;
    }

    public async Task<Result> Handle(UpdateCategoryDto request, CancellationToken cancellationToken)
    {
        var category = await _context.Categories
            .FirstOrDefaultAsync(c => c.Id == request.Id && c.UserId == _currentUser.UserId, cancellationToken);

        if (category == null)
            return Result.Failure("Category not found");

        if (category.IsDefault)
            return Result.Failure("Cannot update default category");

        category.Name = request.Name;
        category.Description = request.Description;
        category.Icon = request.Icon;
        category.Color = request.Color;
        category.UpdatedAt = _dateTime.UtcNow;
        category.UpdatedBy = _currentUser.Email;

        await _context.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}
