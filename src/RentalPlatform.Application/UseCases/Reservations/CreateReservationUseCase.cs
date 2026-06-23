using RentalPlatform.Domain.Entities;
using RentalPlatform.Domain.Enums;
using RentalPlatform.Domain.Services;
using RentalPlatform.Domain.ValueObjects;
using RentalPlatform.Application.Interfaces;

namespace RentalPlatform.Application.UseCases.Reservations;

public record CreateReservationCommand(
    Guid PropertyId,
    Guid GuestId,
    DateTime CheckIn,
    DateTime CheckOut
);

public record CreateReservationResult(Guid ReservationId, decimal TotalAmount);

public class CreateReservationUseCase
{
    private readonly IReservationRepository _reservations;
    private readonly IPropertyRepository _properties;
    private readonly IUserRepository _users;
    private readonly INotificationService _notifications;
    private readonly ReservationDomainService _domainService;

    public CreateReservationUseCase(
        IReservationRepository reservations,
        IPropertyRepository properties,
        IUserRepository users,
        INotificationService notifications,
        ReservationDomainService domainService)
    {
        _reservations = reservations;
        _properties = properties;
        _users = users;
        _notifications = notifications;
        _domainService = domainService;
    }

    public async Task<CreateReservationResult> ExecuteAsync(CreateReservationCommand cmd)
    {
        var guest = await _users.GetByIdAsync(cmd.GuestId)
            ?? throw new InvalidOperationException("User not found");

        if (!guest.KycVerified)
            throw new InvalidOperationException("KYC verification required before booking");

        var property = await _properties.GetByIdAsync(cmd.PropertyId)
            ?? throw new InvalidOperationException("Property not found");

        if (!property.IsActive)
            throw new InvalidOperationException("Property is not available");

        var stayPeriod = new DateRange(cmd.CheckIn, cmd.CheckOut);

        var existingReservations = await _reservations.GetByPropertyIdAsync(cmd.PropertyId);

        if (_domainService.HasConflict(existingReservations, stayPeriod))
            throw new InvalidOperationException("Property is not available for selected dates");

        var totalAmount = property.PricePerNight.Multiply(stayPeriod.NightCount);

        var reservation = new Reservation(
            Guid.NewGuid(),
            cmd.PropertyId,
            cmd.GuestId,
            stayPeriod,
            totalAmount,
            ReservationStatus.Confirmed
        );

        await _reservations.AddAsync(reservation);
        await _reservations.SaveChangesAsync();

        await _notifications.SendInAppAsync(
            cmd.GuestId,
            $"Your reservation at {property.Title} has been confirmed.",
            NotificationType.ReservationConfirmed);

        await _notifications.SendEmailAsync(
            guest.Email.Value,
            "Reservation Confirmed",
            $"Your stay at {property.Title} is confirmed. Check-in: {stayPeriod.CheckIn:f}. Check-out: {stayPeriod.CheckOut:f}.");

        return new CreateReservationResult(reservation.Id, totalAmount.Amount);
    }
}
