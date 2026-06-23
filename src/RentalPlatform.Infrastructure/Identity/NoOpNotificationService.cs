using RentalPlatform.Application.Interfaces;
using RentalPlatform.Domain.Enums;

namespace RentalPlatform.Infrastructure.Identity;

public class NoOpNotificationService : INotificationService
{
    public Task SendInAppAsync(Guid userId, string message, NotificationType type) =>
        Task.CompletedTask;

    public Task SendEmailAsync(string toEmail, string subject, string body) =>
        Task.CompletedTask;
}
