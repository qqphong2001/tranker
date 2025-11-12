using ExpenseManagement.Domain.Common;

namespace ExpenseManagement.Domain.Entities;

public class Currency : BaseEntity
{
    public string Code { get; set; } = string.Empty; // USD, VND, EUR
    public string Symbol { get; set; } = string.Empty; // $, ₫, €
    public string Name { get; set; } = string.Empty;
    public decimal ExchangeRate { get; set; } = 1.0m; // Relative to base currency
    public bool IsDefault { get; set; }
}
