namespace RentalPlatform.Application.Interfaces;

public interface IOcrService
{
    Task<string> ExtractTextAsync(string imagePath);
}
