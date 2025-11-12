using ExpenseManagement.Domain.Common;

namespace ExpenseManagement.Domain.Entities;

public class Income : AuditableEntity
{
    public decimal Amount { get; set; }
    public string? Description { get; set; }
    public DateTime Date { get; set; }
    public string? Notes { get; set; }
    public string? Source { get; set; }
    public string UserId { get; set; } = string.Empty;
    public Guid CategoryId { get; set; }
    public Guid? CurrencyId { get; set; }
    public Guid? RecurringTransactionId { get; set; }

    // Navigation properties
    public Category Category { get; set; } = null!;
    public Currency? Currency { get; set; }
    public RecurringTransaction? RecurringTransaction { get; set; }
}
