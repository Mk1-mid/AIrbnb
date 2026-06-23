using RentalPlatform.Domain.Enums;

namespace RentalPlatform.Application.Interfaces;

public interface INotificationService
{
    Task SendInAppAsync(Guid userId, string message, NotificationType type);
    Task SendEmailAsync(string toEmail, string subject, string body);
}
