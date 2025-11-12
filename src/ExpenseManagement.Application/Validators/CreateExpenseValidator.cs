using ExpenseManagement.Application.Expenses.DTOs;
using FluentValidation;

namespace ExpenseManagement.Application.Validators;

public class CreateExpenseValidator : AbstractValidator<CreateExpenseDto>
{
    public CreateExpenseValidator()
    {
        RuleFor(x => x.Amount)
            .GreaterThan(0)
            .WithMessage("Amount must be greater than 0");

        RuleFor(x => x.Date)
            .NotEmpty()
            .WithMessage("Date is required")
            .LessThanOrEqualTo(DateTime.Now.AddDays(1))
            .WithMessage("Date cannot be in the future");

        RuleFor(x => x.CategoryId)
            .NotEmpty()
            .WithMessage("Category is required");

        RuleFor(x => x.Description)
            .MaximumLength(200)
            .WithMessage("Description cannot exceed 200 characters");

        RuleFor(x => x.Notes)
            .MaximumLength(1000)
            .WithMessage("Notes cannot exceed 1000 characters");
    }
}
