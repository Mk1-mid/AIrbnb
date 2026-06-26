using Microsoft.AspNetCore.Mvc.RazorPages;
using RentalPlatform.Application.Interfaces;
using RentalPlatform.Domain.Entities;

namespace RentalPlatform.Web.Pages.Front;

public class IndexModel : PageModel
{
    private readonly IPropertyRepository _propertyRepository;

    public IndexModel(IPropertyRepository propertyRepository)
    {
        _propertyRepository = propertyRepository;
    }

    public IEnumerable<Property> Properties { get; private set; } = new List<Property>();

    public async Task OnGetAsync()
    {
        Properties = await _propertyRepository.SearchAsync(null, null, null);
    }
}
