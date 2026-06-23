using Microsoft.EntityFrameworkCore;
using RentalPlatform.Application.Interfaces;
using RentalPlatform.Domain.Entities;

namespace RentalPlatform.Infrastructure.Persistence.Repositories;

public class KycRepository : IKycRepository
{
    private readonly RentalDbContext _context;

    public KycRepository(RentalDbContext context)
    {
        _context = context;
    }

    public Task<KycRecord?> GetByUserIdAsync(Guid userId) =>
        _context.KycRecords.FirstOrDefaultAsync(k => k.UserId == userId);

    public Task AddAsync(KycRecord record) =>
        _context.KycRecords.AddAsync(record).AsTask();

    public Task SaveChangesAsync() =>
        _context.SaveChangesAsync();
}
