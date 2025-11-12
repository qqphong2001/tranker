using ExpenseManagement.Domain.Common;
using ExpenseManagement.Domain.Enums;

namespace ExpenseManagement.Domain.Entities;

public class Category : AuditableEntity
{
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public string? Icon { get; set; }
    public string Color { get; set; } = "#000000";
    public TransactionType Type { get; set; }
    public bool IsDefault { get; set; }
    public string UserId { get; set; } = string.Empty;

    // Navigation properties
    public ICollection<Expense> Expenses { get; set; } = new List<Expense>();
    public ICollection<Income> Incomes { get; set; } = new List<Income>();
    public ICollection<Budget> Budgets { get; set; } = new List<Budget>();
}
