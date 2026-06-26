using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using RentalPlatform.Application.Interfaces;
using RentalPlatform.Application.UseCases.Dashboard;
using System.Security.Claims;

namespace RentalPlatform.Web.Pages.Front;

[Authorize(Policy = "OwnerOnly")]
public class OwnerDashboardModel : PageModel
{
    private readonly GetOwnerDashboardUseCase _getDashboard;

    public DashboardResult? Dashboard { get; private set; }
    public string? ErrorMessage { get; private set; }

    public OwnerDashboardModel(GetOwnerDashboardUseCase getDashboard)
    {
        _getDashboard = getDashboard;
    }

    public async Task OnGet()
    {
        try
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier) ?? User.FindFirst("user_id");
            if (userIdClaim == null)
            {
                ErrorMessage = "User not authenticated";
                return;
            }

            var ownerId = Guid.Parse(userIdClaim.Value);
            var from = DateTime.UtcNow.AddDays(-30);
            var to = DateTime.UtcNow;

            Dashboard = await _getDashboard.ExecuteAsync(new GetOwnerDashboardQuery(
                ownerId, null, from, to));
        }
        catch (Exception ex)
        {
            ErrorMessage = ex.Message;
        }
    }
}