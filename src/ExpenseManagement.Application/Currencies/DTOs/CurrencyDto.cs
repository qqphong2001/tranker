namespace ExpenseManagement.Application.Currencies.DTOs;

public class CurrencyDto
{
    public Guid Id { get; set; }
    public string Code { get; set; } = string.Empty;
    public string Symbol { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public decimal ExchangeRate { get; set; }
    public bool IsDefault { get; set; }
}

public class UpdateCurrencyExchangeRateDto
{
    public Guid Id { get; set; }
    public decimal ExchangeRate { get; set; }
}
