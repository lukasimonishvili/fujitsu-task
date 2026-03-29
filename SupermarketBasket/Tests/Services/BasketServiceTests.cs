using Application.Services;
using Domain.Entities;
using Domain.Interfaces;
using FluentAssertions;
using Moq;

namespace Tests.Services;

public class BasketServiceTests
{
    private readonly Mock<IShoppingListRepository> _repositoryMock;
    private readonly BasketService _service;

    public BasketServiceTests()
    {
        _repositoryMock = new Mock<IShoppingListRepository>();
        _service = new BasketService(_repositoryMock.Object);
    }

    [Fact]
    public async Task BuildBasketAsync_ShouldNotExceedMaxWeight()
    {
        // Arrange
        var items = new List<ShoppingListItem>
        {
            new ShoppingListItem
            {
                ProductId = 1,
                Quantity = 5,
                Product = new Product { Id = 1, Name = "Heavy", Weight = 10 }
            },
            new ShoppingListItem
            {
                ProductId = 2,
                Quantity = 5,
                Product = new Product { Id = 2, Name = "Light", Weight = 2 }
            }
        };

        _repositoryMock
            .Setup(r => r.GetByUserIdAsync(It.IsAny<int>()))
            .ReturnsAsync(items);

        // Act
        var result = await _service.BuildBasketAsync(1);

        var totalWeight = result.Sum(x => x.Weight * x.Quantity);

        // Assert
        totalWeight.Should().BeLessThanOrEqualTo(20);
    }

    [Fact]
    public async Task BuildBasketAsync_ShouldPrioritizeHeavierItems()
    {
        // Arrange
        var items = new List<ShoppingListItem>
        {
            new ShoppingListItem
            {
                ProductId = 1,
                Quantity = 2,
                Product = new Product { Id = 1, Name = "Light", Weight = 2 }
            },
            new ShoppingListItem
            {
                ProductId = 2,
                Quantity = 2,
                Product = new Product { Id = 2, Name = "Heavy", Weight = 10 }
            }
        };

        _repositoryMock
            .Setup(r => r.GetByUserIdAsync(It.IsAny<int>()))
            .ReturnsAsync(items);

        // Act
        var result = await _service.BuildBasketAsync(1);

        // Assert
        result.First().Weight.Should().Be(10);
    }

    [Fact]
    public async Task BuildBasketAsync_ShouldSkipItemsHeavierThanMaxWeight()
    {
        // Arrange
        var items = new List<ShoppingListItem>
        {
            new ShoppingListItem
            {
                ProductId = 1,
                Quantity = 1,
                Product = new Product { Id = 1, Name = "TooHeavy", Weight = 50 }
            }
        };

        _repositoryMock
            .Setup(r => r.GetByUserIdAsync(It.IsAny<int>()))
            .ReturnsAsync(items);

        // Act
        var result = await _service.BuildBasketAsync(1);

        // Assert
        result.Should().BeEmpty();
    }

    [Fact]
    public async Task BuildBasketAsync_ShouldRespectItemQuantityLimits()
    {
        // Arrange
        var items = new List<ShoppingListItem>
        {
            new ShoppingListItem
            {
                ProductId = 1,
                Quantity = 10,
                Product = new Product { Id = 1, Name = "Medium", Weight = 5 }
            }
        };

        _repositoryMock
            .Setup(r => r.GetByUserIdAsync(It.IsAny<int>()))
            .ReturnsAsync(items);

        // Act
        var result = await _service.BuildBasketAsync(1);

        var item = result.First();

        // Assert
        item.Quantity.Should().Be(4);
    }
}