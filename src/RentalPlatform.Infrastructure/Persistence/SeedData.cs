using BCrypt.Net;
using Microsoft.EntityFrameworkCore;
using RentalPlatform.Domain.Entities;
using RentalPlatform.Domain.Enums;
using RentalPlatform.Domain.ValueObjects;

namespace RentalPlatform.Infrastructure.Persistence;

public static class SeedData
{
    public static async Task InitializeAsync(RentalDbContext context)
    {
        if (await context.Users.AnyAsync())
            return;

        var users = new List<User>();

        var owners = new[]
        {
            ("Sofia", "Vega", "sofia.owner1@demo.com"),
            ("Mateo", "Rios", "mateo.owner2@demo.com"),
            ("Camila", "Perez", "camila.owner3@demo.com"),
        };

        var guests = new[]
        {
            ("Laura", "Mora", "laura.guest1@demo.com"),
            ("Andres", "Lopez", "andres.guest2@demo.com"),
            ("Valentina", "Diaz", "valentina.guest3@demo.com"),
            ("Javier", "Castro", "javier.guest4@demo.com"),
            ("Paula", "Ortiz", "paula.guest5@demo.com"),
        };

        foreach (var (firstName, lastName, email) in owners)
        {
            users.Add(new User
            {
                Id = Guid.NewGuid(),
                FirstName = firstName,
                LastName = lastName,
                Email = new Email(email),
                PasswordHash = BCrypt.Net.BCrypt.HashPassword("Password123!"),
                Role = UserRole.Owner,
                KycVerified = false,
                CreatedAt = DateTime.UtcNow
            });
        }

        foreach (var (firstName, lastName, email) in guests)
        {
            users.Add(new User
            {
                Id = Guid.NewGuid(),
                FirstName = firstName,
                LastName = lastName,
                Email = new Email(email),
                PasswordHash = BCrypt.Net.BCrypt.HashPassword("Password123!"),
                Role = UserRole.Guest,
                KycVerified = true,
                CreatedAt = DateTime.UtcNow
            });
        }

        await context.Users.AddRangeAsync(users);
        await context.SaveChangesAsync();

        var ownerIds = users.Where(u => u.Role == UserRole.Owner).Select(u => u.Id).ToArray();
        var propertySpecs = new[]
        {
            ("Ocean View Loft", "Bright loft near the waterfront", "Cartagena", 180000m),
            ("Andes Cabin", "Cozy mountain escape", "Bogota", 220000m),
            ("Urban Studio", "Compact studio in the city center", "Medellin", 150000m),
            ("Garden House", "Private house with a calm garden", "Cali", 210000m),
            ("Skyline Apartment", "Modern apartment with skyline views", "Barranquilla", 195000m),
            ("Lake Retreat", "Relaxing stay by the lake", "Bucaramanga", 175000m),
            ("Historic Flat", "Restored flat with character", "Cartagena", 205000m),
            ("Beach Bungalow", "Steps away from the beach", "Santa Marta", 230000m),
            ("Penthouse Suite", "Large penthouse for premium stays", "Bogota", 310000m),
            ("Countryside Villa", "Quiet villa outside the city", "Pereira", 260000m),
        };

        var properties = propertySpecs.Select((spec, index) => new Property
        {
            Id = Guid.NewGuid(),
            OwnerId = ownerIds[index % ownerIds.Length],
            Title = spec.Item1,
            Description = spec.Item2,
            City = spec.Item3,
            PricePerNight = new Money(spec.Item4),
            IsActive = true,
            CreatedAt = DateTime.UtcNow
        }).ToList();

        await context.Properties.AddRangeAsync(properties);
        await context.SaveChangesAsync();
    }
}
