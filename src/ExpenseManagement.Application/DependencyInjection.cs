using System.Reflection;
using ExpenseManagement.Application.Budgets.Commands;
using ExpenseManagement.Application.Budgets.Queries;
using ExpenseManagement.Application.Categories.Commands;
using ExpenseManagement.Application.Categories.Queries;
using ExpenseManagement.Application.Common.Services;
using ExpenseManagement.Application.Currencies.Commands;
using ExpenseManagement.Application.Currencies.Queries;
using ExpenseManagement.Application.Dashboard.Queries;
using ExpenseManagement.Application.Expenses.Commands;
using ExpenseManagement.Application.Expenses.Queries;
using ExpenseManagement.Application.Incomes.Commands;
using ExpenseManagement.Application.Incomes.Queries;
using ExpenseManagement.Application.Notifications.Commands;
using ExpenseManagement.Application.Notifications.Queries;
using ExpenseManagement.Application.RecurringTransactions.Commands;
using ExpenseManagement.Application.RecurringTransactions.Queries;
using ExpenseManagement.Application.Reports.Queries;
using ExpenseManagement.Application.Subscriptions.Commands;
using ExpenseManagement.Application.Subscriptions.Queries;
using ExpenseManagement.Application.Tags.Commands;
using ExpenseManagement.Application.Tags.Queries;
using ExpenseManagement.Application.Validators;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;

namespace ExpenseManagement.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        // AutoMapper
        services.AddAutoMapper(Assembly.GetExecutingAssembly());

        // FluentValidation
        services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());

        // Services
        services.AddScoped<IBudgetTrackingService, BudgetTrackingService>();

        // Command and Query Handlers
        // Expenses
        services.AddScoped<CreateExpenseCommandHandler>();
        services.AddScoped<UpdateExpenseCommandHandler>();
        services.AddScoped<DeleteExpenseCommandHandler>();
        services.AddScoped<GetExpensesQueryHandler>();

        // Incomes
        services.AddScoped<CreateIncomeCommandHandler>();
        services.AddScoped<UpdateIncomeCommandHandler>();
        services.AddScoped<DeleteIncomeCommandHandler>();
        services.AddScoped<GetIncomesQueryHandler>();
        services.AddScoped<GetIncomeByIdQueryHandler>();

        // Budgets
        services.AddScoped<CreateBudgetCommandHandler>();
        services.AddScoped<UpdateBudgetCommandHandler>();
        services.AddScoped<DeleteBudgetCommandHandler>();
        services.AddScoped<GetBudgetsQueryHandler>();
        services.AddScoped<GetBudgetByIdQueryHandler>();

        // Subscriptions
        services.AddScoped<CreateSubscriptionCommandHandler>();
        services.AddScoped<UpdateSubscriptionCommandHandler>();
        services.AddScoped<DeleteSubscriptionCommandHandler>();
        services.AddScoped<GetSubscriptionsQueryHandler>();
        services.AddScoped<GetSubscriptionByIdQueryHandler>();

        // Categories
        services.AddScoped<CreateCategoryCommandHandler>();
        services.AddScoped<UpdateCategoryCommandHandler>();
        services.AddScoped<DeleteCategoryCommandHandler>();
        services.AddScoped<GetCategoriesQueryHandler>();

        // Tags
        services.AddScoped<CreateTagCommandHandler>();
        services.AddScoped<DeleteTagCommandHandler>();
        services.AddScoped<GetTagsQueryHandler>();

        // Recurring Transactions
        services.AddScoped<CreateRecurringTransactionCommandHandler>();
        services.AddScoped<UpdateRecurringTransactionCommandHandler>();
        services.AddScoped<DeleteRecurringTransactionCommandHandler>();
        services.AddScoped<GetRecurringTransactionsQueryHandler>();

        // Notifications
        services.AddScoped<GetNotificationsQueryHandler>();
        services.AddScoped<GetUnreadNotificationCountQueryHandler>();
        services.AddScoped<MarkNotificationAsReadCommandHandler>();
        services.AddScoped<MarkAllNotificationsAsReadCommandHandler>();
        services.AddScoped<DeleteNotificationCommandHandler>();

        // Currencies
        services.AddScoped<GetCurrenciesQueryHandler>();
        services.AddScoped<UpdateCurrencyExchangeRateCommandHandler>();

        // Dashboard
        services.AddScoped<GetDashboardDataQueryHandler>();

        // Reports
        services.AddScoped<ExportExpensesToPdfQueryHandler>();
        services.AddScoped<ExportExpensesToExcelQueryHandler>();

        return services;
    }
}
