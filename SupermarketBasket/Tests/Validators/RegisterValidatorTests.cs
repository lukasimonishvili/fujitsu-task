using Application.DTOs;
using Application.Validators;
using FluentAssertions;

namespace Tests.Validators;

public class RegisterValidatorTests
{
    [Fact]
    public void Validate_ShouldReturnNull_WhenInputIsValid()
    {
        var dto = new RegisterDto
        {
            Email = "test@test.com",
            Password = "Password123!",
            ConfirmPassword = "Password123!"
        };

        var result = RegisterValidator.Validate(dto);

        result.Should().BeNull();
    }

    [Fact]
    public void Validate_ShouldReturnError_WhenPasswordsDoNotMatch()
    {
        var dto = new RegisterDto
        {
            Email = "test@test.com",
            Password = "Password123!",
            ConfirmPassword = "Different123!"
        };

        var result = RegisterValidator.Validate(dto);

        result.Should().NotBeNull();
    }

    [Fact]
    public void Validate_ShouldReturnError_WhenPasswordIsWeak()
    {
        var dto = new RegisterDto
        {
            Email = "test@test.com",
            Password = "123",
            ConfirmPassword = "123"
        };

        var result = RegisterValidator.Validate(dto);

        result.Should().NotBeNull();
    }

    [Fact]
    public void Validate_ShouldReturnError_WhenEmailIsInvalid()
    {
        var dto = new RegisterDto
        {
            Email = "invalid-email",
            Password = "Password123!",
            ConfirmPassword = "Password123!"
        };

        var result = RegisterValidator.Validate(dto);

        result.Should().NotBeNull();
    }
}