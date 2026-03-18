using Application.DTOs;
using Domain.Entities;

public interface IShoppingListService
{
    Task<List<ShoppingListItemDto>> GetUserListAsync(int userId);
    Task AddAsync(int userId, int productId, int quantity);
    Task<bool> RemoveAsync(int userId, int productId);
    Task<bool> UpdateQuantityAsync(int userId, int productId, int quantity);
}