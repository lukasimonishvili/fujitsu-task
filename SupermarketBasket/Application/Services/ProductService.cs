using Domain.Entities;
using SupermarketBasket.Application.Interfaces;
using SupermarketBasket.Domain.Interfaces;

namespace SupermarketBasket.Application.Services;

public class ProductService : IProductService
{
    private readonly IProductRepository _repository;

    public ProductService(IProductRepository repository)
    {
        _repository = repository;
    }

    public async Task<List<Product>> GetAllAsync()
    {
        return await _repository.GetAllAsync();
    }

    public async Task<Product?> GetByIdAsync(int id)
    {
        return await _repository.GetByIdAsync(id);
    }

    public async Task<Product> CreateAsync(string name, double weight, int quantity)
    {
        var product = new Product
        {
            Name = name,
            Weight = weight,
            Quantity = quantity
        };

        await _repository.AddAsync(product);
        return product;
    }

    public async Task<Product?> UpdateAsync(int id, string name, double weight, int quantity)
    {
        var product = await _repository.GetByIdAsync(id);

        if (product == null) return null;

        product.Name = name;
        product.Weight = weight;
        product.Quantity = quantity;

        await _repository.UpdateAsync(product);
        return product;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var product = await _repository.GetByIdAsync(id);

        if (product == null) return false;

        await _repository.DeleteAsync(product);
        return true;
    }
}