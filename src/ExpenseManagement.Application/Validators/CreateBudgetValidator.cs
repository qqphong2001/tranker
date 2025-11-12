using ExpenseManagement.Application.Budgets.DTOs;
using FluentValidation;

namespace ExpenseManagement.Application.Validators;

public class CreateBudgetValidator : AbstractValidator<CreateBudgetDto>
{
    public CreateBudgetValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty()
            .WithMessage("Budget name is required")
            .MaximumLength(100)
            .WithMessage("Budget name cannot exceed 100 characters");

        RuleFor(x => x.Amount)
            .GreaterThan(0)
            .WithMessage("Amount must be greater than 0");

        RuleFor(x => x.StartDate)
            .NotEmpty()
            .WithMessage("Start date is required");

        RuleFor(x => x.CategoryId)
            .NotEmpty()
            .WithMessage("Category is required");

        RuleFor(x => x.WarningThreshold)
            .InclusiveBetween(0, 100)
            .WithMessage("Warning threshold must be between 0 and 100");
    }
}
