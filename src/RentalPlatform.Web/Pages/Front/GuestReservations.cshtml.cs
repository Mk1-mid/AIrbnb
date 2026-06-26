using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.RazorPages;
using RentalPlatform.Application.Interfaces;
using RentalPlatform.Domain.Entities;

namespace RentalPlatform.Web.Pages.Front;

[Authorize]
public class GuestReservationsModel : PageModel
{
    private readonly IReservationRepository _reservationRepository;

    public GuestReservationsModel(IReservationRepository reservationRepository)
    {
        _reservationRepository = reservationRepository;
    }

    public IEnumerable<Reservation> Reservations { get; private set; } = new List<Reservation>();
    public string? ErrorMessage { get; private set; }

    public async Task OnGet()
    {
        try
        {
            var userIdClaim = User.FindFirst("sub")?.Value ?? User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
            if (!string.IsNullOrEmpty(userIdClaim) && Guid.TryParse(userIdClaim, out var userId))
            {
                Reservations = await _reservationRepository.GetByGuestIdAsync(userId);
            }
        }
        catch (Exception ex)
        {
            ErrorMessage = ex.Message;
        }
    }
}