using RentalPlatform.Application.Interfaces;
using RentalPlatform.Domain.Entities;
using RentalPlatform.Domain.Enums;

namespace RentalPlatform.Application.UseCases.Identity;

public record LoginUserCommand(string Email, string Password);

public record LoginUserResult(Guid UserId, string Email, UserRole Role, string Token);

public class LoginUserUseCase
{
    private readonly IUserRepository _users;
    private readonly ITokenService _tokenService;

    public LoginUserUseCase(IUserRepository users, ITokenService tokenService)
    {
        _users = users;
        _tokenService = tokenService;
    }

    public async Task<LoginUserResult> ExecuteAsync(LoginUserCommand cmd)
    {
        var user = await _users.GetByEmailAsync(cmd.Email.ToLowerInvariant())
            ?? throw new InvalidOperationException("Invalid credentials");

        if (!BCrypt.Net.BCrypt.Verify(cmd.Password, user.PasswordHash))
            throw new InvalidOperationException("Invalid credentials");

        var token = await _tokenService.GenerateTokenAsync(user);

        return new LoginUserResult(user.Id, user.Email.Value, user.Role, token);
    }
}
