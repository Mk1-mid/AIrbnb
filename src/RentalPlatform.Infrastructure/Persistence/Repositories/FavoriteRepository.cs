using Microsoft.EntityFrameworkCore;
using RentalPlatform.Application.Interfaces;
using RentalPlatform.Domain.Entities;
using RentalPlatform.Infrastructure.Persistence;

namespace RentalPlatform.Infrastructure.Persistence.Repositories;

public class FavoriteRepository : IFavoriteRepository
{
    private readonly RentalDbContext _context;

    public FavoriteRepository(RentalDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Favorite>> GetByUserIdAsync(Guid userId)
    {
        return await _context.Favorites
            .Where(f => f.UserId == userId)
            .Include(f => f.Property)
            .OrderByDescending(f => f.SavedAt)
            .ToListAsync();
    }

    public async Task<Favorite?> GetAsync(Guid userId, Guid propertyId)
    {
        return await _context.Favorites
            .FirstOrDefaultAsync(f => f.UserId == userId && f.PropertyId == propertyId);
    }

    public async Task AddAsync(Favorite favorite)
    {
        await _context.Favorites.AddAsync(favorite);
    }

    public Task RemoveAsync(Favorite favorite)
    {
        _context.Favorites.Remove(favorite);
        return Task.CompletedTask;
    }

    public async Task SaveChangesAsync()
    {
        await _context.SaveChangesAsync();
    }
}