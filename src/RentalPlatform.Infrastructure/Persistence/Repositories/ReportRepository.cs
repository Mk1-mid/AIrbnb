using Microsoft.EntityFrameworkCore;
using RentalPlatform.Application.Interfaces;
using RentalPlatform.Domain.Entities;
using RentalPlatform.Domain.Enums;
using RentalPlatform.Infrastructure.Persistence;

namespace RentalPlatform.Infrastructure.Persistence.Repositories;

public class ReportRepository : IReportRepository
{
    private readonly RentalDbContext _context;

    public ReportRepository(RentalDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Reservation>> GetReservationsForReportAsync(
        Guid ownerId,
        Guid? propertyId,
        DateTime from,
        DateTime to)
    {
        var fromUtc = DateTime.SpecifyKind(from.Date, DateTimeKind.Utc);
        var toUtc = DateTime.SpecifyKind(to.Date.AddDays(1).AddTicks(-1), DateTimeKind.Utc);

        var query = _context.Reservations
            .Include(r => r.Property)
            .Include(r => r.Guest)
            .Where(r => r.Property.OwnerId == ownerId)
            .Where(r => r.Status == ReservationStatus.Confirmed ||
                        r.Status == ReservationStatus.Completed)
            .Where(r => r.StayPeriod.CheckIn >= fromUtc && r.StayPeriod.CheckOut <= toUtc);

        if (propertyId.HasValue)
            query = query.Where(r => r.PropertyId == propertyId.Value);

        return await query.ToListAsync();
    }
}
