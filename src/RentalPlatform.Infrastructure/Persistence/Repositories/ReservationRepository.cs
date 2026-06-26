using Microsoft.EntityFrameworkCore;
using RentalPlatform.Application.Interfaces;
using RentalPlatform.Domain.Entities;

namespace RentalPlatform.Infrastructure.Persistence.Repositories;

public class ReservationRepository : IReservationRepository
{
    private readonly RentalDbContext _context;

    public ReservationRepository(RentalDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Reservation>> GetByPropertyIdAsync(Guid propertyId) =>
        await _context.Reservations.Where(r => r.PropertyId == propertyId).ToListAsync();

    public async Task<IEnumerable<Reservation>> GetByGuestIdAsync(Guid guestId) =>
        await _context.Reservations
            .Include(r => r.Property)
            .Where(r => r.GuestId == guestId)
            .ToListAsync();

    public Task AddAsync(Reservation reservation) =>
        _context.Reservations.AddAsync(reservation).AsTask();

    public Task SaveChangesAsync() => _context.SaveChangesAsync();
}
