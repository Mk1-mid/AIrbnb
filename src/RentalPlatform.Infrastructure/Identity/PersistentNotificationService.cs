using RentalPlatform.Application.Interfaces;
using RentalPlatform.Domain.Entities;
using RentalPlatform.Domain.Enums;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Linq;

namespace RentalPlatform.Infrastructure.Identity;

public class PersistentNotificationService : INotificationService
{
    private readonly INotificationRepository _notificationRepository;
    private readonly HttpClient _httpClient;
    private readonly string _mailerBaseUrl;

    public PersistentNotificationService(
        INotificationRepository notificationRepository,
        HttpClient httpClient,
        Microsoft.Extensions.Configuration.IConfiguration configuration)
    {
        _notificationRepository = notificationRepository;
        _httpClient = httpClient;
        _mailerBaseUrl = configuration["Mailer:BaseUrl"]
            ?? "http://mailer:8081";
    }

    public async Task SendInAppAsync(Guid userId, string message, NotificationType type)
    {
        var notification = new Notification
        {
            Id = Guid.NewGuid(),
            UserId = userId,
            Message = message,
            Type = type,
            IsRead = false,
            CreatedAt = System.DateTime.UtcNow
        };

        await _notificationRepository.AddAsync(notification);
        await _notificationRepository.SaveChangesAsync();
    }

    public async Task SendEmailAsync(string toEmail, string subject, string body)
    {
        try
        {
            var payload = new
            {
                to = toEmail,
                subject = subject,
                body = body
            };

            await _httpClient.PostAsJsonAsync(
                $"{_mailerBaseUrl}/send-email",
                payload);
        }
        catch
        {
            // Email is best-effort; do not break user flows
        }
    }

    public Task<int> GetUnreadCountAsync(Guid userId)
    {
        return _notificationRepository.GetUnreadByUserIdAsync(userId)
            .ContinueWith(t => t.Result.Count());
    }
}
