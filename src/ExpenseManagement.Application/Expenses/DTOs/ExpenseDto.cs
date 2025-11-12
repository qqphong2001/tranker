namespace ExpenseManagement.Application.Expenses.DTOs;

public class ExpenseDto
{
    public Guid Id { get; set; }
    public decimal Amount { get; set; }
    public string? Description { get; set; }
    public DateTime Date { get; set; }
    public string? Notes { get; set; }
    public string? ImageUrl { get; set; }
    public Guid CategoryId { get; set; }
    public string CategoryName { get; set; } = string.Empty;
    public string CategoryColor { get; set; } = string.Empty;
    public string? CategoryIcon { get; set; }
    public Guid? CurrencyId { get; set; }
    public string? CurrencyCode { get; set; }
    public string? CurrencySymbol { get; set; }
    public List<TagDto> Tags { get; set; } = new();
    public DateTime CreatedAt { get; set; }
}

public class TagDto
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? Color { get; set; }
}
