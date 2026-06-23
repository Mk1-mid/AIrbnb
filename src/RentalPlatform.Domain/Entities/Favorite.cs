namespace RentalPlatform.Domain.Entities;

public class Favorite
{
    public Guid UserId { get; set; }
    public Guid PropertyId { get; set; }
    public DateTime SavedAt { get; set; } = DateTime.UtcNow;

    public User User { get; set; } = null!;
    public Property Property { get; set; } = null!;
}
