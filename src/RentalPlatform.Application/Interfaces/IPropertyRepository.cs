using RentalPlatform.Domain.Entities;

namespace RentalPlatform.Application.Interfaces;

public interface IPropertyRepository
{
    Task<Property?> GetByIdAsync(Guid id);
    Task<IEnumerable<Property>> SearchAsync(string? city, DateTime? checkIn, DateTime? checkOut);
    Task AddAsync(Property property);
    Task SaveChangesAsync();
}
