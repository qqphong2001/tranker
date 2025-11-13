using ExpenseManagement.Application.Common.Interfaces;
using ExpenseManagement.Application.Common.Models;
using Microsoft.EntityFrameworkCore;

namespace ExpenseManagement.Application.Incomes.Commands;

public class DeleteIncomeCommandHandler
{
    private readonly IApplicationDbContext _context;
    private readonly ICurrentUserService _currentUser;

    public DeleteIncomeCommandHandler(IApplicationDbContext context, ICurrentUserService currentUser)
    {
        _context = context;
        _currentUser = currentUser;
    }

    public async Task<Result> Handle(Guid id, CancellationToken cancellationToken)
    {
        var income = await _context.Incomes
            .FirstOrDefaultAsync(i => i.Id == id && i.UserId == _currentUser.UserId, cancellationToken);

        if (income == null)
            return Result.Failure("Income not found");

        _context.Incomes.Remove(income);
        await _context.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}
