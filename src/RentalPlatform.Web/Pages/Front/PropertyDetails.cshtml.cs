using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using RentalPlatform.Application.Interfaces;
using RentalPlatform.Domain.Entities;
using RentalPlatform.Application.UseCases.Reservations;
using System.Security.Claims;

namespace RentalPlatform.Web.Pages.Front;

public class PropertyDetailsModel : PageModel
{
    private readonly IPropertyRepository _properties;
    private readonly IReservationRepository _reservations;
    private readonly CreateReservationUseCase _createReservation;

    public PropertyDetailsModel(
        IPropertyRepository properties,
        IReservationRepository reservations,
        CreateReservationUseCase createReservation)
    {
        _properties = properties;
        _reservations = reservations;
        _createReservation = createReservation;
    }

    public Property? Property { get; private set; }

    [BindProperty]
    public DateTime CheckIn { get; set; } = DateTime.Today.AddDays(1);

    [BindProperty]
    public DateTime CheckOut { get; set; } = DateTime.Today.AddDays(2);

    public string? ErrorMessage { get; private set; }
    public string? SuccessMessage { get; private set; }

    public async Task<IActionResult> OnGetAsync(Guid id)
    {
        Property = await _properties.GetByIdAsync(id);
        if (Property == null)
        {
            return NotFound();
        }
        return Page();
    }

    public async Task<IActionResult> OnPostAsync(Guid id)
    {
        Property = await _properties.GetByIdAsync(id);
        if (Property == null)
        {
            return NotFound();
        }

        var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? User.FindFirst("sub")?.Value;
        if (!Guid.TryParse(userIdClaim, out var guestId))
        {
            ErrorMessage = "You must be signed in to make a reservation";
            return Page();
        }

        try
        {
            var result = await _createReservation.ExecuteAsync(new CreateReservationCommand(
                id,
                guestId,
                CheckIn,
                CheckOut
            ));

            SuccessMessage = $"Reservation confirmed! Total: {result.TotalAmount:C}";
            return Page();
        }
        catch (Exception ex)
        {
            ErrorMessage = ex.Message;
            return Page();
        }
    }
}