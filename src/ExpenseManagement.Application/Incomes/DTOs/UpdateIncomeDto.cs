namespace ExpenseManagement.Application.Incomes.DTOs;

public class UpdateIncomeDto
{
    public Guid Id { get; set; }
    public decimal Amount { get; set; }
    public string? Description { get; set; }
    public DateTime Date { get; set; }
    public string? Notes { get; set; }
    public string? Source { get; set; }
    public Guid CategoryId { get; set; }
    public Guid? CurrencyId { get; set; }
}
