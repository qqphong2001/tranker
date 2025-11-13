using ExpenseManagement.Application.Common.Interfaces;
using ExpenseManagement.Application.Common.Models;
using ExpenseManagement.Application.Notifications.DTOs;
using Microsoft.EntityFrameworkCore;

namespace ExpenseManagement.Application.Notifications.Queries;

public class GetNotificationsQuery
{
    public int PageNumber { get; set; } = 1;
    public int PageSize { get; set; } = 10;
    public bool? IsRead { get; set; }
}

public class GetNotificationsQueryHandler
{
    private readonly IApplicationDbContext _context;
    private readonly ICurrentUserService _currentUser;

    public GetNotificationsQueryHandler(IApplicationDbContext context, ICurrentUserService currentUser)
    {
        _context = context;
        _currentUser = currentUser;
    }

    public async Task<PaginatedList<NotificationDto>> Handle(GetNotificationsQuery request, CancellationToken cancellationToken)
    {
        var query = _context.Notifications
            .Where(n => n.UserId == _currentUser.UserId)
            .AsQueryable();

        // Apply filter
        if (request.IsRead.HasValue)
            query = query.Where(n => n.IsRead == request.IsRead.Value);

        // Order by created date descending (newest first)
        query = query.OrderByDescending(n => n.CreatedAt);

        // Project to DTO
        var dtoQuery = query.Select(n => new NotificationDto
        {
            Id = n.Id,
            Title = n.Title,
            Message = n.Message,
            Type = n.Type,
            IsRead = n.IsRead,
            ReadAt = n.ReadAt,
            RelatedEntityId = n.RelatedEntityId,
            CreatedAt = n.CreatedAt
        });

        return await PaginatedList<NotificationDto>.CreateAsync(dtoQuery, request.PageNumber, request.PageSize);
    }
}
