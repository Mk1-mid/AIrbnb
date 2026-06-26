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
        // Always ensure we have properties in the database
        if (!await context.Properties.AnyAsync())
        {
            // Create default owner if no users exist
            if (!await context.Users.AnyAsync())
            {
                var defaultOwner = new User
                {
                    Id = Guid.NewGuid(),
                    FirstName = "Demo",
                    LastName = "Owner",
                    Email = new Email("demo.owner@luxe stay.com"),
                    PasswordHash = BCrypt.Net.BCrypt.HashPassword("Password123!"),
                    Role = UserRole.Owner,
                    KycVerified = true,
                    CreatedAt = DateTime.UtcNow
                };

                await context.Users.AddAsync(defaultOwner);
                await context.SaveChangesAsync();

                var ownerIds = new[] { defaultOwner.Id };
                await CreatePropertiesAsync(context, ownerIds);
            }
            else
            {
                // Users exist but no properties - create properties for existing owners
                var ownerIds = context.Users
                    .Where(u => u.Role == UserRole.Owner)
                    .Select(u => u.Id)
                    .ToArray();

                if (ownerIds.Any())
                {
                    await CreatePropertiesAsync(context, ownerIds);
                }
            }
        }
    }

    private static async Task CreatePropertiesAsync(RentalDbContext context, Guid[] ownerIds)
    {
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
