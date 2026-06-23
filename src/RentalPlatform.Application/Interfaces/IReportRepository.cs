using RentalPlatform.Domain.Entities;

namespace RentalPlatform.Application.Interfaces;

public interface IReportRepository
{
    Task<IEnumerable<Reservation>> GetReservationsForReportAsync(
        Guid ownerId,
        Guid? propertyId,
        DateTime from,
        DateTime to);
}
