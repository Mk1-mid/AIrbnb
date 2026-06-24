using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using RentalPlatform.Application.Interfaces;
using RentalPlatform.Domain.Entities;
using RentalPlatform.Domain.ValueObjects;
using System.Security.Claims;

namespace RentalPlatform.Web.Pages.Front;

[Authorize(Policy = "OwnerOnly")]
public class PublishEditPropertyModel : PageModel
{
    private readonly IPropertyRepository _properties;

    public PublishEditPropertyModel(IPropertyRepository properties)
    {
        _properties = properties;
    }

    [BindProperty(SupportsGet = true)]
    public Guid? PropertyId { get; set; }

    [BindProperty]
    public string Title { get; set; } = string.Empty;

    [BindProperty]
    public string Description { get; set; } = string.Empty;

    [BindProperty]
    public string City { get; set; } = string.Empty;

    [BindProperty]
    public decimal Price { get; set; }

    [BindProperty]
    public string PropertyType { get; set; } = "villa";

    [BindProperty]
    public bool IsActive { get; set; }

    public async Task OnGetAsync()
    {
        if (PropertyId is null)
            return;

        var property = await _properties.GetByIdAsync(PropertyId.Value);
        if (property is null)
            return;

        Title = property.Title;
        Description = property.Description;
        City = property.City;
        Price = property.PricePerNight.Amount;
        IsActive = property.IsActive;
    }

    public async Task<IActionResult> OnPostSaveAsync()
    {
        var ownerIdClaim = User.FindFirstValue(ClaimTypes.NameIdentifier) ?? User.FindFirst("sub")?.Value;
        if (!Guid.TryParse(ownerIdClaim, out var ownerId))
            return Unauthorized();

        Property property;
        if (PropertyId is not null)
        {
            property = await _properties.GetByIdAsync(PropertyId.Value)
                ?? new Property { Id = Guid.NewGuid(), OwnerId = ownerId };
        }
        else
        {
            property = new Property { Id = Guid.NewGuid(), OwnerId = ownerId };
        }

        property.Title = Title;
        property.Description = Description;
        property.City = City;
        property.PricePerNight = new Money(Price, "COP");
        property.IsActive = IsActive;

        if (PropertyId is null)
        {
            await _properties.AddAsync(property);
        }

        await _properties.SaveChangesAsync();
        return RedirectToPage("/Front/OwnerProperties");
    }
}
