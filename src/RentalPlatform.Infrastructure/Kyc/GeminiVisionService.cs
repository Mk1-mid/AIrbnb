using Microsoft.Extensions.Configuration;
using RentalPlatform.Application.Interfaces;
using RentalPlatform.Domain.Entities;
using RentalPlatform.Domain.Enums;

namespace RentalPlatform.Infrastructure.Kyc;

public class GeminiVisionService : IOcrService
{
    private readonly HttpClient _httpClient;
    private readonly string? _apiKey;

    public GeminiVisionService(HttpClient httpClient, IConfiguration configuration)
    {
        _httpClient = httpClient;
        _apiKey = configuration["Gemini:ApiKey"];
    }

    public async Task<OcrResult> ExtractAsync(string imagePath)
    {
        // Fallback: If no API key, return the filename as mock data
        if (string.IsNullOrEmpty(_apiKey))
        {
            return new OcrResult(
                FirstName: "Test",
                LastName: "User",
                DocumentNumber: "TEST-" + Guid.NewGuid().ToString().Substring(0, 8),
                BirthDate: DateOnly.FromDateTime(DateTime.UtcNow.AddYears(-25)));
        }

        var base64 = Convert.ToBase64String(File.ReadAllBytes(imagePath));

        var payload = new
        {
            contents = new[]
            {
                new
                {
                    parts = new object[]
                    {
                        new
                        {
                            inline_data = new
                            {
                                mime_type = "image/jpeg",
                                data = base64
                            }
                        },
                        new
                        {
                            text = "Extract the following fields from this ID document and return ONLY a JSON object with no markdown: { \"firstName\": \"\", \"lastName\": \"\", \"documentNumber\": \"\", \"birthDate\": \"YYYY-MM-DD\" }"
                        }
                    }
                }
            }
        };

        var url = $"https://generativelanguage.googleapis.com/v1beta/models/gemini-1.5-flash:generateContent?key={_apiKey}";

        using var response = await _httpClient.PostAsJsonAsync(url, payload);
        response.EnsureSuccessStatusCode();

        var json = await response.Content.ReadAsStringAsync();
        using var doc = JsonDocument.Parse(json);
        var text = doc.RootElement
            .GetProperty("candidates")[0]
            .GetProperty("content")
            .GetProperty("parts")[0]
            .GetProperty("text")
            .GetString() ?? string.Empty;

        var cleaned = text.Replace("```json", string.Empty)
                          .Replace("```", string.Empty)
                          .Trim();

        var parsed = JsonSerializer.Deserialize<GeminiOcrResponse>(cleaned, new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        });

        return new OcrResult(
            parsed?.FirstName ?? "Unknown",
            parsed?.LastName ?? "User",
            parsed?.DocumentNumber ?? string.Empty,
            parsed?.BirthDate);
    }

    private sealed class GeminiOcrResponse
    {
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? DocumentNumber { get; set; }
        public string? BirthDate { get; set; }
    }
}
