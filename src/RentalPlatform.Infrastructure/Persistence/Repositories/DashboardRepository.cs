using Microsoft.EntityFrameworkCore;
using RentalPlatform.Application.Interfaces;
using RentalPlatform.Domain.Enums;
using RentalPlatform.Infrastructure.Persistence;

namespace RentalPlatform.Infrastructure.Persistence.Repositories;

public class DashboardRepository : IDashboardRepository
{
    private readonly RentalDbContext _context;

    public DashboardRepository(RentalDbContext context)
    {
        _context = context;
    }

    public async Task<DashboardResult> GetOwnerMetricsAsync(
        Guid ownerId, Guid? propertyId, DateTime from, DateTime to)
    {
        var fromUtc = DateTime.SpecifyKind(from.Date, DateTimeKind.Utc);
        var toUtc = DateTime.SpecifyKind(to.Date.AddDays(1).AddTicks(-1), DateTimeKind.Utc);

        var query = _context.Reservations
            .Include(r => r.Property)
            .Where(r => r.Property.OwnerId == ownerId)
            .Where(r => r.Status == ReservationStatus.Confirmed ||
                        r.Status == ReservationStatus.Completed)
            .Where(r => r.StayPeriod.CheckIn >= fromUtc && r.StayPeriod.CheckOut <= toUtc);

        if (propertyId.HasValue)
            query = query.Where(r => r.PropertyId == propertyId.Value);

        var reservations = await query.ToListAsync();

        var totalRevenue = reservations.Sum(r => r.TotalPrice.Amount);
        var totalBookings = reservations.Count;
        var completedBookings = reservations.Count(r => r.Status == ReservationStatus.Completed);

        var totalDays = Math.Max(1, (toUtc - fromUtc).Days);
        var occupiedDays = reservations.Sum(r => r.StayPeriod.NightCount);
        var occupancyRate = Math.Min(100.0, (occupiedDays / (double)totalDays) * 100);

        var propertyMetrics = reservations
            .GroupBy(r => new { r.PropertyId, r.Property.Title })
            .Select(g => new PropertyMetric(
                g.Key.PropertyId,
                g.Key.Title,
                g.Sum(r => r.TotalPrice.Amount),
                g.Count(),
                Math.Min(100.0, (g.Sum(r => r.StayPeriod.NightCount) / (double)totalDays) * 100)
            ))
            .OrderByDescending(m => m.Revenue)
            .ToList();

        return new DashboardResult(
            totalRevenue,
            totalBookings,
            completedBookings,
            occupancyRate,
            propertyMetrics);
    }
}
