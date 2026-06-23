namespace RentalPlatform.Application.Interfaces;

public record OcrResult(
    string? FirstName,
    string? LastName,
    string? DocumentNumber,
    DateOnly? BirthDate
);

public interface IOcrService
{
    Task<OcrResult> ExtractAsync(string imagePath);
}
