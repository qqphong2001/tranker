using ExpenseManagement.Application.Common.Interfaces;
using ExpenseManagement.Domain.Entities;
using ExpenseManagement.Domain.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace ExpenseManagement.Infrastructure.BackgroundJobs;

public class RecurringTransactionJob
{
    private readonly IApplicationDbContext _context;
    private readonly IDateTime _dateTime;
    private readonly ILogger<RecurringTransactionJob> _logger;

    public RecurringTransactionJob(
        IApplicationDbContext context,
        IDateTime dateTime,
        ILogger<RecurringTransactionJob> logger)
    {
        _context = context;
        _dateTime = dateTime;
        _logger = logger;
    }

    public async Task ExecuteAsync()
    {
        _logger.LogInformation("Starting recurring transaction job at {Time}", _dateTime.UtcNow);

        try
        {
            var today = _dateTime.Now.Date;

            // Get recurring transactions that are due
            var recurringTransactions = await _context.RecurringTransactions
                .Where(rt => rt.IsActive && rt.NextOccurrence.Date <= today)
                .ToListAsync();

            foreach (var recurring in recurringTransactions)
            {
                // Create the transaction based on type
                if (recurring.TransactionType == TransactionType.Expense)
                {
                    var expense = new Expense
                    {
                        Id = Guid.NewGuid(),
                        Amount = recurring.Amount,
                        Description = recurring.Description,
                        Date = recurring.NextOccurrence,
                        CategoryId = recurring.CategoryId,
                        CurrencyId = recurring.CurrencyId,
                        UserId = recurring.UserId,
                        RecurringTransactionId = recurring.Id,
                        CreatedAt = _dateTime.UtcNow,
                        CreatedBy = "System - Recurring Transaction"
                    };

                    _context.Expenses.Add(expense);
                }
                else if (recurring.TransactionType == TransactionType.Income)
                {
                    var income = new Income
                    {
                        Id = Guid.NewGuid(),
                        Amount = recurring.Amount,
                        Description = recurring.Description,
                        Date = recurring.NextOccurrence,
                        CategoryId = recurring.CategoryId,
                        CurrencyId = recurring.CurrencyId,
                        UserId = recurring.UserId,
                        RecurringTransactionId = recurring.Id,
                        CreatedAt = _dateTime.UtcNow,
                        CreatedBy = "System - Recurring Transaction"
                    };

                    _context.Incomes.Add(income);
                }

                // Update next occurrence
                recurring.NextOccurrence = CalculateNextOccurrence(recurring.NextOccurrence, recurring.RecurrenceType);

                // Check if end date is reached
                if (recurring.EndDate.HasValue && recurring.NextOccurrence > recurring.EndDate.Value)
                {
                    recurring.IsActive = false;
                }

                _logger.LogInformation("Created {Type} from recurring transaction {RecurringId}", recurring.TransactionType, recurring.Id);
            }

            await _context.SaveChangesAsync();

            _logger.LogInformation("Recurring transaction job completed successfully");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error executing recurring transaction job");
        }
    }

    private static DateTime CalculateNextOccurrence(DateTime current, RecurrenceType type)
    {
        return type switch
        {
            RecurrenceType.Daily => current.AddDays(1),
            RecurrenceType.Weekly => current.AddDays(7),
            RecurrenceType.Monthly => current.AddMonths(1),
            RecurrenceType.Yearly => current.AddYears(1),
            _ => current.AddMonths(1)
        };
    }
}
