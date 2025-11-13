using ExpenseManagement.Domain.Enums;

namespace ExpenseManagement.Application.RecurringTransactions.DTOs;

public class RecurringTransactionDto
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public decimal Amount { get; set; }
    public string? Description { get; set; }
    public RecurrenceType RecurrenceType { get; set; }
    public TransactionType TransactionType { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime? EndDate { get; set; }
    public DateTime NextOccurrence { get; set; }
    public bool IsActive { get; set; }
    public Guid CategoryId { get; set; }
    public string CategoryName { get; set; } = string.Empty;
    public string CategoryColor { get; set; } = string.Empty;
    public Guid? CurrencyId { get; set; }
    public string? CurrencyCode { get; set; }
    public string? CurrencySymbol { get; set; }
}

public class CreateRecurringTransactionDto
{
    public string Name { get; set; } = string.Empty;
    public decimal Amount { get; set; }
    public string? Description { get; set; }
    public RecurrenceType RecurrenceType { get; set; }
    public TransactionType TransactionType { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime? EndDate { get; set; }
    public Guid CategoryId { get; set; }
    public Guid? CurrencyId { get; set; }
}

public class UpdateRecurringTransactionDto
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public decimal Amount { get; set; }
    public string? Description { get; set; }
    public RecurrenceType RecurrenceType { get; set; }
    public bool IsActive { get; set; }
    public Guid CategoryId { get; set; }
}
