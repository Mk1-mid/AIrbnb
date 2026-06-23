using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.RazorPages;
using RentalPlatform.Application.Interfaces;
using System.Security.Claims;

namespace RentalPlatform.Web.Pages.Front;

[Authorize]
public class GuestNotificationsModel : PageModel
{
    private readonly INotificationRepository _notificationRepository;

    public GuestNotificationsModel(INotificationRepository notificationRepository)
    {
        _notificationRepository = notificationRepository;
    }

    public List<Domain.Entities.Notification> Notifications { get; set; } = new();

    public async Task OnGet()
    {
        var userIdClaim = User.FindFirstValue(ClaimTypes.NameIdentifier)
            ?? User.FindFirst("sub")?.Value;

        if (string.IsNullOrEmpty(userIdClaim) || !Guid.TryParse(userIdClaim, out var userId))
        {
            return;
        }

        Notifications = (await _notificationRepository.GetUnreadByUserIdAsync(userId)).ToList();
    }
}
