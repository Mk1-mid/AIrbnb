using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using RentalPlatform.Application.Interfaces;
using RentalPlatform.Application.UseCases.Properties;
using System.Security.Claims;

namespace RentalPlatform.Web.Pages.Front;

[Authorize(Policy = "OwnerOnly")]
public class PublishEditPropertyModel : PageModel
{
    private readonly CreatePropertyUseCase _createProperty;
    private readonly UpdatePropertyUseCase _updateProperty;
    private readonly IPropertyRepository _propertyRepository;

    public PublishEditPropertyModel(
        CreatePropertyUseCase createProperty,
        UpdatePropertyUseCase updateProperty,
        IPropertyRepository propertyRepository)
    {
        _createProperty = createProperty;
        _updateProperty = updateProperty;
        _propertyRepository = propertyRepository;
    }

    [BindProperty(SupportsGet = true)]
    public Guid? Id { get; set; }

    [BindProperty]
    public string Title { get; set; } = string.Empty;

    [BindProperty]
    public string Description { get; set; } = string.Empty;

    [BindProperty]
    public string City { get; set; } = string.Empty;

    [BindProperty]
    public decimal PricePerNight { get; set; }

    [BindProperty]
    public bool IsActive { get; set; } = true;

    public string? ErrorMessage { get; private set; }
    public string? SuccessMessage { get; private set; }
    public bool IsEditMode => Id.HasValue;

    public async Task OnGetAsync()
    {
        if (Id.HasValue)
        {
            var property = await _propertyRepository.GetByIdAsync(Id.Value);
            if (property != null)
            {
                Title = property.Title;
                Description = property.Description;
                City = property.City;
                PricePerNight = property.PricePerNight.Amount;
                IsActive = property.IsActive;
            }
            else
            {
                ErrorMessage = "Property not found";
            }
        }
    }

    public async Task<IActionResult> OnPostAsync()
    {
        try
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? User.FindFirst("sub")?.Value;
            if (!Guid.TryParse(userIdClaim, out var ownerId))
            {
                ErrorMessage = "User not authenticated";
                return Page();
            }

            if (Id.HasValue)
            {
                // Edit mode
                await _updateProperty.ExecuteAsync(new UpdatePropertyCommand(
                    Id.Value,
                    Title,
                    Description,
                    City,
                    PricePerNight
                ));

                // Update IsActive status
                var property = await _propertyRepository.GetByIdAsync(Id.Value);
                if (property != null)
                {
                    property.IsActive = IsActive;
                    await _propertyRepository.UpdateAsync(property);
                    await _propertyRepository.SaveChangesAsync();
                }

                SuccessMessage = "Property updated successfully!";
            }
            else
            {
                // Create mode
                var result = await _createProperty.ExecuteAsync(new CreatePropertyCommand(
                    ownerId,
                    Title,
                    Description,
                    City,
                    PricePerNight
                ));

                // Set IsActive status
                var property = await _propertyRepository.GetByIdAsync(result.PropertyId);
                if (property != null)
                {
                    property.IsActive = IsActive;
                    await _propertyRepository.UpdateAsync(property);
                    await _propertyRepository.SaveChangesAsync();
                }

                SuccessMessage = "Property published successfully!";
                ModelState.Clear();
                Title = string.Empty;
                Description = string.Empty;
                City = string.Empty;
                PricePerNight = 0;
                IsActive = true;
            }

            return Page();
        }
        catch (Exception ex)
        {
            ErrorMessage = ex.Message;
            return Page();
        }
    }
}