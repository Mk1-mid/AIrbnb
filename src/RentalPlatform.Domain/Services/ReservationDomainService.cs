using RentalPlatform.Domain.Entities;
using RentalPlatform.Domain.Enums;
using RentalPlatform.Domain.ValueObjects;

namespace RentalPlatform.Domain.Services;

public class ReservationDomainService
{
    public bool HasConflict(IEnumerable<Reservation> existingReservations, DateRange requested)
    {
        return existingReservations
            .Where(r => r.Status == ReservationStatus.Confirmed)
            .Any(r => r.StayPeriod.Overlaps(requested));
    }
}
