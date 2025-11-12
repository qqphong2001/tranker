using ExpenseManagement.Application.Common.Interfaces;
using ExpenseManagement.Application.Common.Models;
using ExpenseManagement.Application.Expenses.DTOs;
using Microsoft.EntityFrameworkCore;

namespace ExpenseManagement.Application.Expenses.Queries;

public class GetExpensesQuery
{
    public int PageNumber { get; set; } = 1;
    public int PageSize { get; set; } = 10;
    public Guid? CategoryId { get; set; }
    public DateTime? StartDate { get; set; }
    public DateTime? EndDate { get; set; }
    public string? SearchTerm { get; set; }
    public List<Guid>? TagIds { get; set; }
}

public class GetExpensesQueryHandler
{
    private readonly IApplicationDbContext _context;
    private readonly ICurrentUserService _currentUser;

    public GetExpensesQueryHandler(IApplicationDbContext context, ICurrentUserService currentUser)
    {
        _context = context;
        _currentUser = currentUser;
    }

    public async Task<PaginatedList<ExpenseDto>> Handle(GetExpensesQuery request, CancellationToken cancellationToken)
    {
        var query = _context.Expenses
            .Include(e => e.Category)
            .Include(e => e.Currency)
            .Include(e => e.Tags)
            .Where(e => e.UserId == _currentUser.UserId)
            .AsQueryable();

        // Apply filters
        if (request.CategoryId.HasValue)
            query = query.Where(e => e.CategoryId == request.CategoryId.Value);

        if (request.StartDate.HasValue)
            query = query.Where(e => e.Date >= request.StartDate.Value);

        if (request.EndDate.HasValue)
            query = query.Where(e => e.Date <= request.EndDate.Value);

        if (!string.IsNullOrWhiteSpace(request.SearchTerm))
            query = query.Where(e => e.Description != null && e.Description.Contains(request.SearchTerm));

        if (request.TagIds?.Any() == true)
            query = query.Where(e => e.Tags.Any(t => request.TagIds.Contains(t.Id)));

        // Order by date descending
        query = query.OrderByDescending(e => e.Date);

        // Project to DTO
        var dtoQuery = query.Select(e => new ExpenseDto
        {
            Id = e.Id,
            Amount = e.Amount,
            Description = e.Description,
            Date = e.Date,
            Notes = e.Notes,
            ImageUrl = e.ImageUrl,
            CategoryId = e.CategoryId,
            CategoryName = e.Category.Name,
            CategoryColor = e.Category.Color,
            CategoryIcon = e.Category.Icon,
            CurrencyId = e.CurrencyId,
            CurrencyCode = e.Currency != null ? e.Currency.Code : null,
            CurrencySymbol = e.Currency != null ? e.Currency.Symbol : null,
            Tags = e.Tags.Select(t => new TagDto
            {
                Id = t.Id,
                Name = t.Name,
                Color = t.Color
            }).ToList(),
            CreatedAt = e.CreatedAt
        });

        return await PaginatedList<ExpenseDto>.CreateAsync(dtoQuery, request.PageNumber, request.PageSize);
    }
}
