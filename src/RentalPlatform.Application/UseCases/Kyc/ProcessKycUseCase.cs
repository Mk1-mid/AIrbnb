using System.Text.RegularExpressions;
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

            var extractedText = await _ocrService.ExtractTextAsync(tempPath);
            var parsed = ParseColombianId(extractedText);
            var status = parsed.IsComplete ? KycStatus.Approved : KycStatus.Rejected;

            var record = new KycRecord
            {
                Id = Guid.NewGuid(),
                UserId = cmd.UserId,
                DocumentNumber = parsed.DocumentNumber ?? "UNKNOWN",
                FirstName = parsed.FirstName ?? string.Empty,
                LastName = parsed.LastName ?? string.Empty,
                BirthDate = parsed.BirthDate ?? DateOnly.MinValue,
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

            var message = status == KycStatus.Approved
                ? "Your identity has been verified. You can now make reservations."
                : "We could not verify your identity. Please upload a clearer image.";

            await _notifications.SendInAppAsync(cmd.UserId, message, notificationType);
            await _notifications.SendEmailAsync(
                user.Email.Value,
                status == KycStatus.Approved ? "Identity Verified" : "Verification Failed",
                message);

            return new ProcessKycResult(
                status,
                parsed.DocumentNumber,
                parsed.FirstName,
                parsed.LastName,
                parsed.BirthDate,
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

    private static ParsedIdData ParseColombianId(string text)
    {
        var result = new ParsedIdData();
        var lines = text.Split('\n', StringSplitOptions.RemoveEmptyEntries)
            .Select(l => l.Trim())
            .ToArray();

        var docMatch = Regex.Match(text, @"\b(\d{8,10})\b");
        if (docMatch.Success)
        {
            result.DocumentNumber = docMatch.Value;
        }

        var dateMatch = Regex.Match(text, @"\b(\d{1,2})[\/\-\s](\d{1,2})[\/\-\s](\d{4})\b");
        if (dateMatch.Success &&
            int.TryParse(dateMatch.Groups[1].Value, out var day) &&
            int.TryParse(dateMatch.Groups[2].Value, out var month) &&
            int.TryParse(dateMatch.Groups[3].Value, out var year))
        {
            result.BirthDate = new DateOnly(year, month, day);
        }

        for (var i = 0; i < lines.Length; i++)
        {
            if (lines[i].Contains("APELLIDOS", StringComparison.OrdinalIgnoreCase) && i + 1 < lines.Length)
            {
                result.LastName = lines[i + 1];
            }

            if (lines[i].Contains("NOMBRES", StringComparison.OrdinalIgnoreCase) && i + 1 < lines.Length)
            {
                result.FirstName = lines[i + 1];
            }
        }

        return result;
    }

    private sealed class ParsedIdData
    {
        public string? DocumentNumber { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public DateOnly? BirthDate { get; set; }

        public bool IsComplete =>
            !string.IsNullOrEmpty(DocumentNumber) &&
            !string.IsNullOrEmpty(FirstName) &&
            !string.IsNullOrEmpty(LastName) &&
            BirthDate.HasValue;
    }
}
