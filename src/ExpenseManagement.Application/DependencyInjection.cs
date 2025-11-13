using System.Reflection;
using ExpenseManagement.Application.Budgets.Commands;
using ExpenseManagement.Application.Budgets.Queries;
using ExpenseManagement.Application.Categories.Commands;
using ExpenseManagement.Application.Categories.Queries;
using ExpenseManagement.Application.Dashboard.Queries;
using ExpenseManagement.Application.Expenses.Commands;
using ExpenseManagement.Application.Expenses.Queries;
using ExpenseManagement.Application.Incomes.Commands;
using ExpenseManagement.Application.Incomes.Queries;
using ExpenseManagement.Application.Subscriptions.Commands;
using ExpenseManagement.Application.Subscriptions.Queries;
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

        // Dashboard
        services.AddScoped<GetDashboardDataQueryHandler>();

        return services;
    }
}
