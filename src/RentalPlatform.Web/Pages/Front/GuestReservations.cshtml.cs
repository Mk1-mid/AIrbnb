using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.RazorPages;
using RentalPlatform.Application.Interfaces;
using RentalPlatform.Domain.Entities;
using System.Security.Claims;

namespace RentalPlatform.Web.Pages.Front;

[Authorize]
public class GuestReservationsModel : PageModel
{
    private readonly IReservationRepository _reservations;

    public GuestReservationsModel(IReservationRepository reservations)
    {
        _reservations = reservations;
    }

    public IReadOnlyList<Reservation> Reservations { get; private set; } = [];

    public async Task OnGetAsync()
    {
        var userId = GetUserId();
        if (userId is null)
        {
            Reservations = [];
            return;
        }

        Reservations = (await _reservations.GetByGuestIdAsync(userId.Value)).ToList();
    }

    private Guid? GetUserId()
    {
        var userIdClaim = User.FindFirstValue(ClaimTypes.NameIdentifier) ?? User.FindFirst("sub")?.Value;
        return Guid.TryParse(userIdClaim, out var userId) ? userId : null;
    }
}
