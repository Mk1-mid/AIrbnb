using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.RazorPages;
using RentalPlatform.Application.Interfaces;
using RentalPlatform.Domain.Entities;
using System.Security.Claims;

namespace RentalPlatform.Web.Pages.Front;

[Authorize(Policy = "OwnerOnly")]
public class OwnerPropertiesModel : PageModel
{
    private readonly IPropertyRepository _properties;

    public OwnerPropertiesModel(IPropertyRepository properties)
    {
        _properties = properties;
    }

    public IReadOnlyList<Property> Properties { get; private set; } = [];

    public async Task OnGetAsync()
    {
        var ownerIdClaim = User.FindFirstValue(ClaimTypes.NameIdentifier) ?? User.FindFirst("sub")?.Value;
        if (!Guid.TryParse(ownerIdClaim, out var ownerId))
        {
            Properties = [];
            return;
        }

        Properties = (await _properties.GetByOwnerIdAsync(ownerId)).ToList();
    }
}
