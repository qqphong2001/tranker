using ExpenseManagement.Application.Budgets.DTOs;
using ExpenseManagement.Application.Common.Interfaces;
using ExpenseManagement.Application.Common.Models;
using ExpenseManagement.Domain.Enums;
using Microsoft.EntityFrameworkCore;

namespace ExpenseManagement.Application.Budgets.Queries;

public class GetBudgetsQuery
{
    public int PageNumber { get; set; } = 1;
    public int PageSize { get; set; } = 10;
    public Guid? CategoryId { get; set; }
    public BudgetPeriod? Period { get; set; }
    public bool? IsActive { get; set; }
}

public class GetBudgetsQueryHandler
{
    private readonly IApplicationDbContext _context;
    private readonly ICurrentUserService _currentUser;

    public GetBudgetsQueryHandler(IApplicationDbContext context, ICurrentUserService currentUser)
    {
        _context = context;
        _currentUser = currentUser;
    }

    public async Task<PaginatedList<BudgetDto>> Handle(GetBudgetsQuery request, CancellationToken cancellationToken)
    {
        var query = _context.Budgets
            .Include(b => b.Category)
            .Where(b => b.UserId == _currentUser.UserId)
            .AsQueryable();

        // Apply filters
        if (request.CategoryId.HasValue)
            query = query.Where(b => b.CategoryId == request.CategoryId.Value);

        if (request.Period.HasValue)
            query = query.Where(b => b.Period == request.Period.Value);

        if (request.IsActive.HasValue)
            query = query.Where(b => b.IsActive == request.IsActive.Value);

        // Order by start date descending
        query = query.OrderByDescending(b => b.StartDate);

        // Project to DTO
        var dtoQuery = query.Select(b => new BudgetDto
        {
            Id = b.Id,
            Name = b.Name,
            Amount = b.Amount,
            SpentAmount = b.SpentAmount,
            RemainingAmount = b.Amount - b.SpentAmount,
            Percentage = b.Amount > 0 ? (b.SpentAmount / b.Amount) * 100 : 0,
            Period = b.Period,
            StartDate = b.StartDate,
            EndDate = b.EndDate,
            WarningThreshold = b.WarningThreshold,
            IsActive = b.IsActive,
            CategoryId = b.CategoryId,
            CategoryName = b.Category.Name,
            CategoryColor = b.Category.Color,
            IsExceeded = b.SpentAmount > b.Amount,
            IsWarning = b.Amount > 0 && (b.SpentAmount / b.Amount) * 100 >= b.WarningThreshold
        });

        return await PaginatedList<BudgetDto>.CreateAsync(dtoQuery, request.PageNumber, request.PageSize);
    }
}
