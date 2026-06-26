using System.Net.Http.Json;
using System.Text.Json;
using Microsoft.Extensions.Configuration;
using RentalPlatform.Application.Interfaces;

namespace RentalPlatform.Infrastructure.Kyc;

public class GeminiVisionService : IOcrService
{
    private readonly HttpClient _httpClient;
    private readonly IConfiguration _configuration;

    public GeminiVisionService(HttpClient httpClient, IConfiguration configuration)
    {
        _httpClient = httpClient;
        _configuration = configuration;
    }

    public async Task<OcrResult> ExtractAsync(string imagePath)
    {
        var apiKey = _configuration["Gemini:ApiKey"];
        
        // Fallback: If no API key configured, return mock data
        if (string.IsNullOrEmpty(apiKey))
        {
            return new OcrResult(
                FirstName: "Anonymous",
                LastName: "User",
                DocumentNumber: "MOCK-" + Guid.NewGuid().ToString("N").Substring(0, 8),
                BirthDate: null);
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
                            text = "Extract the following fields from this ID document and return ONLY a JSON object with no markdown: { \"firstName\": \"\", \"lastName\": \"\", \"documentNumber\": \"\", \"birthDate\": \"\" }"
                        }
                    }
                }
            }
        };

        var url = $"https://generativelanguage.googleapis.com/v1beta/models/gemini-1.5-flash:generateContent?key={apiKey}";

        var httpContent = JsonContent.Create(payload);
        using var response = await _httpClient.PostAsync(url, httpContent);
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

        var parsed = JsonSerializer.Deserialize<OcrResponse>(cleaned, new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        });

        return new OcrResult(
            parsed?.FirstName ?? "Anonymous",
            parsed?.LastName ?? "User",
            parsed?.DocumentNumber ?? string.Empty,
            null);
    }

    private class OcrResponse
    {
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? DocumentNumber { get; set; }
        public string? BirthDate { get; set; }
    }
}
