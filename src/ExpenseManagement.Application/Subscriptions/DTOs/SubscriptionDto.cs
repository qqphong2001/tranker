namespace ExpenseManagement.Application.Subscriptions.DTOs;

public class SubscriptionDto
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public decimal Amount { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime? EndDate { get; set; }
    public DateTime NextBillingDate { get; set; }
    public int BillingCycle { get; set; }
    public bool IsActive { get; set; }
    public int ReminderDaysBefore { get; set; }
    public Guid CategoryId { get; set; }
    public string CategoryName { get; set; } = string.Empty;
    public string CategoryColor { get; set; } = string.Empty;
    public Guid? CurrencyId { get; set; }
    public string? CurrencyCode { get; set; }
    public string? CurrencySymbol { get; set; }
}

public class CreateSubscriptionDto
{
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public decimal Amount { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime? EndDate { get; set; }
    public int BillingCycle { get; set; } = 30; // Default monthly
    public int ReminderDaysBefore { get; set; } = 3;
    public Guid CategoryId { get; set; }
    public Guid? CurrencyId { get; set; }
}

public class UpdateSubscriptionDto
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public decimal Amount { get; set; }
    public int BillingCycle { get; set; }
    public int ReminderDaysBefore { get; set; }
    public bool IsActive { get; set; }
    public Guid CategoryId { get; set; }
}
