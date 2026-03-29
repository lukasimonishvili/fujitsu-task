using Application.DTOs;
using Application.Interfaces;
using Domain.Entities;
using Domain.Interfaces;
using FluentAssertions;
using Moq;

namespace Tests.Services;

public class UserServiceTests
{
    private readonly Mock<IUserRepository> _repoMock;
    private readonly Mock<IEmailService> _emailMock;
    private readonly UserService _service;

    public UserServiceTests()
    {
        _repoMock = new Mock<IUserRepository>();
        _emailMock = new Mock<IEmailService>();

        _service = new UserService(_repoMock.Object, _emailMock.Object);
    }

    [Fact]
    public async Task RegisterAsync_ShouldReturnToken_WhenUserIsValid()
    {
        // Arrange
        var dto = new RegisterDto
        {
            Email = "test@test.com",
            Password = "Password123!",
            ConfirmPassword = "Password123!"
        };

        _repoMock
            .Setup(r => r.GetByEmailAsync(dto.Email))
            .ReturnsAsync((User?)null);

        // Act
        var result = await _service.RegisterAsync(dto);

        // Assert
        result.Should().NotBeNull();
        _repoMock.Verify(r => r.AddAsync(It.IsAny<User>()), Times.Once);
        _emailMock.Verify(e => e.SendEmailAsync(
            dto.Email,
            It.IsAny<string>(),
            It.IsAny<string>()),
            Times.Once);
    }

    [Fact]
    public async Task RegisterAsync_ShouldReturnNull_WhenUserAlreadyExists()
    {
        // Arrange
        var dto = new RegisterDto
        {
            Email = "test@test.com",
            Password = "Password123!",
            ConfirmPassword = "Password123!"
        };

        _repoMock
            .Setup(r => r.GetByEmailAsync(dto.Email))
            .ReturnsAsync(new User());

        // Act
        var result = await _service.RegisterAsync(dto);

        // Assert
        result.Should().BeNull();
        _repoMock.Verify(r => r.AddAsync(It.IsAny<User>()), Times.Never);
    }

    [Fact]
    public async Task LoginAsync_ShouldReturnUser_WhenCredentialsAreCorrect()
    {
        // Arrange
        var dto = new LoginDto
        {
            Email = "test@test.com",
            Password = "Password123!"
        };

        var hashed = Convert.ToBase64String(
            System.Security.Cryptography.SHA256.HashData(
                System.Text.Encoding.UTF8.GetBytes(dto.Password)
            )
        );

        var user = new User
        {
            Email = dto.Email,
            PasswordHash = hashed,
            IsEmailConfirmed = true
        };

        _repoMock
            .Setup(r => r.GetByEmailAsync(dto.Email))
            .ReturnsAsync(user);

        // Act
        var result = await _service.LoginAsync(dto);

        // Assert
        result.Should().NotBeNull();
        result!.Email.Should().Be(dto.Email);
    }

    [Fact]
    public async Task LoginAsync_ShouldReturnNull_WhenPasswordIsWrong()
    {
        // Arrange
        var dto = new LoginDto
        {
            Email = "test@test.com",
            Password = "WrongPassword123!"
        };

        var user = new User
        {
            Email = dto.Email,
            PasswordHash = "somehash",
            IsEmailConfirmed = true
        };

        _repoMock
            .Setup(r => r.GetByEmailAsync(dto.Email))
            .ReturnsAsync(user);

        // Act
        var result = await _service.LoginAsync(dto);

        // Assert
        result.Should().BeNull();
    }

    [Fact]
    public async Task LoginAsync_ShouldReturnNull_WhenEmailNotConfirmed()
    {
        // Arrange
        var dto = new LoginDto
        {
            Email = "test@test.com",
            Password = "Password123!"
        };

        var hashed = Convert.ToBase64String(
            System.Security.Cryptography.SHA256.HashData(
                System.Text.Encoding.UTF8.GetBytes(dto.Password)
            )
        );

        var user = new User
        {
            Email = dto.Email,
            PasswordHash = hashed,
            IsEmailConfirmed = false
        };

        _repoMock
            .Setup(r => r.GetByEmailAsync(dto.Email))
            .ReturnsAsync(user);

        // Act
        var result = await _service.LoginAsync(dto);

        // Assert
        result.Should().BeNull();
    }

    [Fact]
    public async Task ConfirmEmailAsync_ShouldReturnTrue_WhenTokenIsValid()
    {
        // Arrange
        var token = "valid-token";

        var user = new User
        {
            EmailConfirmationToken = token,
            IsEmailConfirmed = false
        };

        _repoMock
            .Setup(r => r.GetByTokenAsync(token))
            .ReturnsAsync(user);

        // Act
        var result = await _service.ConfirmEmailAsync(token);

        // Assert
        result.Should().BeTrue();
        user.IsEmailConfirmed.Should().BeTrue();
        user.EmailConfirmationToken.Should().BeNull();

        _repoMock.Verify(r => r.UpdateAsync(user), Times.Once);
    }

    [Fact]
    public async Task ConfirmEmailAsync_ShouldReturnFalse_WhenTokenInvalid()
    {
        // Arrange
        _repoMock
            .Setup(r => r.GetByTokenAsync(It.IsAny<string>()))
            .ReturnsAsync((User?)null);

        // Act
        var result = await _service.ConfirmEmailAsync("invalid");

        // Assert
        result.Should().BeFalse();
    }
}