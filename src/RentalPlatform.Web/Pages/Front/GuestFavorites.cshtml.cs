using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace RentalPlatform.Web.Pages.Front;

[Authorize]
public class GuestFavoritesModel : PageModel
{
    public void OnGet()
    {
    }
}