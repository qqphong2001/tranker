using ExpenseManagement.Application.Common.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace ExpenseManagement.Infrastructure.BackgroundJobs;

public class SubscriptionReminderJob
{
    private readonly IApplicationDbContext _context;
    private readonly INotificationService _notificationService;
    private readonly IDateTime _dateTime;
    private readonly ILogger<SubscriptionReminderJob> _logger;

    public SubscriptionReminderJob(
        IApplicationDbContext context,
        INotificationService notificationService,
        IDateTime dateTime,
        ILogger<SubscriptionReminderJob> logger)
    {
        _context = context;
        _notificationService = notificationService;
        _dateTime = dateTime;
        _logger = logger;
    }

    public async Task ExecuteAsync()
    {
        _logger.LogInformation("Starting subscription reminder job at {Time}", _dateTime.UtcNow);

        try
        {
            var today = _dateTime.Now.Date;

            // Get subscriptions that need reminders
            var subscriptions = await _context.Subscriptions
                .Include(s => s.Category)
                .Where(s => s.IsActive && s.NextBillingDate.Date >= today && s.NextBillingDate.Date <= today.AddDays(7))
                .ToListAsync();

            foreach (var subscription in subscriptions)
            {
                var daysUntilBilling = (subscription.NextBillingDate.Date - today).Days;

                if (daysUntilBilling <= subscription.ReminderDaysBefore)
                {
                    var message = $"Your subscription '{subscription.Name}' of {subscription.Amount:C} will be charged in {daysUntilBilling} day(s) on {subscription.NextBillingDate:yyyy-MM-dd}.";

                    await _notificationService.CreateNotificationAsync(
                        subscription.UserId,
                        "Subscription Reminder",
                        message,
                        Domain.Enums.NotificationType.SubscriptionDue,
                        subscription.Id.ToString());

                    _logger.LogInformation("Created reminder for subscription {SubscriptionId}", subscription.Id);
                }
            }

            _logger.LogInformation("Subscription reminder job completed successfully");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error executing subscription reminder job");
        }
    }
}
