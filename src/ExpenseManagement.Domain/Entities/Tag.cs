using ExpenseManagement.Domain.Common;

namespace ExpenseManagement.Domain.Entities;

public class Tag : AuditableEntity
{
    public string Name { get; set; } = string.Empty;
    public string? Color { get; set; }
    public string UserId { get; set; } = string.Empty;

    // Navigation properties
    public ICollection<Expense> Expenses { get; set; } = new List<Expense>();
}
