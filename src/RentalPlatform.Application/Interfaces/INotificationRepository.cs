using RentalPlatform.Domain.Entities;

namespace RentalPlatform.Application.Interfaces;

public interface INotificationRepository
{
    Task AddAsync(Notification notification);
    Task<IEnumerable<Notification>> GetUnreadByUserIdAsync(Guid userId);
    Task SaveChangesAsync();
}
