using ExpenseManagement.Application.Common.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace ExpenseManagement.Application.Reports.Queries;

public class ExportExpensesToPdfQuery
{
    public DateTime? StartDate { get; set; }
    public DateTime? EndDate { get; set; }
    public Guid? CategoryId { get; set; }
}

public class ExportExpensesToPdfQueryHandler
{
    private readonly IApplicationDbContext _context;
    private readonly ICurrentUserService _currentUser;
    private readonly IPdfService _pdfService;

    public ExportExpensesToPdfQueryHandler(
        IApplicationDbContext context,
        ICurrentUserService currentUser,
        IPdfService pdfService)
    {
        _context = context;
        _currentUser = currentUser;
        _pdfService = pdfService;
    }

    public async Task<byte[]> Handle(ExportExpensesToPdfQuery request, CancellationToken cancellationToken)
    {
        var query = _context.Expenses
            .Include(e => e.Category)
            .Include(e => e.Currency)
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

        // Prepare data for PDF
        var expenseData = expenses.Select(e => new Dictionary<string, string>
        {
            ["Date"] = e.Date.ToString("yyyy-MM-dd"),
            ["Description"] = e.Description ?? "-",
            ["Category"] = e.Category.Name,
            ["Amount"] = $"{e.Currency?.Symbol ?? "$"}{e.Amount:N2}"
        }).ToList();

        var totalAmount = expenses.Sum(e => e.Amount);
        var title = "Expense Report";
        var periodText = request.StartDate.HasValue && request.EndDate.HasValue
            ? $"Period: {request.StartDate.Value:yyyy-MM-dd} to {request.EndDate.Value:yyyy-MM-dd}"
            : "All Time";

        return _pdfService.GenerateReport(title, periodText, expenseData, totalAmount);
    }
}
