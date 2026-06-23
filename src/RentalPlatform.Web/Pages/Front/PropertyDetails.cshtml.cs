using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using RentalPlatform.Application.UseCases.Reservations;
using System.Security.Claims;

namespace RentalPlatform.Web.Pages.Front;

[Authorize]
public class PropertyDetailsModel : PageModel
{
    private readonly CreateReservationUseCase _createReservationUseCase;

    public PropertyDetailsModel(CreateReservationUseCase createReservationUseCase)
    {
        _createReservationUseCase = createReservationUseCase;
    }

    [BindProperty(SupportsGet = true)]
    public Guid PropertyId { get; set; }

    [BindProperty]
    public DateTime CheckIn { get; set; }

    [BindProperty]
    public DateTime CheckOut { get; set; }

    public void OnGet()
    {
    }

    public async Task<IActionResult> OnPostReserveAsync()
    {
        var userIdClaim = User.FindFirstValue(ClaimTypes.NameIdentifier)
            ?? User.FindFirst("sub")?.Value;

        if (string.IsNullOrEmpty(userIdClaim) || !Guid.TryParse(userIdClaim, out var userId))
        {
            ModelState.AddModelError(string.Empty, "Usuario no autenticado.");
            return Page();
        }

        if (CheckOut <= CheckIn)
        {
            ModelState.AddModelError(string.Empty, "La fecha de salida debe ser posterior a la de entrada.");
            return Page();
        }

        try
        {
            var result = await _createReservationUseCase.ExecuteAsync(
                new CreateReservationCommand(PropertyId, userId, CheckIn, CheckOut));

            TempData["Success"] = "Reserva creada correctamente.";
            return RedirectToPage("/Front/GuestReservations");
        }
        catch (InvalidOperationException ex)
        {
            ModelState.AddModelError(string.Empty, ex.Message);
            return Page();
        }
    }
}
