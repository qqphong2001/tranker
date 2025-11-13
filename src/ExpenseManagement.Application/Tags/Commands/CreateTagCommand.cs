using ExpenseManagement.Application.Common.Interfaces;
using ExpenseManagement.Application.Common.Models;
using ExpenseManagement.Application.Tags.DTOs;
using ExpenseManagement.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace ExpenseManagement.Application.Tags.Commands;

public class CreateTagCommandHandler
{
    private readonly IApplicationDbContext _context;
    private readonly ICurrentUserService _currentUser;
    private readonly IDateTime _dateTime;

    public CreateTagCommandHandler(
        IApplicationDbContext context,
        ICurrentUserService currentUser,
        IDateTime dateTime)
    {
        _context = context;
        _currentUser = currentUser;
        _dateTime = dateTime;
    }

    public async Task<Result<Guid>> Handle(CreateTagDto request, CancellationToken cancellationToken)
    {
        // Check if tag name already exists for this user
        var exists = await _context.Tags
            .AnyAsync(t => t.UserId == _currentUser.UserId && t.Name.ToLower() == request.Name.ToLower(), cancellationToken);

        if (exists)
            return Result<Guid>.Failure("A tag with this name already exists");

        var tag = new Tag
        {
            Id = Guid.NewGuid(),
            Name = request.Name,
            Color = request.Color,
            UserId = _currentUser.UserId!,
            CreatedAt = _dateTime.UtcNow,
            CreatedBy = _currentUser.Email
        };

        _context.Tags.Add(tag);
        await _context.SaveChangesAsync(cancellationToken);

        return Result<Guid>.Success(tag.Id);
    }
}
