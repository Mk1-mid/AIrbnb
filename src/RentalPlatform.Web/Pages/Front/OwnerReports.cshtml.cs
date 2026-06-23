using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using RentalPlatform.Application.UseCases.Reports;
using System.Security.Claims;

namespace RentalPlatform.Web.Pages.Front;

[Authorize(Policy = "OwnerOnly")]
public class OwnerReportsModel : PageModel
{
    private readonly ExportReportUseCase _exportReportUseCase;

    public OwnerReportsModel(ExportReportUseCase exportReportUseCase)
    {
        _exportReportUseCase = exportReportUseCase;
    }

    [BindProperty]
    public DateTime From { get; set; }

    [BindProperty]
    public DateTime To { get; set; }

    [BindProperty]
    public string? PropertyId { get; set; }

    public void OnGet()
    {
    }

    public async Task<IActionResult> OnPostExportAsync()
    {
        var userIdClaim = User.FindFirstValue(ClaimTypes.NameIdentifier)
            ?? User.FindFirst("sub")?.Value;

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
