using ExpenseManagement.Application.Common.Models;

namespace ExpenseManagement.Application.Common.Interfaces;

public interface IEmailService
{
    Task SendEmailAsync(EmailRequest request, CancellationToken cancellationToken = default);
    Task SendBulkEmailAsync(List<EmailRequest> requests, CancellationToken cancellationToken = default);
}
