using ExpenseManagement.Domain.Common;

namespace ExpenseManagement.Domain.Entities;

public class Subscription : AuditableEntity
{
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public decimal Amount { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime? EndDate { get; set; }
    public DateTime NextBillingDate { get; set; }
    public int BillingCycle { get; set; } // Days between billings
    public bool IsActive { get; set; }
    public int ReminderDaysBefore { get; set; } = 3;
    public string UserId { get; set; } = string.Empty;
    public Guid CategoryId { get; set; }
    public Guid? CurrencyId { get; set; }

    // Navigation properties
    public Category Category { get; set; } = null!;
    public Currency? Currency { get; set; }
}
