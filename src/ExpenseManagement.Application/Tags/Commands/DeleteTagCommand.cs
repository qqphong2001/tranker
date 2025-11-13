using ExpenseManagement.Application.Common.Interfaces;
using ExpenseManagement.Application.Common.Models;
using Microsoft.EntityFrameworkCore;

namespace ExpenseManagement.Application.Tags.Commands;

public class DeleteTagCommandHandler
{
    private readonly IApplicationDbContext _context;
    private readonly ICurrentUserService _currentUser;

    public DeleteTagCommandHandler(IApplicationDbContext context, ICurrentUserService currentUser)
    {
        _context = context;
        _currentUser = currentUser;
    }

    public async Task<Result> Handle(Guid id, CancellationToken cancellationToken)
    {
        var tag = await _context.Tags
            .FirstOrDefaultAsync(t => t.Id == id && t.UserId == _currentUser.UserId, cancellationToken);

        if (tag == null)
            return Result.Failure("Tag not found");

        _context.Tags.Remove(tag);
        await _context.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}
