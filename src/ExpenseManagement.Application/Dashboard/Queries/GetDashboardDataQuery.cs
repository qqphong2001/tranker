using ExpenseManagement.Application.Common.Interfaces;
using ExpenseManagement.Application.Dashboard.DTOs;
using Microsoft.EntityFrameworkCore;
using System.Globalization;

namespace ExpenseManagement.Application.Dashboard.Queries;

public class GetDashboardDataQueryHandler
{
    private readonly IApplicationDbContext _context;
    private readonly ICurrentUserService _currentUser;
    private readonly IDateTime _dateTime;

    public GetDashboardDataQueryHandler(
        IApplicationDbContext context,
        ICurrentUserService currentUser,
        IDateTime dateTime)
    {
        _context = context;
        _currentUser = currentUser;
        _dateTime = dateTime;
    }

    public async Task<DashboardDto> Handle(CancellationToken cancellationToken)
    {
        var now = _dateTime.Now;
        var currentMonthStart = new DateTime(now.Year, now.Month, 1);
        var currentMonthEnd = currentMonthStart.AddMonths(1).AddDays(-1);

        // Get all-time totals
        var totalIncome = await _context.Incomes
            .Where(i => i.UserId == _currentUser.UserId)
            .SumAsync(i => i.Amount, cancellationToken);

        var totalExpenses = await _context.Expenses
            .Where(e => e.UserId == _currentUser.UserId)
            .SumAsync(e => e.Amount, cancellationToken);

        // Get monthly totals
        var monthlyIncome = await _context.Incomes
            .Where(i => i.UserId == _currentUser.UserId && i.Date >= currentMonthStart && i.Date <= currentMonthEnd)
            .SumAsync(i => i.Amount, cancellationToken);

        var monthlyExpenses = await _context.Expenses
            .Where(e => e.UserId == _currentUser.UserId && e.Date >= currentMonthStart && e.Date <= currentMonthEnd)
            .SumAsync(e => e.Amount, cancellationToken);

        // Get top expense categories (current month)
        var topExpenseCategories = await _context.Expenses
            .Where(e => e.UserId == _currentUser.UserId && e.Date >= currentMonthStart && e.Date <= currentMonthEnd)
            .GroupBy(e => new { e.CategoryId, e.Category.Name, e.Category.Color, e.Category.Icon })
            .Select(g => new CategorySummaryDto
            {
                CategoryId = g.Key.CategoryId,
                CategoryName = g.Key.Name,
                CategoryColor = g.Key.Color,
                CategoryIcon = g.Key.Icon,
                Amount = g.Sum(e => e.Amount),
                Count = g.Count()
            })
            .OrderByDescending(c => c.Amount)
            .Take(5)
            .ToListAsync(cancellationToken);

        // Calculate percentages
        if (monthlyExpenses > 0)
        {
            foreach (var category in topExpenseCategories)
            {
                category.Percentage = Math.Round((category.Amount / monthlyExpenses) * 100, 2);
            }
        }

        // Get monthly trend (last 6 months)
        var sixMonthsAgo = currentMonthStart.AddMonths(-5);
        var monthlyTrend = await GetMonthlyTrend(sixMonthsAgo, currentMonthEnd, cancellationToken);

        // Get recent transactions
        var recentExpenses = await _context.Expenses
            .Include(e => e.Category)
            .Where(e => e.UserId == _currentUser.UserId)
            .OrderByDescending(e => e.Date)
            .Take(5)
            .Select(e => new RecentTransactionDto
            {
                Id = e.Id,
                Type = "Expense",
                Amount = e.Amount,
                Description = e.Description,
                Date = e.Date,
                CategoryName = e.Category.Name,
                CategoryColor = e.Category.Color
            })
            .ToListAsync(cancellationToken);

        var recentIncomes = await _context.Incomes
            .Include(i => i.Category)
            .Where(i => i.UserId == _currentUser.UserId)
            .OrderByDescending(i => i.Date)
            .Take(5)
            .Select(i => new RecentTransactionDto
            {
                Id = i.Id,
                Type = "Income",
                Amount = i.Amount,
                Description = i.Description,
                Date = i.Date,
                CategoryName = i.Category.Name,
                CategoryColor = i.Category.Color
            })
            .ToListAsync(cancellationToken);

        var recentTransactions = recentExpenses.Concat(recentIncomes)
            .OrderByDescending(t => t.Date)
            .Take(10)
            .ToList();

        // Get budget progress
        var budgetProgress = await _context.Budgets
            .Include(b => b.Category)
            .Where(b => b.UserId == _currentUser.UserId && b.IsActive && b.StartDate <= now && b.EndDate >= now)
            .Select(b => new BudgetProgressDto
            {
                BudgetId = b.Id,
                BudgetName = b.Name,
                CategoryName = b.Category.Name,
                BudgetAmount = b.Amount,
                SpentAmount = b.SpentAmount,
                RemainingAmount = b.Amount - b.SpentAmount,
                Percentage = b.Amount > 0 ? Math.Round((b.SpentAmount / b.Amount) * 100, 2) : 0,
                IsExceeded = b.SpentAmount > b.Amount,
                IsWarning = b.Amount > 0 && (b.SpentAmount / b.Amount) * 100 >= b.WarningThreshold
            })
            .ToListAsync(cancellationToken);

        // Get upcoming subscriptions (next 30 days)
        var next30Days = now.AddDays(30);
        var upcomingSubscriptions = await _context.Subscriptions
            .Include(s => s.Category)
            .Where(s => s.UserId == _currentUser.UserId && s.IsActive && s.NextBillingDate <= next30Days)
            .OrderBy(s => s.NextBillingDate)
            .Select(s => new UpcomingSubscriptionDto
            {
                SubscriptionId = s.Id,
                Name = s.Name,
                Amount = s.Amount,
                NextBillingDate = s.NextBillingDate,
                DaysUntilBilling = (int)(s.NextBillingDate - now).TotalDays,
                CategoryName = s.Category.Name
            })
            .Take(5)
            .ToListAsync(cancellationToken);

        return new DashboardDto
        {
            TotalIncome = totalIncome,
            TotalExpenses = totalExpenses,
            Balance = totalIncome - totalExpenses,
            MonthlyIncome = monthlyIncome,
            MonthlyExpenses = monthlyExpenses,
            MonthlyBalance = monthlyIncome - monthlyExpenses,
            TopExpenseCategories = topExpenseCategories,
            MonthlyTrend = monthlyTrend,
            RecentTransactions = recentTransactions,
            BudgetProgress = budgetProgress,
            UpcomingSubscriptions = upcomingSubscriptions
        };
    }

    private async Task<List<MonthlyTrendDto>> GetMonthlyTrend(DateTime startDate, DateTime endDate, CancellationToken cancellationToken)
    {
        var months = new List<MonthlyTrendDto>();
        var current = startDate;

        while (current <= endDate)
        {
            var monthStart = new DateTime(current.Year, current.Month, 1);
            var monthEnd = monthStart.AddMonths(1).AddDays(-1);

            var income = await _context.Incomes
                .Where(i => i.UserId == _currentUser.UserId && i.Date >= monthStart && i.Date <= monthEnd)
                .SumAsync(i => i.Amount, cancellationToken);

            var expenses = await _context.Expenses
                .Where(e => e.UserId == _currentUser.UserId && e.Date >= monthStart && e.Date <= monthEnd)
                .SumAsync(e => e.Amount, cancellationToken);

            months.Add(new MonthlyTrendDto
            {
                Year = current.Year,
                Month = current.Month,
                MonthName = current.ToString("MMM yyyy", CultureInfo.InvariantCulture),
                Income = income,
                Expenses = expenses,
                Balance = income - expenses
            });

            current = current.AddMonths(1);
        }

        return months;
    }
}
