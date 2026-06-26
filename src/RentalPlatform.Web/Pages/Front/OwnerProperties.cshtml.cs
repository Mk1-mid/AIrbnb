using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using RentalPlatform.Application.Interfaces;

namespace RentalPlatform.Web.Pages.Front;

[Authorize(Policy = "OwnerOnly")]
public class OwnerPropertiesModel : PageModel
{
    private readonly IPropertyRepository _propertyRepository;

    public OwnerPropertiesModel(IPropertyRepository propertyRepository)
    {
        _propertyRepository = propertyRepository;
    }

    public IEnumerable<RentalPlatform.Domain.Entities.Property> Properties { get; private set; } = new List<RentalPlatform.Domain.Entities.Property>();
    public string? ErrorMessage { get; private set; }

    public async Task OnGet()
    {
        try
        {
            var userIdClaim = User.FindFirst("sub")?.Value ?? User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
            if (!string.IsNullOrEmpty(userIdClaim) && Guid.TryParse(userIdClaim, out var ownerId))
            {
                Properties = await _propertyRepository.GetByOwnerIdAsync(ownerId);
            }
            else
            {
                Properties = new List<RentalPlatform.Domain.Entities.Property>();
            }
        }
        catch (Exception ex)
        {
            ErrorMessage = ex.Message;
        }
    }

    public async Task<IActionResult> OnPostToggleStatusAsync(Guid id)
    {
        var property = await _propertyRepository.GetByIdAsync(id);
        if (property == null)
        {
            return NotFound();
        }

        // Toggle status
        property.IsActive = !property.IsActive;
        await _propertyRepository.UpdateAsync(property);
        await _propertyRepository.SaveChangesAsync();

        return RedirectToPage();
    }
}