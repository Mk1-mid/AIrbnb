using RentalPlatform.Application.Interfaces;
using RentalPlatform.Domain.Entities;
using RentalPlatform.Domain.Enums;
using System.Linq;

namespace RentalPlatform.Infrastructure.Identity;

public class PersistentNotificationService : INotificationService
{
    private readonly INotificationRepository _notificationRepository;

    public PersistentNotificationService(INotificationRepository notificationRepository)
    {
        _notificationRepository = notificationRepository;
    }

    public async Task SendInAppAsync(Guid userId, string message, NotificationType type)
    {
        var notification = new Notification
        {
            Id = Guid.NewGuid(),
            UserId = userId,
            Message = message,
            Type = type,
            IsRead = false,
            CreatedAt = DateTime.UtcNow
        };

        await _notificationRepository.AddAsync(notification);
        await _notificationRepository.SaveChangesAsync();
    }

    public Task SendEmailAsync(string toEmail, string subject, string body)
    {
        return Task.CompletedTask;
    }

    public Task<int> GetUnreadCountAsync(Guid userId)
    {
        return _notificationRepository.GetUnreadByUserIdAsync(userId)
            .ContinueWith(t => t.Result.Count());
    }
}
