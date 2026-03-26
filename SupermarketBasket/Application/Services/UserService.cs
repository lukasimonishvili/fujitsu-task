using Application.DTOs;
using Application.Interfaces;
using Application.Validators;
using Domain.Entities;
using Domain.Interfaces;
using System.Security.Cryptography;
using System.Text;

public class UserService : IUserService
{
    private readonly IUserRepository _repository;
    private readonly IEmailService _emailService;

    public UserService(IUserRepository repository, IEmailService emailService)
    {
        _repository = repository;
        _emailService = emailService;
    }

    public async Task<string?> RegisterAsync(RegisterDto dto)
    {
        var validationError = RegisterValidator.Validate(dto);

        if (validationError != null)
            throw new Exception(validationError);

        var existing = await _repository.GetByEmailAsync(dto.Email);

        if (existing != null)
            return null;

        var token = Guid.NewGuid().ToString();

        var user = new User
        {
            Email = dto.Email,
            PasswordHash = HashPassword(dto.Password),
            IsEmailConfirmed = false,
            EmailConfirmationToken = token
        };

        await _repository.AddAsync(user);
        var confirmationLink = $"http://localhost:4200/confirm-email?token={token}";

        await _emailService.SendEmailAsync(
            dto.Email,
            "Confirm your email",
            $"Click <a href='{confirmationLink}'>here</a> to confirm your email."
        );

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

    public async Task<User?> LoginAsync(LoginDto dto)
    {
        var validationError = LoginValidator.Validate(dto);

        if (validationError != null)
            throw new Exception(validationError);

        var user = await _repository.GetByEmailAsync(dto.Email);

        if (user == null)
            return null;

        if (!user.IsEmailConfirmed)
            return null;

        var hashedPassword = HashPassword(dto.Password);

        if (user.PasswordHash != hashedPassword)
            return null;

        return user;
    }
}