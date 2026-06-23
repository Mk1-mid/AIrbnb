using RentalPlatform.Domain.Enums;
using RentalPlatform.Domain.ValueObjects;

namespace RentalPlatform.Domain.Entities;

public class User
{
    public Guid Id { get; set; }
    public string FirstName { get; set; } = null!;
    public string LastName { get; set; } = null!;
    public Email Email { get; set; } = null!;
    public string PasswordHash { get; set; } = null!;
    public UserRole Role { get; set; }
    public bool KycVerified { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public KycRecord? KycRecord { get; set; }
    public ICollection<Reservation> Reservations { get; set; } = new List<Reservation>();
    public ICollection<Favorite> Favorites { get; set; } = new List<Favorite>();
    public ICollection<Notification> Notifications { get; set; } = new List<Notification>();
}
