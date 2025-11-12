using ExpenseManagement.Domain.Common;
using ExpenseManagement.Domain.Enums;

namespace ExpenseManagement.Domain.Entities;

public class RecurringTransaction : AuditableEntity
{
    public string Name { get; set; } = string.Empty;
    public decimal Amount { get; set; }
    public string? Description { get; set; }
    public RecurrenceType RecurrenceType { get; set; }
    public TransactionType TransactionType { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime? EndDate { get; set; }
    public DateTime NextOccurrence { get; set; }
    public bool IsActive { get; set; }
    public string UserId { get; set; } = string.Empty;
    public Guid CategoryId { get; set; }
    public Guid? CurrencyId { get; set; }

    // Navigation properties
    public Category Category { get; set; } = null!;
    public Currency? Currency { get; set; }
    public ICollection<Expense> Expenses { get; set; } = new List<Expense>();
    public ICollection<Income> Incomes { get; set; } = new List<Income>();
}
