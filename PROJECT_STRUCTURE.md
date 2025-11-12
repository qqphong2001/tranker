# 📁 Project Structure

## Backend: ExpenseManagement.API

```
ExpenseManagement/
├── src/
│   ├── ExpenseManagement.Domain/
│   │   ├── Entities/
│   │   │   ├── User.cs
│   │   │   ├── Expense.cs
│   │   │   ├── Income.cs
│   │   │   ├── Category.cs
│   │   │   ├── Subscription.cs
│   │   │   ├── Budget.cs
│   │   │   ├── RecurringTransaction.cs
│   │   │   ├── Notification.cs
│   │   │   ├── Tag.cs
│   │   │   └── Currency.cs
│   │   ├── Enums/
│   │   │   ├── TransactionType.cs
│   │   │   ├── RecurrenceType.cs
│   │   │   ├── NotificationType.cs
│   │   │   └── BudgetPeriod.cs
│   │   ├── Common/
│   │   │   ├── BaseEntity.cs
│   │   │   └── AuditableEntity.cs
│   │   └── ExpenseManagement.Domain.csproj
│   │
│   ├── ExpenseManagement.Application/
│   │   ├── Common/
│   │   │   ├── Interfaces/
│   │   │   │   ├── IApplicationDbContext.cs
│   │   │   │   ├── IEmailService.cs
│   │   │   │   ├── IPdfService.cs
│   │   │   │   ├── IExcelService.cs
│   │   │   │   ├── INotificationService.cs
│   │   │   │   ├── ICurrentUserService.cs
│   │   │   │   └── IDateTime.cs
│   │   │   ├── Models/
│   │   │   │   ├── Result.cs
│   │   │   │   ├── PaginatedList.cs
│   │   │   │   └── EmailRequest.cs
│   │   │   └── Mappings/
│   │   │       └── MappingProfile.cs
│   │   ├── Authentication/
│   │   │   ├── Commands/
│   │   │   │   ├── RegisterCommand.cs
│   │   │   │   ├── LoginCommand.cs
│   │   │   │   └── RefreshTokenCommand.cs
│   │   │   ├── Queries/
│   │   │   │   └── GetCurrentUserQuery.cs
│   │   │   └── DTOs/
│   │   │       ├── AuthResponse.cs
│   │   │       ├── RegisterDto.cs
│   │   │       └── LoginDto.cs
│   │   ├── Expenses/
│   │   │   ├── Commands/
│   │   │   │   ├── CreateExpenseCommand.cs
│   │   │   │   ├── UpdateExpenseCommand.cs
│   │   │   │   └── DeleteExpenseCommand.cs
│   │   │   ├── Queries/
│   │   │   │   ├── GetExpensesQuery.cs
│   │   │   │   ├── GetExpenseByIdQuery.cs
│   │   │   │   └── GetExpenseStatisticsQuery.cs
│   │   │   └── DTOs/
│   │   │       ├── ExpenseDto.cs
│   │   │       └── ExpenseStatisticsDto.cs
│   │   ├── Incomes/
│   │   │   ├── Commands/
│   │   │   ├── Queries/
│   │   │   └── DTOs/
│   │   ├── Subscriptions/
│   │   │   ├── Commands/
│   │   │   ├── Queries/
│   │   │   └── DTOs/
│   │   ├── Budgets/
│   │   │   ├── Commands/
│   │   │   ├── Queries/
│   │   │   └── DTOs/
│   │   ├── Reports/
│   │   │   ├── Queries/
│   │   │   │   ├── GenerateMonthlyReportQuery.cs
│   │   │   │   └── GenerateYearlyReportQuery.cs
│   │   │   └── DTOs/
│   │   │       └── ReportDto.cs
│   │   ├── Categories/
│   │   │   ├── Queries/
│   │   │   └── DTOs/
│   │   ├── Dashboard/
│   │   │   ├── Queries/
│   │   │   │   └── GetDashboardDataQuery.cs
│   │   │   └── DTOs/
│   │   │       └── DashboardDto.cs
│   │   ├── Validators/
│   │   │   ├── CreateExpenseValidator.cs
│   │   │   ├── CreateBudgetValidator.cs
│   │   │   └── RegisterValidator.cs
│   │   ├── DependencyInjection.cs
│   │   └── ExpenseManagement.Application.csproj
│   │
│   ├── ExpenseManagement.Infrastructure/
│   │   ├── Data/
│   │   │   ├── ApplicationDbContext.cs
│   │   │   ├── Configurations/
│   │   │   │   ├── ExpenseConfiguration.cs
│   │   │   │   ├── IncomeConfiguration.cs
│   │   │   │   ├── BudgetConfiguration.cs
│   │   │   │   └── SubscriptionConfiguration.cs
│   │   │   └── Migrations/
│   │   ├── Identity/
│   │   │   ├── ApplicationUser.cs
│   │   │   ├── IdentityService.cs
│   │   │   └── JwtTokenService.cs
│   │   ├── Services/
│   │   │   ├── EmailService.cs
│   │   │   ├── PdfService.cs
│   │   │   ├── ExcelService.cs
│   │   │   ├── NotificationService.cs
│   │   │   ├── DateTimeService.cs
│   │   │   └── CurrentUserService.cs
│   │   ├── BackgroundJobs/
│   │   │   ├── SubscriptionReminderJob.cs
│   │   │   └── RecurringTransactionJob.cs
│   │   ├── Seeders/
│   │   │   └── ApplicationDbContextSeed.cs
│   │   ├── DependencyInjection.cs
│   │   └── ExpenseManagement.Infrastructure.csproj
│   │
│   └── ExpenseManagement.WebAPI/
│       ├── Controllers/
│       │   ├── AuthController.cs
│       │   ├── ExpensesController.cs
│       │   ├── IncomesController.cs
│       │   ├── SubscriptionsController.cs
│       │   ├── BudgetsController.cs
│       │   ├── ReportsController.cs
│       │   ├── DashboardController.cs
│       │   └── CategoriesController.cs
│       ├── Middleware/
│       │   ├── ErrorHandlerMiddleware.cs
│       │   └── RequestLoggingMiddleware.cs
│       ├── Filters/
│       │   └── ApiExceptionFilterAttribute.cs
│       ├── Extensions/
│       │   └── ServiceCollectionExtensions.cs
│       ├── appsettings.json
│       ├── appsettings.Development.json
│       ├── Program.cs
│       ├── Dockerfile
│       └── ExpenseManagement.WebAPI.csproj
│
├── tests/
│   ├── ExpenseManagement.Application.Tests/
│   └── ExpenseManagement.Infrastructure.Tests/
│
└── ExpenseManagement.sln
```

## Frontend: expense-management-ui

```
expense-management-ui/
├── public/
│   └── vite.svg
├── src/
│   ├── app/
│   │   ├── store.ts
│   │   └── hooks.ts
│   ├── features/
│   │   ├── auth/
│   │   │   ├── authSlice.ts
│   │   │   ├── authApi.ts
│   │   │   ├── Login.tsx
│   │   │   ├── Register.tsx
│   │   │   └── ForgotPassword.tsx
│   │   ├── dashboard/
│   │   │   ├── Dashboard.tsx
│   │   │   ├── components/
│   │   │   │   ├── StatCard.tsx
│   │   │   │   ├── ExpenseChart.tsx
│   │   │   │   ├── IncomeExpenseChart.tsx
│   │   │   │   └── CategoryPieChart.tsx
│   │   │   └── dashboardApi.ts
│   │   ├── expenses/
│   │   │   ├── ExpenseList.tsx
│   │   │   ├── ExpenseForm.tsx
│   │   │   ├── ExpenseDetails.tsx
│   │   │   ├── components/
│   │   │   │   ├── ExpenseCard.tsx
│   │   │   │   ├── ExpenseFilter.tsx
│   │   │   │   └── ExpenseTable.tsx
│   │   │   └── expenseApi.ts
│   │   ├── incomes/
│   │   │   ├── IncomeList.tsx
│   │   │   ├── IncomeForm.tsx
│   │   │   └── incomeApi.ts
│   │   ├── subscriptions/
│   │   │   ├── SubscriptionList.tsx
│   │   │   ├── SubscriptionForm.tsx
│   │   │   ├── components/
│   │   │   │   └── SubscriptionCard.tsx
│   │   │   └── subscriptionApi.ts
│   │   ├── budgets/
│   │   │   ├── BudgetList.tsx
│   │   │   ├── BudgetForm.tsx
│   │   │   ├── components/
│   │   │   │   ├── BudgetProgress.tsx
│   │   │   │   └── BudgetAlert.tsx
│   │   │   └── budgetApi.ts
│   │   ├── reports/
│   │   │   ├── Reports.tsx
│   │   │   ├── components/
│   │   │   │   ├── ReportFilters.tsx
│   │   │   │   └── ReportPreview.tsx
│   │   │   └── reportApi.ts
│   │   └── settings/
│   │       ├── Settings.tsx
│   │       └── components/
│   │           ├── ProfileSettings.tsx
│   │           └── CategoryManagement.tsx
│   ├── components/
│   │   ├── layout/
│   │   │   ├── Header.tsx
│   │   │   ├── Sidebar.tsx
│   │   │   ├── Layout.tsx
│   │   │   └── Footer.tsx
│   │   ├── common/
│   │   │   ├── Button.tsx
│   │   │   ├── Card.tsx
│   │   │   ├── Modal.tsx
│   │   │   ├── Table.tsx
│   │   │   ├── Input.tsx
│   │   │   ├── Select.tsx
│   │   │   ├── DatePicker.tsx
│   │   │   ├── Loading.tsx
│   │   │   └── ErrorBoundary.tsx
│   │   └── ThemeToggle.tsx
│   ├── utils/
│   │   ├── api.ts
│   │   ├── formatters.ts
│   │   ├── validators.ts
│   │   └── constants.ts
│   ├── types/
│   │   ├── expense.ts
│   │   ├── income.ts
│   │   ├── budget.ts
│   │   ├── subscription.ts
│   │   └── user.ts
│   ├── hooks/
│   │   ├── useAuth.ts
│   │   ├── useTheme.ts
│   │   └── useDebounce.ts
│   ├── styles/
│   │   └── index.css
│   ├── App.tsx
│   ├── main.tsx
│   └── vite-env.d.ts
├── .env.example
├── .gitignore
├── package.json
├── tailwind.config.js
├── postcss.config.js
├── tsconfig.json
├── vite.config.ts
├── Dockerfile
└── README.md
```

## Docker Setup

```
docker/
├── docker-compose.yml
├── docker-compose.override.yml
└── .env.example
```

## Key Architectural Decisions

### Backend:
1. **Clean Architecture** - Clear separation of concerns, testable, maintainable
2. **CQRS Pattern** - Commands for writes, Queries for reads (using MediatR if needed, or simple pattern)
3. **Repository Pattern** - Abstraction over data access
4. **Unit of Work** - Transaction management via DbContext
5. **JWT + Identity** - Secure authentication with refresh tokens
6. **Background Jobs** - Hangfire for subscription reminders and recurring transactions
7. **Logging** - Serilog with structured logging

### Frontend:
1. **Feature-based structure** - Each feature is self-contained
2. **RTK Query** - Built-in caching, loading states, auto-refetching
3. **Tailwind** - Utility-first CSS for rapid UI development
4. **Dark mode** - CSS variables + Tailwind dark mode
5. **Type safety** - TypeScript for all components and API calls
