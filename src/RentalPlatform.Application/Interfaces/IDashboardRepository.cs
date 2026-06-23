using RentalPlatform.Domain.Entities;

namespace RentalPlatform.Application.Interfaces;

public interface IDashboardRepository
{
    Task<DashboardResult> GetOwnerMetricsAsync(
        Guid ownerId,
        Guid? propertyId,
        DateTime from,
        DateTime to);
}

public record DashboardResult(
    decimal TotalRevenue,
    int TotalBookings,
    int CompletedBookings,
    double OccupancyRate,
    IEnumerable<PropertyMetric> PropertyMetrics
);

public record PropertyMetric(
    Guid PropertyId,
    string Title,
    decimal Revenue,
    int Bookings,
    double OccupancyRate
);
