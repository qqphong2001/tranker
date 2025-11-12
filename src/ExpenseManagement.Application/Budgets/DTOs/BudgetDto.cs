using ExpenseManagement.Domain.Enums;

namespace ExpenseManagement.Application.Budgets.DTOs;

public class BudgetDto
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public decimal Amount { get; set; }
    public decimal SpentAmount { get; set; }
    public decimal RemainingAmount { get; set; }
    public decimal Percentage { get; set; }
    public BudgetPeriod Period { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public decimal WarningThreshold { get; set; }
    public bool IsActive { get; set; }
    public Guid CategoryId { get; set; }
    public string CategoryName { get; set; } = string.Empty;
    public string CategoryColor { get; set; } = string.Empty;
    public bool IsExceeded { get; set; }
    public bool IsWarning { get; set; }
}

public class CreateBudgetDto
{
    public string Name { get; set; } = string.Empty;
    public decimal Amount { get; set; }
    public BudgetPeriod Period { get; set; }
    public DateTime StartDate { get; set; }
    public decimal WarningThreshold { get; set; } = 80;
    public Guid CategoryId { get; set; }
}

public class UpdateBudgetDto
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public decimal Amount { get; set; }
    public decimal WarningThreshold { get; set; }
    public bool IsActive { get; set; }
}
