using RentalPlatform.Application.Interfaces;
using RentalPlatform.Domain.Entities;
using RentalPlatform.Domain.Enums;
using RentalPlatform.Domain.ValueObjects;
using BCrypt.Net;

namespace RentalPlatform.Application.UseCases.Identity;

public record RegisterUserCommand(
    string FirstName,
    string LastName,
    string Email,
    string Password,
    UserRole Role
);

public record RegisterUserResult(Guid UserId);

public class RegisterUserUseCase
{
    private readonly IUserRepository _users;

    public RegisterUserUseCase(IUserRepository users)
    {
        _users = users;
    }

    public async Task<RegisterUserResult> ExecuteAsync(RegisterUserCommand cmd)
    {
        var existing = await _users.GetByEmailAsync(cmd.Email.ToLowerInvariant());
        if (existing is not null)
            throw new InvalidOperationException("Email already registered");

        var passwordHash = BCrypt.Net.BCrypt.HashPassword(cmd.Password);

        var user = new User
        {
            Id = Guid.NewGuid(),
            FirstName = cmd.FirstName,
            LastName = cmd.LastName,
            Email = new Email(cmd.Email),
            PasswordHash = passwordHash,
            Role = cmd.Role,
            KycVerified = false,
            CreatedAt = DateTime.UtcNow
        };

        await _users.AddAsync(user);
        await _users.SaveChangesAsync();

        return new RegisterUserResult(user.Id);
    }
}
