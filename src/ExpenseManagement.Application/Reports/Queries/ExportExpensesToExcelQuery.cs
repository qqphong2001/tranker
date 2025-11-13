using ExpenseManagement.Application.Common.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace ExpenseManagement.Application.Reports.Queries;

public class ExportExpensesToExcelQuery
{
    public DateTime? StartDate { get; set; }
    public DateTime? EndDate { get; set; }
    public Guid? CategoryId { get; set; }
}

public class ExportExpensesToExcelQueryHandler
{
    private readonly IApplicationDbContext _context;
    private readonly ICurrentUserService _currentUser;
    private readonly IExcelService _excelService;

    public ExportExpensesToExcelQueryHandler(
        IApplicationDbContext context,
        ICurrentUserService currentUser,
        IExcelService excelService)
    {
        _context = context;
        _currentUser = currentUser;
        _excelService = excelService;
    }

    public async Task<byte[]> Handle(ExportExpensesToExcelQuery request, CancellationToken cancellationToken)
    {
        var query = _context.Expenses
            .Include(e => e.Category)
            .Include(e => e.Currency)
            .Include(e => e.Tags)
            .Where(e => e.UserId == _currentUser.UserId)
            .AsQueryable();

        // Apply filters
        if (request.StartDate.HasValue)
            query = query.Where(e => e.Date >= request.StartDate.Value);

        if (request.EndDate.HasValue)
            query = query.Where(e => e.Date <= request.EndDate.Value);

        if (request.CategoryId.HasValue)
            query = query.Where(e => e.CategoryId == request.CategoryId.Value);

        // Order by date descending
        query = query.OrderByDescending(e => e.Date);

        var expenses = await query.ToListAsync(cancellationToken);

        // Prepare data for Excel
        var expenseData = expenses.Select(e => new Dictionary<string, object>
        {
            ["Date"] = e.Date.ToString("yyyy-MM-dd"),
            ["Description"] = e.Description ?? "-",
            ["Category"] = e.Category.Name,
            ["Amount"] = e.Amount,
            ["Currency"] = e.Currency?.Code ?? "USD",
            ["Tags"] = string.Join(", ", e.Tags.Select(t => t.Name)),
            ["Notes"] = e.Notes ?? "-"
        }).ToList();

        var sheetName = "Expenses";
        return _excelService.ExportToExcel(expenseData, sheetName);
    }
}
