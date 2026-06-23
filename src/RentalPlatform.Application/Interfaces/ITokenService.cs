using RentalPlatform.Domain.Entities;

namespace RentalPlatform.Application.Interfaces;

public interface ITokenService
{
    Task<string> GenerateTokenAsync(User user);
}
