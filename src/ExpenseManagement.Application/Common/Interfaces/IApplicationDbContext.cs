using ExpenseManagement.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace ExpenseManagement.Application.Common.Interfaces;

public interface IApplicationDbContext
{
    DbSet<Expense> Expenses { get; }
    DbSet<Income> Incomes { get; }
    DbSet<Category> Categories { get; }
    DbSet<Budget> Budgets { get; }
    DbSet<Subscription> Subscriptions { get; }
    DbSet<RecurringTransaction> RecurringTransactions { get; }
    DbSet<Notification> Notifications { get; }
    DbSet<Tag> Tags { get; }
    DbSet<Currency> Currencies { get; }

    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}
