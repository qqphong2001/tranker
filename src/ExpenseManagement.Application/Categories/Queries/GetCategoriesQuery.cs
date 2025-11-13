using ExpenseManagement.Application.Categories.DTOs;
using ExpenseManagement.Application.Common.Interfaces;
using ExpenseManagement.Domain.Enums;
using Microsoft.EntityFrameworkCore;

namespace ExpenseManagement.Application.Categories.Queries;

public class GetCategoriesQuery
{
    public TransactionType? TransactionType { get; set; }
}

public class GetCategoriesQueryHandler
{
    private readonly IApplicationDbContext _context;
    private readonly ICurrentUserService _currentUser;

    public GetCategoriesQueryHandler(IApplicationDbContext context, ICurrentUserService currentUser)
    {
        _context = context;
        _currentUser = currentUser;
    }

    public async Task<List<CategoryDto>> Handle(GetCategoriesQuery request, CancellationToken cancellationToken)
    {
        var query = _context.Categories
            .Where(c => c.UserId == _currentUser.UserId || c.IsDefault)
            .AsQueryable();

        // Apply filter
        if (request.TransactionType.HasValue)
            query = query.Where(c => c.TransactionType == request.TransactionType.Value);

        // Order by name
        query = query.OrderBy(c => c.Name);

        var categories = await query.Select(c => new CategoryDto
        {
            Id = c.Id,
            Name = c.Name,
            Description = c.Description,
            Icon = c.Icon,
            Color = c.Color,
            TransactionType = c.TransactionType,
            IsDefault = c.IsDefault,
            CreatedAt = c.CreatedAt
        }).ToListAsync(cancellationToken);

        return categories;
    }
}
