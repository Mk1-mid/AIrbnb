using Microsoft.EntityFrameworkCore;
using RentalPlatform.Application.Interfaces;
using RentalPlatform.Domain.Entities;

namespace RentalPlatform.Infrastructure.Persistence.Repositories;

public class UserRepository : IUserRepository
{
    private readonly RentalDbContext _context;

    public UserRepository(RentalDbContext context)
    {
        _context = context;
    }

    public Task<User?> GetByEmailAsync(string email) =>
        _context.Users.FirstOrDefaultAsync(u => u.Email.Value == email.ToLowerInvariant());

    public Task<User?> GetByIdAsync(Guid id) =>
        _context.Users.FirstOrDefaultAsync(u => u.Id == id);

    public Task AddAsync(User user) =>
        _context.Users.AddAsync(user).AsTask();

    public Task SaveChangesAsync() =>
        _context.SaveChangesAsync();
}
