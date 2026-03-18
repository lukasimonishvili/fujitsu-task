using Domain.Entities;

namespace SupermarketBasket.Application.Interfaces;

public interface IProductService
{
    Task<List<Product>> GetAllAsync();
    Task<Product?> GetByIdAsync(int id);
    Task<Product> CreateAsync(string name, double weight, int quantity);
    Task<Product?> UpdateAsync(int id, string name, double weight, int quantity);
    Task<bool> DeleteAsync(int id);
}