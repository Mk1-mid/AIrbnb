using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using RentalPlatform.Application.Interfaces;
using RentalPlatform.Domain.Entities;

namespace RentalPlatform.Web.Pages.Front;

public class IndexModel : PageModel
{
    private readonly IPropertyRepository _properties;
    private readonly ILogger<IndexModel> _logger;

    public IndexModel(IPropertyRepository properties, ILogger<IndexModel> logger)
    {
        _properties = properties;
        _logger = logger;
    }

    [BindProperty(SupportsGet = true)]
    public string? City { get; set; }

    [BindProperty(SupportsGet = true)]
    public DateTime? CheckIn { get; set; }

    [BindProperty(SupportsGet = true)]
    public DateTime? CheckOut { get; set; }

    public IReadOnlyList<Property> Properties { get; private set; } = [];

    public async Task OnGetAsync()
    {
        Properties = (await _properties.SearchAsync(City, CheckIn, CheckOut)).ToList();
        _logger.LogInformation("Loaded {Count} properties for front page", Properties.Count);
    }
}
