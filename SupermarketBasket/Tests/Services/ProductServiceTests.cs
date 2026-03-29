using Domain.Entities;
using FluentAssertions;
using Moq;
using SupermarketBasket.Application.Services;
using SupermarketBasket.Domain.Interfaces;

namespace Tests.Services;

public class ProductServiceTests
{
    private readonly Mock<IProductRepository> _repoMock;
    private readonly ProductService _service;

    public ProductServiceTests()
    {
        _repoMock = new Mock<IProductRepository>();
        _service = new ProductService(_repoMock.Object);
    }

    [Fact]
    public async Task GetAllAsync_ShouldReturnProducts()
    {
        // Arrange
        var products = new List<Product>
        {
            new Product { Id = 1, Name = "Test", Weight = 1, Quantity = 1 }
        };

        _repoMock.Setup(r => r.GetAllAsync()).ReturnsAsync(products);

        // Act
        var result = await _service.GetAllAsync();

        // Assert
        result.Should().HaveCount(1);
    }

    [Fact]
    public async Task CreateAsync_ShouldAddProduct()
    {
        // Act
        var result = await _service.CreateAsync("Apple", 1.5, 10);

        // Assert
        result.Name.Should().Be("Apple");
        _repoMock.Verify(r => r.AddAsync(It.IsAny<Product>()), Times.Once);
    }

    [Fact]
    public async Task UpdateAsync_ShouldReturnNull_WhenProductNotFound()
    {
        // Arrange
        _repoMock.Setup(r => r.GetByIdAsync(1)).ReturnsAsync((Product?)null);

        // Act
        var result = await _service.UpdateAsync(1, "New", 2, 5);

        // Assert
        result.Should().BeNull();
    }

    [Fact]
    public async Task UpdateAsync_ShouldUpdateProduct_WhenExists()
    {
        // Arrange
        var product = new Product { Id = 1, Name = "Old", Weight = 1, Quantity = 1 };

        _repoMock.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(product);

        // Act
        var result = await _service.UpdateAsync(1, "New", 2, 5);

        // Assert
        result!.Name.Should().Be("New");
        result.Weight.Should().Be(2);
        result.Quantity.Should().Be(5);

        _repoMock.Verify(r => r.UpdateAsync(product), Times.Once);
    }

    [Fact]
    public async Task DeleteAsync_ShouldReturnFalse_WhenProductNotFound()
    {
        // Arrange
        _repoMock.Setup(r => r.GetByIdAsync(1)).ReturnsAsync((Product?)null);

        // Act
        var result = await _service.DeleteAsync(1);

        // Assert
        result.Should().BeFalse();
    }

    [Fact]
    public async Task DeleteAsync_ShouldDeleteProduct_WhenExists()
    {
        // Arrange
        var product = new Product { Id = 1 };

        _repoMock.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(product);

        // Act
        var result = await _service.DeleteAsync(1);

        // Assert
        result.Should().BeTrue();
        _repoMock.Verify(r => r.DeleteAsync(product), Times.Once);
    }
}