using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.RazorPages;
using RentalPlatform.Application.Interfaces;
using RentalPlatform.Application.UseCases.Dashboard;
using System.Security.Claims;

namespace RentalPlatform.Web.Pages.Front;

[Authorize]
public class OwnerDashboardModel : PageModel
{
    private readonly GetOwnerDashboardUseCase _getOwnerDashboardUseCase;

    public OwnerDashboardModel(GetOwnerDashboardUseCase getOwnerDashboardUseCase)
    {
        _getOwnerDashboardUseCase = getOwnerDashboardUseCase;
    }

    public DashboardResult? Dashboard { get; private set; }

    public async Task OnGetAsync()
    {
        var ownerIdClaim = User.FindFirstValue(ClaimTypes.NameIdentifier) ?? User.FindFirst("sub")?.Value;
        if (!Guid.TryParse(ownerIdClaim, out var ownerId))
        {
            return;
        }

        Dashboard = await _getOwnerDashboardUseCase.ExecuteAsync(new GetOwnerDashboardQuery(
            ownerId,
            null,
            DateTime.UtcNow.AddMonths(-1),
            DateTime.UtcNow));
    }
}
