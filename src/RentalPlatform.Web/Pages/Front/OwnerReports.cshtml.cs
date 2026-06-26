using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using RentalPlatform.Application.Interfaces;
using RentalPlatform.Application.UseCases.Reports;
using RentalPlatform.Domain.Entities;

namespace RentalPlatform.Web.Pages.Front;

[Authorize(Policy = "OwnerOnly")]
public class OwnerReportsModel : PageModel
{
    private readonly ExportReportUseCase _exportReportUseCase;
    private readonly IPropertyRepository _propertyRepository;

    public OwnerReportsModel(
        ExportReportUseCase exportReportUseCase,
        IPropertyRepository propertyRepository)
    {
        _exportReportUseCase = exportReportUseCase;
        _propertyRepository = propertyRepository;
    }

    [BindProperty]
    public DateTime From { get; set; } = DateTime.UtcNow.AddDays(-30);

    [BindProperty]
    public DateTime To { get; set; } = DateTime.UtcNow;

    [BindProperty]
    public string? PropertyId { get; set; }

    public IEnumerable<Property> Properties { get; private set; } = new List<Property>();
    public string? ErrorMessage { get; private set; }

    public async Task OnGet()
    {
        try
        {
            Properties = await _propertyRepository.SearchAsync(null, null, null);
        }
        catch (Exception ex)
        {
            ErrorMessage = ex.Message;
        }
    }

    public async Task<IActionResult> OnPostExportAsync()
    {
        var userIdClaim = User.FindFirst("sub")?.Value ?? User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;

        if (string.IsNullOrEmpty(userIdClaim) || !Guid.TryParse(userIdClaim, out var ownerId))
        {
            return Forbid();
        }

        Guid? propertyId = null;
        if (!string.IsNullOrWhiteSpace(PropertyId) && Guid.TryParse(PropertyId, out var parsedPropertyId))
        {
            propertyId = parsedPropertyId;
        }

        var bytes = await _exportReportUseCase.ExecuteAsync(
            new ExportReportCommand(ownerId, propertyId, From, To));

        var fileName = $"report_{DateTime.UtcNow:yyyyMMdd_HHmmss}.xlsx";
        return File(bytes, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", fileName);
    }
}
