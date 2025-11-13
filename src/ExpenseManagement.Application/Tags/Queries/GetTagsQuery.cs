using ExpenseManagement.Application.Common.Interfaces;
using ExpenseManagement.Application.Tags.DTOs;
using Microsoft.EntityFrameworkCore;

namespace ExpenseManagement.Application.Tags.Queries;

public class GetTagsQueryHandler
{
    private readonly IApplicationDbContext _context;
    private readonly ICurrentUserService _currentUser;

    public GetTagsQueryHandler(IApplicationDbContext context, ICurrentUserService currentUser)
    {
        _context = context;
        _currentUser = currentUser;
    }

    public async Task<List<TagDto>> Handle(CancellationToken cancellationToken)
    {
        var tags = await _context.Tags
            .Where(t => t.UserId == _currentUser.UserId)
            .OrderBy(t => t.Name)
            .Select(t => new TagDto
            {
                Id = t.Id,
                Name = t.Name,
                Color = t.Color,
                CreatedAt = t.CreatedAt
            })
            .ToListAsync(cancellationToken);

        return tags;
    }
}
