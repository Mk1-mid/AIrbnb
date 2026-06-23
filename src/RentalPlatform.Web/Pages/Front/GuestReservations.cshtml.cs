using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace RentalPlatform.Web.Pages.Front;

[Authorize]
public class GuestReservationsModel : PageModel
{
    public void OnGet()
    {
    }
}