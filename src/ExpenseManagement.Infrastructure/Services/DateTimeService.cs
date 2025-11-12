using ExpenseManagement.Application.Common.Interfaces;

namespace ExpenseManagement.Infrastructure.Services;

public class DateTimeService : IDateTime
{
    public DateTime Now => DateTime.Now;
    public DateTime UtcNow => DateTime.UtcNow;
}
