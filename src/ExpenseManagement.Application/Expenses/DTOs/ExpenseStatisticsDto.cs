namespace ExpenseManagement.Application.Expenses.DTOs;

public class ExpenseStatisticsDto
{
    public decimal TotalExpenses { get; set; }
    public int TransactionCount { get; set; }
    public decimal AverageExpense { get; set; }
    public List<CategoryExpenseDto> ExpensesByCategory { get; set; } = new();
    public List<MonthlyExpenseDto> MonthlyTrend { get; set; } = new();
}

public class CategoryExpenseDto
{
    public Guid CategoryId { get; set; }
    public string CategoryName { get; set; } = string.Empty;
    public string CategoryColor { get; set; } = string.Empty;
    public decimal Amount { get; set; }
    public int Count { get; set; }
    public decimal Percentage { get; set; }
}

public class MonthlyExpenseDto
{
    public int Year { get; set; }
    public int Month { get; set; }
    public string MonthName { get; set; } = string.Empty;
    public decimal Amount { get; set; }
    public int Count { get; set; }
}
