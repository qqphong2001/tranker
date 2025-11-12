namespace ExpenseManagement.Application.Expenses.DTOs;

public class UpdateExpenseDto
{
    public Guid Id { get; set; }
    public decimal Amount { get; set; }
    public string? Description { get; set; }
    public DateTime Date { get; set; }
    public string? Notes { get; set; }
    public string? ImageUrl { get; set; }
    public Guid CategoryId { get; set; }
    public Guid? CurrencyId { get; set; }
    public List<Guid> TagIds { get; set; } = new();
}
