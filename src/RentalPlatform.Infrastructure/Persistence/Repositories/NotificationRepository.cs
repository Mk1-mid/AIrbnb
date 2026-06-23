using Microsoft.EntityFrameworkCore;
using RentalPlatform.Application.Interfaces;
using RentalPlatform.Domain.Entities;
using RentalPlatform.Infrastructure.Persistence;

namespace RentalPlatform.Infrastructure.Persistence.Repositories;

public class NotificationRepository : INotificationRepository
{
    private readonly RentalDbContext _context;

    public NotificationRepository(RentalDbContext context)
    {
        _context = context;
    }

    public async Task AddAsync(Notification notification)
    {
        await _context.Notifications.AddAsync(notification);
    }

    public async Task<IEnumerable<Notification>> GetUnreadByUserIdAsync(Guid userId)
    {
        return await _context.Notifications
            .Where(n => n.UserId == userId && !n.IsRead)
            .OrderByDescending(n => n.CreatedAt)
            .ToListAsync();
    }

    public async Task SaveChangesAsync()
    {
        await _context.SaveChangesAsync();
    }
}
