using ExpenseManagement.Domain.Entities;
using ExpenseManagement.Domain.Enums;
using ExpenseManagement.Infrastructure.Data;
using ExpenseManagement.Infrastructure.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace ExpenseManagement.Infrastructure.Seeders;

public class ApplicationDbContextSeed
{
    public static async Task SeedAsync(
        ApplicationDbContext context,
        UserManager<ApplicationUser> userManager,
        RoleManager<IdentityRole> roleManager,
        ILogger logger)
    {
        try
        {
            // Seed Roles
            await SeedRolesAsync(roleManager, logger);

            // Seed Demo User
            var demoUser = await SeedDemoUserAsync(userManager, logger);

            // Seed Currencies
            await SeedCurrenciesAsync(context, logger);

            // Seed Categories
            await SeedCategoriesAsync(context, demoUser?.Id, logger);

            // Seed Sample Data
            if (demoUser != null)
            {
                await SeedSampleDataAsync(context, demoUser.Id, logger);
            }

            await context.SaveChangesAsync();
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "An error occurred while seeding the database");
        }
    }

    private static async Task SeedRolesAsync(RoleManager<IdentityRole> roleManager, ILogger logger)
    {
        string[] roleNames = { "Admin", "User" };

        foreach (var roleName in roleNames)
        {
            if (!await roleManager.RoleExistsAsync(roleName))
            {
                await roleManager.CreateAsync(new IdentityRole(roleName));
                logger.LogInformation("Created role: {RoleName}", roleName);
            }
        }
    }

    private static async Task<ApplicationUser?> SeedDemoUserAsync(UserManager<ApplicationUser> userManager, ILogger logger)
    {
        const string demoEmail = "demo@expensemanager.com";
        const string demoPassword = "Demo@123";

        var demoUser = await userManager.FindByEmailAsync(demoEmail);

        if (demoUser == null)
        {
            demoUser = new ApplicationUser
            {
                UserName = demoEmail,
                Email = demoEmail,
                FullName = "Demo User",
                EmailConfirmed = true,
                CreatedAt = DateTime.UtcNow
            };

            var result = await userManager.CreateAsync(demoUser, demoPassword);

            if (result.Succeeded)
            {
                await userManager.AddToRoleAsync(demoUser, "User");
                logger.LogInformation("Created demo user: {Email}", demoEmail);
            }
            else
            {
                logger.LogError("Failed to create demo user: {Errors}", string.Join(", ", result.Errors.Select(e => e.Description)));
                return null;
            }
        }

        return demoUser;
    }

    private static async Task SeedCurrenciesAsync(ApplicationDbContext context, ILogger logger)
    {
        if (!await context.Currencies.AnyAsync())
        {
            var currencies = new List<Currency>
            {
                new() { Id = Guid.NewGuid(), Code = "VND", Symbol = "₫", Name = "Vietnamese Dong", ExchangeRate = 1.0m, IsDefault = true },
                new() { Id = Guid.NewGuid(), Code = "USD", Symbol = "$", Name = "US Dollar", ExchangeRate = 0.00004m, IsDefault = false },
                new() { Id = Guid.NewGuid(), Code = "EUR", Symbol = "€", Name = "Euro", ExchangeRate = 0.000037m, IsDefault = false },
                new() { Id = Guid.NewGuid(), Code = "GBP", Symbol = "£", Name = "British Pound", ExchangeRate = 0.000032m, IsDefault = false }
            };

            context.Currencies.AddRange(currencies);
            logger.LogInformation("Seeded {Count} currencies", currencies.Count);
        }
    }

    private static async Task SeedCategoriesAsync(ApplicationDbContext context, string? userId, ILogger logger)
    {
        if (userId == null || await context.Categories.AnyAsync(c => c.UserId == userId))
            return;

        var expenseCategories = new List<Category>
        {
            new() { Id = Guid.NewGuid(), Name = "Food & Dining", Icon = "🍔", Color = "#FF6B6B", Type = TransactionType.Expense, IsDefault = true, UserId = userId, CreatedAt = DateTime.UtcNow },
            new() { Id = Guid.NewGuid(), Name = "Transportation", Icon = "🚗", Color = "#4ECDC4", Type = TransactionType.Expense, IsDefault = true, UserId = userId, CreatedAt = DateTime.UtcNow },
            new() { Id = Guid.NewGuid(), Name = "Shopping", Icon = "🛍️", Color = "#95E1D3", Type = TransactionType.Expense, IsDefault = true, UserId = userId, CreatedAt = DateTime.UtcNow },
            new() { Id = Guid.NewGuid(), Name = "Entertainment", Icon = "🎬", Color = "#F38181", Type = TransactionType.Expense, IsDefault = true, UserId = userId, CreatedAt = DateTime.UtcNow },
            new() { Id = Guid.NewGuid(), Name = "Bills & Utilities", Icon = "⚡", Color = "#FFA07A", Type = TransactionType.Expense, IsDefault = true, UserId = userId, CreatedAt = DateTime.UtcNow },
            new() { Id = Guid.NewGuid(), Name = "Healthcare", Icon = "🏥", Color = "#98D8C8", Type = TransactionType.Expense, IsDefault = true, UserId = userId, CreatedAt = DateTime.UtcNow },
            new() { Id = Guid.NewGuid(), Name = "Education", Icon = "📚", Color = "#B4A7D6", Type = TransactionType.Expense, IsDefault = true, UserId = userId, CreatedAt = DateTime.UtcNow }
        };

        var incomeCategories = new List<Category>
        {
            new() { Id = Guid.NewGuid(), Name = "Salary", Icon = "💼", Color = "#56CCF2", Type = TransactionType.Income, IsDefault = true, UserId = userId, CreatedAt = DateTime.UtcNow },
            new() { Id = Guid.NewGuid(), Name = "Freelance", Icon = "💻", Color = "#6FCF97", Type = TransactionType.Income, IsDefault = true, UserId = userId, CreatedAt = DateTime.UtcNow },
            new() { Id = Guid.NewGuid(), Name = "Investment", Icon = "📈", Color = "#F2C94C", Type = TransactionType.Income, IsDefault = true, UserId = userId, CreatedAt = DateTime.UtcNow },
            new() { Id = Guid.NewGuid(), Name = "Other Income", Icon = "💰", Color = "#9B59B6", Type = TransactionType.Income, IsDefault = true, UserId = userId, CreatedAt = DateTime.UtcNow }
        };

        context.Categories.AddRange(expenseCategories);
        context.Categories.AddRange(incomeCategories);

        logger.LogInformation("Seeded {Count} categories for user {UserId}", expenseCategories.Count + incomeCategories.Count, userId);
    }

    private static async Task SeedSampleDataAsync(ApplicationDbContext context, string userId, ILogger logger)
    {
        if (await context.Expenses.AnyAsync(e => e.UserId == userId))
            return;

        var foodCategory = await context.Categories.FirstAsync(c => c.Name == "Food & Dining" && c.UserId == userId);
        var salaryCategory = await context.Categories.FirstAsync(c => c.Name == "Salary" && c.UserId == userId);
        var vndCurrency = await context.Currencies.FirstAsync(c => c.Code == "VND");

        // Sample expenses
        var expenses = new List<Expense>
        {
            new() { Id = Guid.NewGuid(), Amount = 150000, Description = "Lunch at restaurant", Date = DateTime.Now.AddDays(-1), CategoryId = foodCategory.Id, CurrencyId = vndCurrency.Id, UserId = userId, CreatedAt = DateTime.UtcNow },
            new() { Id = Guid.NewGuid(), Amount = 85000, Description = "Coffee with friends", Date = DateTime.Now.AddDays(-2), CategoryId = foodCategory.Id, CurrencyId = vndCurrency.Id, UserId = userId, CreatedAt = DateTime.UtcNow },
            new() { Id = Guid.NewGuid(), Amount = 250000, Description = "Groceries", Date = DateTime.Now.AddDays(-3), CategoryId = foodCategory.Id, CurrencyId = vndCurrency.Id, UserId = userId, CreatedAt = DateTime.UtcNow }
        };

        // Sample income
        var incomes = new List<Income>
        {
            new() { Id = Guid.NewGuid(), Amount = 20000000, Description = "Monthly salary", Date = DateTime.Now.AddDays(-5), Source = "Company ABC", CategoryId = salaryCategory.Id, CurrencyId = vndCurrency.Id, UserId = userId, CreatedAt = DateTime.UtcNow }
        };

        context.Expenses.AddRange(expenses);
        context.Incomes.AddRange(incomes);

        logger.LogInformation("Seeded sample transactions for user {UserId}", userId);
    }
}
