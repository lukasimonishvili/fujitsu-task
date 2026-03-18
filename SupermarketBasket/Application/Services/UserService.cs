using Application.Interfaces;
using Domain.Entities;
using Domain.Interfaces;
using System.Security.Cryptography;
using System.Text;

public class UserService : IUserService
{
    private readonly IUserRepository _repository;

    public UserService(IUserRepository repository)
    {
        _repository = repository;
    }

    public async Task<string?> RegisterAsync(string email, string password)
    {
        var existing = await _repository.GetByEmailAsync(email);

        if (existing != null)
            return null;

        var token = Guid.NewGuid().ToString();

        var user = new User
        {
            Email = email,
            PasswordHash = HashPassword(password),
            IsEmailConfirmed = false,
            EmailConfirmationToken = token
        };

        await _repository.AddAsync(user);

        return token;
    }

    private string HashPassword(string password)
    {
        using var sha256 = SHA256.Create();
        var bytes = Encoding.UTF8.GetBytes(password);
        var hash = sha256.ComputeHash(bytes);

        return Convert.ToBase64String(hash);
    }

    public async Task<bool> ConfirmEmailAsync(string token)
    {
        var user = await _repository.GetByTokenAsync(token);

        if (user == null)
            return false;

        user.IsEmailConfirmed = true;
        user.EmailConfirmationToken = null;

        await _repository.UpdateAsync(user);

        return true;
    }

    public async Task<User?> LoginAsync(string email, string password)
    {
        var user = await _repository.GetByEmailAsync(email);

        if (user == null)
            return null;

        if (!user.IsEmailConfirmed)
            return null;

        var hashedPassword = HashPassword(password);

        if (user.PasswordHash != hashedPassword)
            return null;

        return user;
    }
}