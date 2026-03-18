using Domain.Entities;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Domain.Interfaces;

public class UserRepository : IUserRepository
{
    private readonly AppDbContext _context;

    public UserRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<User?> GetByEmailAsync(string email)
    {
        return await _context.Users.FirstOrDefaultAsync(u => u.Email == email);
    }

    public async Task AddAsync(User user)
    {
        _context.Users.Add(user);
        await _context.SaveChangesAsync();
    }

    public async Task<User?> GetByTokenAsync(string token)
    {
        return await _context.Users
            .FirstOrDefaultAsync(u => u.EmailConfirmationToken == token);
    }

    public async Task UpdateAsync(User user)
    {
        _context.Users.Update(user);
        await _context.SaveChangesAsync();
    }
}