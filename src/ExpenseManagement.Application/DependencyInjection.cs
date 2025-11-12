using System.Reflection;
using ExpenseManagement.Application.Budgets.Commands;
using ExpenseManagement.Application.Dashboard.Queries;
using ExpenseManagement.Application.Expenses.Commands;
using ExpenseManagement.Application.Expenses.Queries;
using ExpenseManagement.Application.Subscriptions.Commands;
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
        services.AddScoped<CreateExpenseCommandHandler>();
        services.AddScoped<UpdateExpenseCommandHandler>();
        services.AddScoped<DeleteExpenseCommandHandler>();
        services.AddScoped<GetExpensesQueryHandler>();
        services.AddScoped<CreateBudgetCommandHandler>();
        services.AddScoped<CreateSubscriptionCommandHandler>();
        services.AddScoped<GetDashboardDataQueryHandler>();

        return services;
    }
}
