using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.RazorPages;
using RentalPlatform.Application.Interfaces;

namespace RentalPlatform.Web.Pages.Front;

[Authorize]
public class GuestFavoritesModel : PageModel
{
    private readonly IPropertyRepository _propertyRepository;

    public GuestFavoritesModel(IPropertyRepository propertyRepository)
    {
        _propertyRepository = propertyRepository;
    }

    public IEnumerable<RentalPlatform.Domain.Entities.Property> Favorites { get; private set; } = new List<RentalPlatform.Domain.Entities.Property>();
    public string? ErrorMessage { get; private set; }

    public async Task OnGet()
    {
        try
        {
            Favorites = await _propertyRepository.SearchAsync(null, null, null);
        }
        catch (Exception ex)
        {
            ErrorMessage = ex.Message;
        }
    }
}