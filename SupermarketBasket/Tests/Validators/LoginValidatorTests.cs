using Application.DTOs;
using Application.Validators;
using FluentAssertions;

namespace Tests.Validators;

public class LoginValidatorTests
{
    [Fact]
    public void Validate_ShouldReturnNull_WhenInputIsValid()
    {
        var dto = new LoginDto
        {
            Email = "test@test.com",
            Password = "Password123!"
        };

        var result = LoginValidator.Validate(dto);

        result.Should().BeNull();
    }

    [Fact]
    public void Validate_ShouldReturnError_WhenEmailIsInvalid()
    {
        var dto = new LoginDto
        {
            Email = "invalid-email",
            Password = "Password123!"
        };

        var result = LoginValidator.Validate(dto);

        result.Should().NotBeNull();
    }

    [Fact]
    public void Validate_ShouldReturnError_WhenPasswordIsEmpty()
    {
        var dto = new LoginDto
        {
            Email = "test@test.com",
            Password = ""
        };

        var result = LoginValidator.Validate(dto);

        result.Should().NotBeNull();
    }
}