using RentalPlatform.Domain.Entities;

namespace RentalPlatform.Application.Interfaces;

public interface IKycRepository
{
    Task<KycRecord?> GetByUserIdAsync(Guid userId);
    Task AddAsync(KycRecord record);
    Task SaveChangesAsync();
}
