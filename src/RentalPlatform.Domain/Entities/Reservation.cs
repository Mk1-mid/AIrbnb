using RentalPlatform.Domain.Enums;
using RentalPlatform.Domain.ValueObjects;

namespace RentalPlatform.Domain.Entities;

public class Reservation
{
    public Guid Id { get; private set; }
    public Guid PropertyId { get; private set; }
    public Guid GuestId { get; private set; }
    public DateRange StayPeriod { get; private set; } = null!;
    public Money TotalPrice { get; private set; } = null!;
    public ReservationStatus Status { get; private set; }
    public DateTime CreatedAt { get; private set; } = DateTime.UtcNow;

    public Property Property { get; private set; } = null!;
    public User Guest { get; private set; } = null!;
}
