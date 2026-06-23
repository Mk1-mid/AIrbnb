using RentalPlatform.Domain.Entities;

namespace RentalPlatform.Application.Interfaces;

public interface IReservationRepository
{
    Task<IEnumerable<Reservation>> GetByPropertyIdAsync(Guid propertyId);
    Task<IEnumerable<Reservation>> GetByGuestIdAsync(Guid guestId);
    Task AddAsync(Reservation reservation);
    Task SaveChangesAsync();
}
