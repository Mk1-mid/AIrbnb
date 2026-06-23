using Microsoft.EntityFrameworkCore;
using RentalPlatform.Application.Interfaces;
using RentalPlatform.Domain.Entities;

namespace RentalPlatform.Infrastructure.Persistence.Repositories;

public class PropertyRepository : IPropertyRepository
{
    private readonly RentalDbContext _context;

    public PropertyRepository(RentalDbContext context)
    {
        _context = context;
    }

    public Task<Property?> GetByIdAsync(Guid id) =>
        _context.Properties.FirstOrDefaultAsync(p => p.Id == id);

    public async Task<IEnumerable<Property>> SearchAsync(string? city, DateTime? checkIn, DateTime? checkOut)
    {
        var query = _context.Properties.AsQueryable().Where(p => p.IsActive);

        if (!string.IsNullOrWhiteSpace(city))
            query = query.Where(p => p.City.Contains(city));

        return await query.ToListAsync();
    }

    public Task AddAsync(Property property) =>
        _context.Properties.AddAsync(property).AsTask();

    public Task SaveChangesAsync() => _context.SaveChangesAsync();
}
