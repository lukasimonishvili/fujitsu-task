using Application.DTOs;
using System.Text.RegularExpressions;

namespace Application.Validators;

public static class LoginValidator
{
    public static string? Validate(LoginDto dto)
    {
        if (string.IsNullOrWhiteSpace(dto.Email))
            return "Email is required";

        if (!IsValidEmail(dto.Email))
            return "Invalid email format";

        if (string.IsNullOrWhiteSpace(dto.Password))
            return "Password is required";

        return null;
    }

    private static bool IsValidEmail(string email)
    {
        return Regex.IsMatch(email,
            @"^[^@\s]+@[^@\s]+\.[^@\s]+$");
    }
}