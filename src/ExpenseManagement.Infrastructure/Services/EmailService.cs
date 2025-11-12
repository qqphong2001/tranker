using ExpenseManagement.Application.Common.Interfaces;
using ExpenseManagement.Application.Common.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using SendGrid;
using SendGrid.Helpers.Mail;
using System.Net;
using System.Net.Mail;

namespace ExpenseManagement.Infrastructure.Services;

public class EmailService : IEmailService
{
    private readonly IConfiguration _configuration;
    private readonly ILogger<EmailService> _logger;

    public EmailService(IConfiguration configuration, ILogger<EmailService> logger)
    {
        _configuration = configuration;
        _logger = logger;
    }

    public async Task SendEmailAsync(EmailRequest request, CancellationToken cancellationToken = default)
    {
        var useSendGrid = !string.IsNullOrEmpty(_configuration["SendGrid:ApiKey"]);

        if (useSendGrid)
        {
            await SendViaSendGridAsync(request, cancellationToken);
        }
        else
        {
            await SendViaSmtpAsync(request, cancellationToken);
        }
    }

    public async Task SendBulkEmailAsync(List<EmailRequest> requests, CancellationToken cancellationToken = default)
    {
        foreach (var request in requests)
        {
            await SendEmailAsync(request, cancellationToken);
        }
    }

    private async Task SendViaSendGridAsync(EmailRequest request, CancellationToken cancellationToken)
    {
        try
        {
            var apiKey = _configuration["SendGrid:ApiKey"];
            var client = new SendGridClient(apiKey);

            var from = new EmailAddress(_configuration["SendGrid:FromEmail"], _configuration["SendGrid:FromName"]);
            var to = new EmailAddress(request.To);
            var msg = MailHelper.CreateSingleEmail(from, to, request.Subject, request.IsHtml ? null : request.Body, request.IsHtml ? request.Body : null);

            var response = await client.SendEmailAsync(msg, cancellationToken);

            if (response.StatusCode != HttpStatusCode.OK && response.StatusCode != HttpStatusCode.Accepted)
            {
                _logger.LogError("Failed to send email via SendGrid: {StatusCode}", response.StatusCode);
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error sending email via SendGrid");
            throw;
        }
    }

    private async Task SendViaSmtpAsync(EmailRequest request, CancellationToken cancellationToken)
    {
        try
        {
            var smtpHost = _configuration["Smtp:Host"];
            var smtpPort = int.Parse(_configuration["Smtp:Port"] ?? "587");
            var smtpUser = _configuration["Smtp:Username"];
            var smtpPassword = _configuration["Smtp:Password"];
            var fromEmail = _configuration["Smtp:FromEmail"];
            var fromName = _configuration["Smtp:FromName"];

            using var client = new SmtpClient(smtpHost, smtpPort)
            {
                Credentials = new NetworkCredential(smtpUser, smtpPassword),
                EnableSsl = true
            };

            var mailMessage = new MailMessage
            {
                From = new MailAddress(fromEmail!, fromName),
                Subject = request.Subject,
                Body = request.Body,
                IsBodyHtml = request.IsHtml
            };

            mailMessage.To.Add(request.To);

            await client.SendMailAsync(mailMessage, cancellationToken);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error sending email via SMTP");
            throw;
        }
    }
}
