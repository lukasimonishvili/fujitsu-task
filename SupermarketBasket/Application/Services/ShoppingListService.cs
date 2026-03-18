using Application.DTOs;
using Domain.Entities;
using Domain.Interfaces;

public class ShoppingListService : IShoppingListService
{
    private readonly IShoppingListRepository _repository;

    public ShoppingListService(IShoppingListRepository repository)
    {
        _repository = repository;
    }

    public async Task<List<ShoppingListItemDto>> GetUserListAsync(int userId)
    {
        var items = await _repository.GetByUserIdAsync(userId);

        return items.Select(x => new ShoppingListItemDto
        {
            ProductId = x.ProductId,
            ProductName = x.Product.Name,
            Weight = x.Product.Weight,
            Quantity = x.Quantity
        }).ToList();
    }

    public async Task AddAsync(int userId, int productId, int quantity)
    {
        var existing = await _repository.GetItemAsync(userId, productId);

        if (existing == null)
        {
            await _repository.AddAsync(new ShoppingListItem
            {
                UserId = userId,
                ProductId = productId,
                Quantity = quantity
            });
        }
        else
        {
            existing.Quantity += quantity;
            await _repository.UpdateAsync(existing);
        }
    }

    public async Task<bool> UpdateQuantityAsync(int userId, int productId, int quantity)
    {
        var existing = await _repository.GetItemAsync(userId, productId);

        if (existing == null)
            return false;

        if (quantity <= 0)
        {
            await _repository.DeleteAsync(existing);
            return true;
        }

        existing.Quantity = quantity;

        await _repository.UpdateAsync(existing);

        return true;
    }

    public async Task<bool> RemoveAsync(int userId, int productId)
    {
        var existing = await _repository.GetItemAsync(userId, productId);

        if (existing == null)
            return false;

        await _repository.DeleteAsync(existing);
        return true;
    }
}