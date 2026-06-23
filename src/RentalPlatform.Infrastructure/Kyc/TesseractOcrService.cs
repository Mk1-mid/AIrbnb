using RentalPlatform.Application.Interfaces;
using Tesseract;

namespace RentalPlatform.Infrastructure.Kyc;

public class TesseractOcrService : IOcrService
{
    private readonly string _tessdataPath;

    public TesseractOcrService(IWebHostEnvironment environment)
    {
        var candidates = new[]
        {
            Path.Combine(environment.ContentRootPath, "tessdata"),
            "/usr/share/tesseract-ocr/5/tessdata",
            "/usr/share/tesseract-ocr/4.00/tessdata"
        };

        _tessdataPath = candidates.FirstOrDefault(Directory.Exists)
            ?? candidates[0];
    }

    public Task<string> ExtractTextAsync(string imagePath)
    {
        if (!Directory.Exists(_tessdataPath))
            throw new InvalidOperationException($"Tesseract tessdata folder not found: {_tessdataPath}");

        using var engine = new TesseractEngine(_tessdataPath, "eng", EngineMode.Default);
        using var img = Pix.LoadFromFile(imagePath);
        using var page = engine.Process(img);

        return Task.FromResult(page.GetText());
    }
}
