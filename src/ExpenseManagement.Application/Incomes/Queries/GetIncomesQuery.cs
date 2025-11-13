using ExpenseManagement.Application.Common.Interfaces;
using ExpenseManagement.Application.Common.Models;
using ExpenseManagement.Application.Incomes.DTOs;
using Microsoft.EntityFrameworkCore;

namespace ExpenseManagement.Application.Incomes.Queries;

public class GetIncomesQuery
{
    public int PageNumber { get; set; } = 1;
    public int PageSize { get; set; } = 10;
    public Guid? CategoryId { get; set; }
    public DateTime? StartDate { get; set; }
    public DateTime? EndDate { get; set; }
    public string? SearchTerm { get; set; }
}

public class GetIncomesQueryHandler
{
    private readonly IApplicationDbContext _context;
    private readonly ICurrentUserService _currentUser;

    public GetIncomesQueryHandler(IApplicationDbContext context, ICurrentUserService currentUser)
    {
        _context = context;
        _currentUser = currentUser;
    }

    public async Task<PaginatedList<IncomeDto>> Handle(GetIncomesQuery request, CancellationToken cancellationToken)
    {
        var query = _context.Incomes
            .Include(i => i.Category)
            .Include(i => i.Currency)
            .Where(i => i.UserId == _currentUser.UserId)
            .AsQueryable();

        // Apply filters
        if (request.CategoryId.HasValue)
            query = query.Where(i => i.CategoryId == request.CategoryId.Value);

        if (request.StartDate.HasValue)
            query = query.Where(i => i.Date >= request.StartDate.Value);

        if (request.EndDate.HasValue)
            query = query.Where(i => i.Date <= request.EndDate.Value);

        if (!string.IsNullOrWhiteSpace(request.SearchTerm))
            query = query.Where(i => (i.Description != null && i.Description.Contains(request.SearchTerm)) ||
                                    (i.Source != null && i.Source.Contains(request.SearchTerm)));

        // Order by date descending
        query = query.OrderByDescending(i => i.Date);

        // Project to DTO
        var dtoQuery = query.Select(i => new IncomeDto
        {
            Id = i.Id,
            Amount = i.Amount,
            Description = i.Description,
            Date = i.Date,
            Notes = i.Notes,
            Source = i.Source,
            CategoryId = i.CategoryId,
            CategoryName = i.Category.Name,
            CategoryColor = i.Category.Color,
            CategoryIcon = i.Category.Icon,
            CurrencyId = i.CurrencyId,
            CurrencyCode = i.Currency != null ? i.Currency.Code : null,
            CurrencySymbol = i.Currency != null ? i.Currency.Symbol : null,
            CreatedAt = i.CreatedAt
        });

        return await PaginatedList<IncomeDto>.CreateAsync(dtoQuery, request.PageNumber, request.PageSize);
    }
}
