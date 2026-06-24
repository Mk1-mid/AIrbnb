using RentalPlatform.Domain.Entities;

namespace RentalPlatform.Application.Interfaces;

public interface IFavoriteRepository
{
    Task<IEnumerable<Favorite>> GetByUserIdAsync(Guid userId);
    Task<Favorite?> GetAsync(Guid userId, Guid propertyId);
    Task AddAsync(Favorite favorite);
    Task RemoveAsync(Favorite favorite);
    Task SaveChangesAsync();
}