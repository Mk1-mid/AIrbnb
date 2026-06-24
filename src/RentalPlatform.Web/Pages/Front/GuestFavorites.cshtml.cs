using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using RentalPlatform.Application.Interfaces;
using RentalPlatform.Domain.Entities;
using System.Security.Claims;

namespace RentalPlatform.Web.Pages.Front;

[Authorize]
public class GuestFavoritesModel : PageModel
{
    private readonly IFavoriteRepository _favorites;

    public GuestFavoritesModel(IFavoriteRepository favorites)
    {
        _favorites = favorites;
    }

    public IReadOnlyList<Favorite> Favorites { get; private set; } = [];

    public async Task OnGetAsync()
    {
        var userId = GetUserId();
        if (userId is null)
        {
            Favorites = [];
            return;
        }

        Favorites = (await _favorites.GetByUserIdAsync(userId.Value)).ToList();
    }

    public async Task<IActionResult> OnPostRemoveAsync(Guid propertyId)
    {
        var userId = GetUserId();
        if (userId is null)
            return Unauthorized();

        var favorite = await _favorites.GetAsync(userId.Value, propertyId);
        if (favorite is not null)
        {
            await _favorites.RemoveAsync(favorite);
            await _favorites.SaveChangesAsync();
        }

        return RedirectToPage();
    }

    private Guid? GetUserId()
    {
        var userIdClaim = User.FindFirstValue(ClaimTypes.NameIdentifier) ?? User.FindFirst("sub")?.Value;
        return Guid.TryParse(userIdClaim, out var userId) ? userId : null;
    }
}
