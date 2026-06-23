using RentalPlatform.Domain.Enums;

namespace RentalPlatform.Domain.Entities;

public class KycRecord
{
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
    public string FirstName { get; set; } = null!;
    public string LastName { get; set; } = null!;
    public string DocumentNumber { get; set; } = null!;
    public DateOnly BirthDate { get; set; }
    public KycStatus Status { get; set; } = KycStatus.Pending;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? ValidatedAt { get; set; }

    public User User { get; set; } = null!;
}
