using Domain.Entities;
using Domain.Interfaces;
using FluentAssertions;
using Moq;

namespace Tests.Services;

public class ShoppingListServiceTests
{
    private readonly Mock<IShoppingListRepository> _repoMock;
    private readonly ShoppingListService _service;

    public ShoppingListServiceTests()
    {
        _repoMock = new Mock<IShoppingListRepository>();
        _service = new ShoppingListService(_repoMock.Object);
    }

    [Fact]
    public async Task GetUserListAsync_ShouldReturnMappedDtos()
    {
        // Arrange
        var items = new List<ShoppingListItem>
        {
            new ShoppingListItem
            {
                ProductId = 1,
                Quantity = 2,
                Product = new Product { Name = "Apple", Weight = 1.5 }
            }
        };

        _repoMock.Setup(r => r.GetByUserIdAsync(1)).ReturnsAsync(items);

        // Act
        var result = await _service.GetUserListAsync(1);

        // Assert
        result.Should().HaveCount(1);
        result[0].ProductName.Should().Be("Apple");
    }

    [Fact]
    public async Task AddAsync_ShouldCreateItem_WhenNotExists()
    {
        // Arrange
        _repoMock
            .Setup(r => r.GetItemAsync(1, 1))
            .ReturnsAsync((ShoppingListItem?)null);

        // Act
        await _service.AddAsync(1, 1, 3);

        // Assert
        _repoMock.Verify(r => r.AddAsync(It.IsAny<ShoppingListItem>()), Times.Once);
    }

    [Fact]
    public async Task AddAsync_ShouldIncreaseQuantity_WhenExists()
    {
        // Arrange
        var existing = new ShoppingListItem { Quantity = 2 };

        _repoMock
            .Setup(r => r.GetItemAsync(1, 1))
            .ReturnsAsync(existing);

        // Act
        await _service.AddAsync(1, 1, 3);

        // Assert
        existing.Quantity.Should().Be(5);
        _repoMock.Verify(r => r.UpdateAsync(existing), Times.Once);
    }

    [Fact]
    public async Task UpdateQuantityAsync_ShouldReturnFalse_WhenItemNotFound()
    {
        // Arrange
        _repoMock
            .Setup(r => r.GetItemAsync(1, 1))
            .ReturnsAsync((ShoppingListItem?)null);

        // Act
        var result = await _service.UpdateQuantityAsync(1, 1, 5);

        // Assert
        result.Should().BeFalse();
    }

    [Fact]
    public async Task UpdateQuantityAsync_ShouldDelete_WhenQuantityIsZero()
    {
        // Arrange
        var existing = new ShoppingListItem();

        _repoMock
            .Setup(r => r.GetItemAsync(1, 1))
            .ReturnsAsync(existing);

        // Act
        var result = await _service.UpdateQuantityAsync(1, 1, 0);

        // Assert
        result.Should().BeTrue();
        _repoMock.Verify(r => r.DeleteAsync(existing), Times.Once);
    }

    [Fact]
    public async Task UpdateQuantityAsync_ShouldUpdate_WhenQuantityPositive()
    {
        // Arrange
        var existing = new ShoppingListItem();

        _repoMock
            .Setup(r => r.GetItemAsync(1, 1))
            .ReturnsAsync(existing);

        // Act
        var result = await _service.UpdateQuantityAsync(1, 1, 5);

        // Assert
        result.Should().BeTrue();
        existing.Quantity.Should().Be(5);
        _repoMock.Verify(r => r.UpdateAsync(existing), Times.Once);
    }

    [Fact]
    public async Task RemoveAsync_ShouldReturnFalse_WhenItemNotFound()
    {
        // Arrange
        _repoMock
            .Setup(r => r.GetItemAsync(1, 1))
            .ReturnsAsync((ShoppingListItem?)null);

        // Act
        var result = await _service.RemoveAsync(1, 1);

        // Assert
        result.Should().BeFalse();
    }

    [Fact]
    public async Task RemoveAsync_ShouldDelete_WhenItemExists()
    {
        // Arrange
        var existing = new ShoppingListItem();

        _repoMock
            .Setup(r => r.GetItemAsync(1, 1))
            .ReturnsAsync(existing);

        // Act
        var result = await _service.RemoveAsync(1, 1);

        // Assert
        result.Should().BeTrue();
        _repoMock.Verify(r => r.DeleteAsync(existing), Times.Once);
    }
}