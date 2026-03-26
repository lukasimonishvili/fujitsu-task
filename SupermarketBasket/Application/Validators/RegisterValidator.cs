using Application.DTOs;
using System.Text.RegularExpressions;

namespace Application.Validators;

public static class RegisterValidator
{
    public static string? Validate(RegisterDto dto)
    {
        if (string.IsNullOrWhiteSpace(dto.Email))
            return "Email is required";

        if (!IsValidEmail(dto.Email))
            return "Invalid email format";

        if (string.IsNullOrWhiteSpace(dto.Password))
            return "Password is required";

        if (!IsStrongPassword(dto.Password))
            return "Password must be at least 8 characters and include uppercase, lowercase, number, and special character";

        if (dto.Password != dto.ConfirmPassword)
            return "Passwords do not match";

        return null;
    }

    private static bool IsValidEmail(string email)
    {
        return Regex.IsMatch(email,
            @"^[^@\s]+@[^@\s]+\.[^@\s]+$");
    }

    private static bool IsStrongPassword(string password)
    {
        return Regex.IsMatch(password,
            @"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[\W_]).{8,}$");
    }
}