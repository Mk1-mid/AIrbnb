using RentalPlatform.Application.Interfaces;
using RentalPlatform.Domain.Entities;
using RentalPlatform.Domain.Enums;

namespace RentalPlatform.Application.UseCases.Kyc;

public record ProcessKycCommand(
    Guid UserId,
    Stream ImageStream,
    string FileName
);

public record ProcessKycResult(
    KycStatus Status,
    string? DocumentNumber,
    string? FirstName,
    string? LastName,
    DateOnly? BirthDate,
    string Message
);

public class ProcessKycUseCase
{
    private readonly IKycRepository _kycRepository;
    private readonly IUserRepository _userRepository;
    private readonly IOcrService _ocrService;
    private readonly INotificationService _notifications;

    public ProcessKycUseCase(
        IKycRepository kycRepository,
        IUserRepository userRepository,
        IOcrService ocrService,
        INotificationService notifications)
    {
        _kycRepository = kycRepository;
        _userRepository = userRepository;
        _ocrService = ocrService;
        _notifications = notifications;
    }

    public async Task<ProcessKycResult> ExecuteAsync(ProcessKycCommand cmd)
    {
        var user = await _userRepository.GetByIdAsync(cmd.UserId)
            ?? throw new InvalidOperationException("User not found");

        var existing = await _kycRepository.GetByUserIdAsync(cmd.UserId);
        if (existing?.Status == KycStatus.Approved)
        {
            return new ProcessKycResult(
                KycStatus.Approved,
                existing.DocumentNumber,
                existing.FirstName,
                existing.LastName,
                existing.BirthDate,
                "Identity already verified");
        }

        var tempPath = Path.Combine(
            Path.GetTempPath(),
            $"kyc_{Guid.NewGuid()}{Path.GetExtension(cmd.FileName)}");

        try
        {
            await using (var fs = File.Create(tempPath))
            {
                await cmd.ImageStream.CopyToAsync(fs);
            }

            var extracted = await _ocrService.ExtractAsync(tempPath);

            var firstNameMatch = FuzzyMatch(user.FirstName, extracted.FirstName);
            var lastNameMatch = FuzzyMatch(user.LastName, extracted.LastName);
            var documentMatch = string.Equals(
                user.KycRecord?.DocumentNumber,
                extracted.DocumentNumber,
                StringComparison.OrdinalIgnoreCase);

            var mismatches = new List<string>();
            if (!firstNameMatch) mismatches.Add("first name");
            if (!lastNameMatch) mismatches.Add("last name");
            if (!documentMatch) mismatches.Add("document number");

            var status = mismatches.Count == 0
                ? KycStatus.Approved
                : KycStatus.Rejected;

            var message = status == KycStatus.Approved
                ? "Your identity has been verified. You can now make reservations."
                : $"We could not verify your identity. Mismatched fields: {string.Join(", ", mismatches)}.";

            var record = new KycRecord
            {
                Id = Guid.NewGuid(),
                UserId = cmd.UserId,
                FirstName = extracted.FirstName ?? user.FirstName,
                LastName = extracted.LastName ?? user.LastName,
                DocumentNumber = extracted.DocumentNumber ?? string.Empty,
                BirthDate = extracted.BirthDate ?? DateOnly.MinValue,
                Status = status,
                CreatedAt = DateTime.UtcNow,
                ValidatedAt = DateTime.UtcNow
            };

            await _kycRepository.AddAsync(record);

            if (status == KycStatus.Approved)
            {
                user.KycVerified = true;
                await _userRepository.SaveChangesAsync();
            }

            await _kycRepository.SaveChangesAsync();

            var notificationType = status == KycStatus.Approved
                ? NotificationType.KycApproved
                : NotificationType.KycRejected;

            await _notifications.SendInAppAsync(cmd.UserId, message, notificationType);
            await _notifications.SendEmailAsync(
                user.Email.Value,
                status == KycStatus.Approved ? "Identity Verified" : "Verification Failed",
                message);

            return new ProcessKycResult(
                status,
                record.DocumentNumber,
                record.FirstName,
                record.LastName,
                record.BirthDate,
                message);
        }
        finally
        {
            if (File.Exists(tempPath))
            {
                File.Delete(tempPath);
            }
        }
    }

    private static bool FuzzyMatch(string? expected, string? actual)
    {
        if (string.IsNullOrWhiteSpace(expected) || string.IsNullOrWhiteSpace(actual))
            return false;

        return LevenshteinDistance(
                   expected.Trim().ToLowerInvariant(),
                   actual.Trim().ToLowerInvariant()) <= 1;
    }

    private static int LevenshteinDistance(string s, string t)
    {
        if (string.IsNullOrEmpty(s)) return t.Length;
        if (string.IsNullOrEmpty(t)) return s.Length;

        var d = new int[s.Length + 1, t.Length + 1];

        for (var i = 0; i <= s.Length; i++) d[i, 0] = i;
        for (var j = 0; j <= t.Length; j++) d[0, j] = j;

        for (var i = 1; i <= s.Length; i++)
        {
            for (var j = 1; j <= t.Length; j++)
            {
                var cost = s[i - 1] == t[j - 1] ? 0 : 1;
                d[i, j] = Math.Min(
                    Math.Min(d[i - 1, j] + 1, d[i, j - 1] + 1),
                    d[i - 1, j - 1] + cost);
            }
        }

        return d[s.Length, t.Length];
    }
}
