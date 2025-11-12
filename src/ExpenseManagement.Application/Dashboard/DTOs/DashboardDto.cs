namespace ExpenseManagement.Application.Dashboard.DTOs;

public class DashboardDto
{
    public decimal TotalIncome { get; set; }
    public decimal TotalExpenses { get; set; }
    public decimal Balance { get; set; }
    public decimal MonthlyIncome { get; set; }
    public decimal MonthlyExpenses { get; set; }
    public decimal MonthlyBalance { get; set; }
    public List<CategorySummaryDto> TopExpenseCategories { get; set; } = new();
    public List<CategorySummaryDto> TopIncomeCategories { get; set; } = new();
    public List<MonthlyTrendDto> MonthlyTrend { get; set; } = new();
    public List<RecentTransactionDto> RecentTransactions { get; set; } = new();
    public List<BudgetProgressDto> BudgetProgress { get; set; } = new();
    public List<UpcomingSubscriptionDto> UpcomingSubscriptions { get; set; } = new();
}

public class CategorySummaryDto
{
    public Guid CategoryId { get; set; }
    public string CategoryName { get; set; } = string.Empty;
    public string CategoryColor { get; set; } = string.Empty;
    public string? CategoryIcon { get; set; }
    public decimal Amount { get; set; }
    public int Count { get; set; }
    public decimal Percentage { get; set; }
}

public class MonthlyTrendDto
{
    public int Year { get; set; }
    public int Month { get; set; }
    public string MonthName { get; set; } = string.Empty;
    public decimal Income { get; set; }
    public decimal Expenses { get; set; }
    public decimal Balance { get; set; }
}

public class RecentTransactionDto
{
    public Guid Id { get; set; }
    public string Type { get; set; } = string.Empty; // Income or Expense
    public decimal Amount { get; set; }
    public string? Description { get; set; }
    public DateTime Date { get; set; }
    public string CategoryName { get; set; } = string.Empty;
    public string CategoryColor { get; set; } = string.Empty;
}

public class BudgetProgressDto
{
    public Guid BudgetId { get; set; }
    public string BudgetName { get; set; } = string.Empty;
    public string CategoryName { get; set; } = string.Empty;
    public decimal BudgetAmount { get; set; }
    public decimal SpentAmount { get; set; }
    public decimal RemainingAmount { get; set; }
    public decimal Percentage { get; set; }
    public bool IsExceeded { get; set; }
    public bool IsWarning { get; set; }
}

public class UpcomingSubscriptionDto
{
    public Guid SubscriptionId { get; set; }
    public string Name { get; set; } = string.Empty;
    public decimal Amount { get; set; }
    public DateTime NextBillingDate { get; set; }
    public int DaysUntilBilling { get; set; }
    public string CategoryName { get; set; } = string.Empty;
}
