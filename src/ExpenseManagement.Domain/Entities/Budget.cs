using ExpenseManagement.Domain.Common;
using ExpenseManagement.Domain.Enums;

namespace ExpenseManagement.Domain.Entities;

public class Budget : AuditableEntity
{
    public string Name { get; set; } = string.Empty;
    public decimal Amount { get; set; }
    public decimal SpentAmount { get; set; }
    public BudgetPeriod Period { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public decimal WarningThreshold { get; set; } = 80; // Percentage
    public bool IsActive { get; set; }
    public string UserId { get; set; } = string.Empty;
    public Guid CategoryId { get; set; }

    // Navigation properties
    public Category Category { get; set; } = null!;
}
