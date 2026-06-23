namespace RentalPlatform.Domain.Entities;

public class PropertyImage
{
    public Guid Id { get; set; }
    public Guid PropertyId { get; set; }
    public string Url { get; set; } = null!;
    public bool IsCover { get; set; }

    public Property Property { get; set; } = null!;
}
