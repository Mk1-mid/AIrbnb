using RentalPlatform.Application.Interfaces;
using RentalPlatform.Domain.Entities;
using RentalPlatform.Domain.ValueObjects;

namespace RentalPlatform.Application.UseCases.Properties;

public record CreatePropertyCommand(
    Guid OwnerId,
    string Title,
    string Description,
    string City,
    decimal PricePerNight
);

public record CreatePropertyResult(Guid PropertyId);

public class CreatePropertyUseCase
{
    private readonly IPropertyRepository _properties;

    public CreatePropertyUseCase(IPropertyRepository properties)
    {
        _properties = properties;
    }

    public async Task<CreatePropertyResult> ExecuteAsync(CreatePropertyCommand cmd)
    {
        var property = new Property
        {
            Id = Guid.NewGuid(),
            OwnerId = cmd.OwnerId,
            Title = cmd.Title,
            Description = cmd.Description,
            City = cmd.City,
            PricePerNight = new Money(cmd.PricePerNight),
            IsActive = true,
            CreatedAt = DateTime.UtcNow
        };

        await _properties.AddAsync(property);
        await _properties.SaveChangesAsync();

        return new CreatePropertyResult(property.Id);
    }
}