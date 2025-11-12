using ExpenseManagement.Domain.Common;
using ExpenseManagement.Domain.Enums;

namespace ExpenseManagement.Domain.Entities;

public class Notification : AuditableEntity
{
    public string Title { get; set; } = string.Empty;
    public string Message { get; set; } = string.Empty;
    public NotificationType Type { get; set; }
    public bool IsRead { get; set; }
    public DateTime? ReadAt { get; set; }
    public string UserId { get; set; } = string.Empty;
    public string? RelatedEntityId { get; set; }
}
