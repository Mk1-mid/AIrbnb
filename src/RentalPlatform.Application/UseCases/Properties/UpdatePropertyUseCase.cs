using RentalPlatform.Application.Interfaces;
using RentalPlatform.Domain.ValueObjects;

namespace RentalPlatform.Application.UseCases.Properties;

public record UpdatePropertyCommand(
    Guid PropertyId,
    string Title,
    string Description,
    string City,
    decimal PricePerNight
);

public record UpdatePropertyResult(Guid PropertyId);

public class UpdatePropertyUseCase
{
    private readonly IPropertyRepository _properties;

    public UpdatePropertyUseCase(IPropertyRepository properties)
    {
        _properties = properties;
    }

    public async Task<UpdatePropertyResult> ExecuteAsync(UpdatePropertyCommand cmd)
    {
        var property = await _properties.GetByIdAsync(cmd.PropertyId);
        if (property == null)
        {
            throw new InvalidOperationException("Property not found");
        }

        property.Title = cmd.Title;
        property.Description = cmd.Description;
        property.City = cmd.City;
        property.PricePerNight = new Money(cmd.PricePerNight);

        await _properties.UpdateAsync(property);
        await _properties.SaveChangesAsync();

        return new UpdatePropertyResult(property.Id);
    }
}